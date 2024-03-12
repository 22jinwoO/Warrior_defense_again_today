using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSpawnManager : Singleton<MonsterSpawnManager>
{


    [SerializeField]
    AbsMonsterUnitFactory[] monsterUnitFactorys;

    public NavMeshSurface nms;

    [Header("오크 프리팹 리스트 인덱스")]
    private int orcListIndex = 0;

    [Header("오크 프리팹 리스트")]
    public List<Orc> orcList = new List<Orc>();

    [Header("오크 프리팹 넣어놓는 오브젝트")]
    [SerializeField]
    private Transform orcPrefab_Objects;
    //====================================================

    [SerializeField]
    private MonsterUnitClass spawnMonster;  //스폰된 몬스터

    public Wave currentWave;   // 현재 웨이브

    [SerializeField]
    private int currentMonsterIndex = 0;  // 웨이브 몬스터 종류 배열 인덱스

    [SerializeField]
    // 현재 웨이브에서 생성한 몬스터 숫자
    private int spawnMonsterCount = 0;


    [Header("몬스터 종류")]
    public MonsterUnitClass[] monsters;

    [Header("몬스터 종류 딕셔너리 (Key :string / Value : MonsterUnitClass)")]
    public SerializableDictionary<string, AbsMonsterUnitFactory> d_MonsterDictonary=new SerializableDictionary<string, AbsMonsterUnitFactory>();

    public WaveSystem waveSys;


    public TextAsset data;  // Json 데이터 에셋  이 파일은 유니티 상에서 드래그해서 넣어줍니다.

    public AllData datas;  // 변환 데이터형


    private Stage1_TextManager txtManager;

    [SerializeField]
    private UI_PopUpManager uiManager;



    //public Text intervalTxt;

    private void Awake()
    {
        //print(monsters[0]);
        //print(d_MonsterDictonary);
        d_MonsterDictonary.Add("orc_warr01", monsterUnitFactorys[0]);
        d_MonsterDictonary.Add("orc_hunt01", monsterUnitFactorys[1]);
        d_MonsterDictonary.Add("orc_sham01", monsterUnitFactorys[2]);
        d_MonsterDictonary.Add("orc_boss01", monsterUnitFactorys[3]);
        //d_MonsterDictonary.Add("orc_hunt01", monsterUnitFactorys[1].monsterPrefab);

        waveSys=GetComponent<WaveSystem>();

        datas = JsonUtility.FromJson<AllData>(data.text);   // Json파일의 텍스트들을 datas 값에 넣어주고

        //foreach (var item in datas.waveDatas)
        //{
        //    print(datas.waveDatas[3].interval);
        //}
        //print("이거 오류 맞음??"+nms.name);
        txtManager = Stage1_TextManager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        // 몬스터 네비메쉬 서페이스 5초마다 초기화
        //StartCoroutine(C_DynamicBake());
        //StartCoroutine(SpawnMonster());
        //waveSys.StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(SpawnMonster());
            for (int i = 0; i < orcList.Count; i++)
            {
                orcList[i].gameObject.SetActive(false);
            }
            waveSys.currentWaveIndex = 8;
            currentWave.wave_maxMonsterCount = currentWave.deathMonsterCnt;
            
        }

        // 여기서 현재 웨이브 시작함
        if (waveSys.currentWaveIndex!=-1&& currentWave.wave_maxMonsterCount==currentWave.deathMonsterCnt)
        {
            print("여기서 시작인가요");
            print($"{waveSys.currentWaveIndex}");
            currentWave.deathMonsterCnt = 0;
            print("웨이브 변경");
            waveSys.StartWave();
        }
    }


    public void StartWave(Wave wave)
    {
        txtManager.currentWave_IntervalTxt.gameObject.SetActive(true);
        // 현재 웨이브 몬스터 종류 리스트 초기화
        currentWave.wave_monsterClasses.Clear();

        // 매개변수로 받아온 웨이브 정보 저장
        currentWave = wave;
        //intervalTxt.text = currentWave.wave_interval.ToString();
        txtManager.currentWave_IntervalTxt.text = $"오크 침공까지 {currentWave.wave_StartTime} 초";
        // 현재 웨이브 시작
        StartCoroutine(SpawnMonster());
    }

    private IEnumerator SpawnMonster()    // 몬스터 생성해주는 코루틴 함수
    {
        //txtManager.currentWaveTxt.text = currentWave.wave_Name;
        //txtManager.CountDeadMonster();


        int repeatNum = 0;
        int waveTime = currentWave.wave_StartTime;
        //Debug.LogWarning("대기시간 :"+currentWave.wave_StartTime);
        while (currentWave.wave_StartTime > 0)
        {
            //intervalTxt.text=currentWave.wave_StartTime.ToString();
            currentWave.wave_StartTime--;
            //txtManager.currentWave_IntervalTxt.text = "웨이브 대기 시간 : " + currentWave.wave_StartTime.ToString();
            yield return new WaitForSeconds(1f);
            if (currentWave.wave_StartTime== waveTime - 2&& txtManager.currentWave_IntervalTxt.gameObject.activeSelf)
            {
                txtManager.currentWave_IntervalTxt.gameObject.SetActive(false);
            }
        }
        //yield return new WaitForSeconds(currentWave.wave_interval);

        txtManager.currentWave_IntervalTxt.gameObject.SetActive(true);
        while (repeatNum<currentWave.wave_RepeatNum)
        {
            int currentMonsterCount = 0;
    
            txtManager.currentWave_IntervalTxt.text = $"오크들이 진영을 향해 돌격하기 시작합니다!";
            
            // 현재 웨이브에서 생성되어야 하는 몬스터의 숫자만큼 몬스터 생성
            while (currentMonsterCount < currentWave.wave_monsterClasses.Count)
                {

                    CheckSpawnMonster(currentMonsterCount);
                    currentMonsterCount++;
                    spawnMonsterCount++;

                    //Debug.LogWarning("currentMonsterCount " + currentMonsterCount);
                    yield return new WaitForSeconds(currentWave.wave_interval);
                    if(txtManager.currentWave_IntervalTxt.gameObject.activeSelf)
                    {
                    txtManager.currentWave_IntervalTxt.gameObject.SetActive(false);
                    }
                }

            //Debug.LogWarning("repeatNum " + repeatNum);
            repeatNum++;
        }
        yield return null;
        orcListIndex = 0;
        spawnMonsterCount = 0;
    }

    private void CheckSpawnMonster(int monsterCnt)
    {
        // 오크 종류 확인하는 게 아니라 딕셔너리 키 밸류를 키로 몬스터아이디, 밸류로 해당 몬스터 팩토리 연결하도록 해서 몬스터팩토리.createMonster하도록설정하기
        //List<MonsterUnitClass> monsterSpawnList = currentWave.wave_monsterClasses;
        print(currentWave.wave_monsterClasses[monsterCnt]);
        CreateMonster(currentWave.wave_monsterClasses[monsterCnt]);
        //CreateOrc();
        //switch (monsterSpawnFactorys[monsterKindsIndex]) // 몬스터 종류 배열의 현재 몬스터 배열 인덱스
        //{
        //    case Orc:

        //        break;

        //}
    }

    //오크 생산자
    private void CreateMonster(string monsterUnitId)
    {
        // 몬스터 종류 리스트 인덱스에 해당하는 팩토리 찾은 후 팩토리.CreateMonster로 반환받은 몬스터 소환하기
        // 몬스터 종류 리스트에서 인덱스가 증가하면서 종류 리스트 스트링을 인덱스 값에 해당하는 팩토리를 찾아서 createmonsterUnit해주기 
        //GameObject spawnOrc = null;
        bool cantSetActive = false;
        //  유닛 생산자
        spawnMonster = d_MonsterDictonary[monsterUnitId].CreateMonsterUnit();
        spawnMonster.InitUnitInfoSetting(UnitDataManager.Instance._unitInfo_Dictionary[monsterUnitId]);
        spawnMonster.transform.position = d_MonsterDictonary[monsterUnitId].spawnPoint;
        print("네비메쉬 위에 있는지 확인" + spawnMonster._nav.isOnNavMesh);
        //spawnMonster.transform.position = spawnPoint;
        spawnMonster.gameObject.name = monsterUnitId;
        spawnMonster.transform.SetParent(orcPrefab_Objects);
        spawnMonster = null;    //spawnMonster 변수 값 초기화
    }

    //오크 생산자
    private void CreateOrc()
    {
        //GameObject spawnOrc = null;
        bool cantSetActive = false;
        //  유닛 생산자
        print("스페이스 바 눌림!");

        if (!orcList.Count.Equals(0))
        {
            foreach (var item in orcList)
            {
                if (!item.gameObject.activeSelf)
                {
                    print("여기 나오면 안됨");
                    spawnMonster = item;
                    spawnMonster.InitUnitInfoSetting(UnitDataManager.Instance._unitInfo_Dictionary["orc_warr01"]);
                    spawnMonster.transform.position = monsterUnitFactorys[0].spawnPoint;
                    item.gameObject.SetActive(true);
                    //
                    //spawnMonster.InitUnitInfoSetting();
                    //spawnOrc = item.gameObject;
                    cantSetActive = true;
                    break;
                }
            }
        }

        if (orcList.Count.Equals(0) || !cantSetActive)
        {
            monsterUnitFactorys[0].orcClass = AbsMonsterUnitFactory.OrcClass.Orc;
            spawnMonster = monsterUnitFactorys[0].CreateMonsterUnit();
            //spawnOrc = spawnMonster.gameObject;
            orcList.Add(spawnMonster.GetComponent<Orc>());
            print("프리팹 생성!");
        }

        print("네비메쉬 위에 있는지 확인" + spawnMonster._nav.isOnNavMesh);
        //spawnMonster.transform.position = spawnPoint;
        spawnMonster.gameObject.name = "오크";
        spawnMonster.transform.SetParent(orcPrefab_Objects);
        spawnMonster = null;    //spawnMonster 변수 값 초기화
    }


    IEnumerator C_DynamicBake() // 동적 베이크 실행하는 코루틴 함수
    {
        yield return new WaitForSeconds(5f);
        print("초기화");
        nms.BuildNavMesh();
        StartCoroutine(C_DynamicBake());


    }
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