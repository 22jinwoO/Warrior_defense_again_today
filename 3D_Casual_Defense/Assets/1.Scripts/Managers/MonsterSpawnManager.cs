using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSpawnManager : Singleton<MonsterSpawnManager>
{

    [Header("몬스터 유닛 팩토리들")]
    [SerializeField]
    private AbsMonsterUnitFactory[] monsterUnitFactorys;


    [Header("몬스터 네비메쉬 서페이스")]
    public NavMeshSurface nms;

    [Header("오크 프리팹 리스트 인덱스")]
    private int orcListIndex = 0;

    [Header("오크 프리팹 리스트")]
    public List<Orc> orcList = new List<Orc>();

    [Header("오크 프리팹 넣어놓는 오브젝트")]
    [SerializeField]
    private Transform orcPrefab_Objects;
    //====================================================

    [Header("몬스터 스폰 포인트")]
    [SerializeField]
    private Transform spawnPoint;

    [Header("스폰된 몬스터")]
    [SerializeField]
    private MonsterUnitClass spawnMonster;  //스폰된 몬스터

    [Header("현재 웨이브")]
    public Wave currentWave;   // 현재 웨이브

    [Header("웨이브 몬스터 종류 배열 인덱스")]
    [SerializeField]
    private int currentMonsterIndex = 0;  // 웨이브 몬스터 종류 배열 인덱스

    [Header("현재 웨이브에서 생성한 몬스터 숫자")]
    [SerializeField]
    private int spawnMonsterCount = 0;      // 현재 웨이브에서 생성한 몬스터 숫자

    [Header("몬스터 종류에 해당하는 팩토리 딕셔너리 (Key :string / Value : AbsMonsterUnitFactory)")]
    public SerializableDictionary<string, AbsMonsterUnitFactory> d_MonsterDictonary=new SerializableDictionary<string, AbsMonsterUnitFactory>();

    [Header("웨이브 시스템 변수")]
    public WaveSystem waveSys;

    [Header("웨이브 제이슨 파일")]
    public TextAsset data;  // Json 데이터 에셋  이 파일은 유니티 상에서 드래그해서 넣어줍니다.

    [Header("웨이브 제이슨 파일 변환 데이터형")]
    public AllData datas;  // 변환 데이터형

    [Header("텍스트 매니저 변수")]
    [SerializeField]
    private Stage1_TextManager txtManager;

    [Header("UI 팝업 매니저 변수")]
    [SerializeField]
    private UI_PopUpManager uiManager;

    [Header("스폰된 몬스터들 리스트")]
    [SerializeField]
    private List<MonsterUnitClass> spawnList = new List<MonsterUnitClass>();

    [Header("웨이브 텍스트 문구 대기시간")]
    [SerializeField]
    private float time;


    private void Awake()
    {
        SetDicMonsterFactory();     // 몬스터 팩토리들 키, 값 할당하는 함수


        waveSys = GetComponent<WaveSystem>();

        datas = JsonUtility.FromJson<AllData>(data.text);   // Json파일의 텍스트들을 datas 값에 넣어주고


        txtManager = Stage1_TextManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // 웨이브 몬스터를 다 처지 했을때, 3초의 대기시간이 주어짐
        if (time < 3f && waveSys.currentWaveIndex != -1 && currentWave.wave_maxMonsterCount == currentWave.deathMonsterCnt)
        {
            txtManager.currentWave_IntervalTxt.gameObject.SetActive(true);
            txtManager.currentWave_IntervalTxt.text = $"휼륭합니다! {waveSys.currentWaveIndex+1}차 침공을 물리쳤습니다!";
            time += Time.deltaTime;
        }

        // 여기서 현재 웨이브 시작함
        else if (time >= 3f && waveSys.currentWaveIndex != -1 && currentWave.wave_maxMonsterCount == currentWave.deathMonsterCnt)
        {

            txtManager.currentWave_IntervalTxt.gameObject.SetActive(false);
            currentWave.deathMonsterCnt = 0;
            time = 0f;

            // 웨이브 시작하는 함수
            waveSys.StartWave();
        }
    }

    private void SetDicMonsterFactory()
    {
        d_MonsterDictonary.Add(key : "orc_warr01", value : monsterUnitFactorys[0]);
        d_MonsterDictonary.Add(key: "orc_hunt01", value: monsterUnitFactorys[1]);
        d_MonsterDictonary.Add(key: "orc_sham01", value: monsterUnitFactorys[2]);
        d_MonsterDictonary.Add(key: "orc_boss01", value: monsterUnitFactorys[3]);
    }

    public void StartWave(Wave wave)
    {
        txtManager.currentWaveTxt.text = $"웨이브 {waveSys.currentWaveIndex+1}";
        txtManager.currentWave_IntervalTxt.gameObject.SetActive(true);

        // 현재 웨이브 몬스터 종류 리스트 초기화
        currentWave.wave_monsterClasses.Clear();

        // 매개변수로 받아온 웨이브 정보 저장
        currentWave = wave;

        txtManager.currentWave_IntervalTxt.text = $"오크 침공까지 {currentWave.wave_StartTime} 초";

        // 현재 웨이브 시작
        StartCoroutine(SpawnMonster());
    }

    private IEnumerator SpawnMonster()    // 몬스터 생성해주는 코루틴 함수
    {
        int repeatNum = 0;

        // 웨이브 대기시간이 0초 보다 클 동안 오크 침공 메세지 텍스트 활성화
        while (currentWave.wave_StartTime > 0)
        {
            currentWave.wave_StartTime--;
            txtManager.currentWave_IntervalTxt.text = $"오크 침공까지 {currentWave.wave_StartTime} 초";
            yield return new WaitForSeconds(1f);

        }

        // 웨이브 몬스터 생성주기동안 반복
        while (repeatNum<currentWave.wave_RepeatNum)
        {
            int currentMonsterCount = 0;
    
            txtManager.currentWave_IntervalTxt.text = $"오크들이 성을 향해 돌격하고 있습니다!";
            
            // 현재 웨이브에서 생성되어야 하는 몬스터의 숫자만큼 몬스터 생성
            while (currentMonsterCount < currentWave.wave_monsterClasses.Count)
                {

                    // 현재 웨이브에서 생성된 몬스터 인덱스 수를 매개변수로 넣어줌
                    CheckSpawnMonster(currentMonsterCount);

                    currentMonsterCount++;

                    spawnMonsterCount++;

                    yield return new WaitForSeconds(currentWave.wave_interval);
                    if(spawnMonsterCount==2)
                    {
                        txtManager.currentWave_IntervalTxt.gameObject.SetActive(false);
                    }
                }
            repeatNum++;
        }
        yield return null;
        orcListIndex = 0;
        spawnMonsterCount = 0;
    }

    #region # CheckSpawnMonster() 함수 :  구상생산자 호출하는 함수
    private void CheckSpawnMonster(int monsterCnt)
    {
        // 몬스터 종류 리스트의 인덱스로 monsterCnt를 넣고, 인덱스에 해당하는 리스트 값(UnitId - string 자료형) 을 매개변수로 전달
        CreateMonster(monsterUnitId : currentWave.wave_monsterClasses[monsterCnt]);

    }
    #endregion

    #region # CreateMonster() 함수 :  몬스터 생산자 함수
    private void CreateMonster(string monsterUnitId)
    {
        // 몬스터 종류 리스트 인덱스에 해당하는 팩토리 찾은 후 팩토리.CreateMonster로 반환받은 몬스터 소환하기
        // 몬스터 종류 리스트에서 인덱스가 증가하면서 종류 리스트 스트링을 인덱스 값에 해당하는 팩토리를 찾아서 createmonsterUnit해주기 

        //  유닛 생산자
        spawnMonster = d_MonsterDictonary[monsterUnitId].CreateMonsterUnit();

        // 초깃값 할당
        spawnMonster.InitUnitInfoSetting(UnitDataManager.Instance._unitInfo_Dictionary[monsterUnitId]);
        spawnMonster.transform.position = spawnPoint.position;

        spawnMonster.gameObject.SetActive(true);

        spawnMonster._nav.SetDestination(spawnMonster.castleTr.position);

        print("네비메쉬 위에 있는지 확인" + spawnMonster._nav.isOnNavMesh);
        //spawnMonster.transform.position = spawnPoint;
        spawnMonster.gameObject.name = monsterUnitId;

        spawnList.Add(spawnMonster);

        spawnMonster = null;    //spawnMonster 변수 값 초기화
    }
    #endregion

    #region # CreateMonster() 함수 :  스폰된 모든 몬스터들 사망시키는 함수
    public void DeadAllMonster()
    {
        for(int i=0; i<spawnList.Count; i++)
        {
            spawnList[i]._unitData.hp = 0;
            spawnList[i].actUnitCs.DeadCheck();
        }
        spawnList.Clear();

    }
    #endregion

    #region # SkipWaveStartTime() 함수 :  웨이브 대기시간 스킵하는 함수
    public void SkipWaveStartTime()
    {
        currentWave.wave_StartTime = 0;
    }
    #endregion

    #region # C_DynamicBake() 함수 :  몬스터 네비메쉬 서페이스 동적 베이크 실행하는 코루틴 함수

    private IEnumerator C_DynamicBake() // 동적 베이크 실행하는 코루틴 함수
    {
        yield return new WaitForSeconds(5f);
        print("초기화");
        nms.BuildNavMesh();
        StartCoroutine(C_DynamicBake());


    }
    #endregion

}

[System.Serializable]
public class AllData
{
    public WaveData[] waveDatas;    // waveDatas 이름은 시트 이름이랑 같아야 함
}


[System.Serializable]
public class WaveData   // WaveDatas 구글 시트에 해당하는 값을 갖고오기 위한 클래스
{
    public int waveNum; // 웨이브 단계
    public int waveStartTime;   // 웨이브 대기 시간
    public string waveName; // 웨이브 이름
    public int interval;    // 몬스터 생성 주기
    public string character1;   // 첫번째 종류 몬스터
    public string character2;   // 두번째 종류 몬스터
    public string character3;   // 세번째 종류 몬스터
    public string character4;   // 네번째 종류 몬스터
    public string character5;   // 다섯번째 종류 몬스터
    public int maxMonster_Count;    // 최대 생성되는 몬스터 수
    public int repeatNum;   // 생성주기 반복 횟수
}