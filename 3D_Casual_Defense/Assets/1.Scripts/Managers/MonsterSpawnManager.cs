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
    public SerializableDictionary<string, MonsterUnitClass> d_MonsterDictonary=new SerializableDictionary<string, MonsterUnitClass>();

    [SerializeField]
    private WaveSystem waveSys;


    public TextAsset data;  // Json 데이터 에셋  이 파일은 유니티 상에서 드래그해서 넣어줍니다.

    public AllData datas;  // 변환 데이터형

    //public Text intervalTxt;

    private void Awake()
    {
        //print(monsters[0]);
        //print(d_MonsterDictonary);
        d_MonsterDictonary.Add("ORC_war01", monsters[0]);

        waveSys=GetComponent<WaveSystem>();

        datas = JsonUtility.FromJson<AllData>(data.text);   // Json파일의 텍스트들을 datas 값에 넣어주고

        //foreach (var item in datas.waveDatas)
        //{
        //    print(datas.waveDatas[3].interval);
        //}
        //nms = GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<NavMeshSurface>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(C_DynamicBake());

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            waveSys.StartWave();
        }

        if (currentWave.wave_maxMonsterCount==currentWave.deathMonsterCnt)
        {
            currentWave.deathMonsterCnt = 0;
            print("웨이브 변경");
            waveSys.StartWave();
        }
    }


    public void StartWave(Wave wave)
    {
        currentWave.wave_monsterClasses.Clear();
        // 매개변수로 받아온 웨이브 정보 저장
        currentWave = wave;
        //intervalTxt.text = currentWave.wave_interval.ToString();
        // 현재 웨이브 시작
        StartCoroutine(SpawnMonster());
    }

    private IEnumerator SpawnMonster()    // 몬스터 생성해주는 코루틴 함수
    {
        int repeatNum = 0;
        Debug.LogWarning("대기시간 :"+currentWave.wave_StartTime);
        while (currentWave.wave_StartTime > 0)
        {
            //intervalTxt.text=currentWave.wave_StartTime.ToString();
            currentWave.wave_StartTime--;
            yield return new WaitForSeconds(1f);
        }
        //yield return new WaitForSeconds(currentWave.wave_interval);

        while (repeatNum<currentWave.wave_RepeatNum)
        {
            int currentMonsterCount = 0;

            Debug.LogWarning(currentWave.wave_monsterClasses.Count);

            // 현재 웨이브에서 생성되어야 하는 몬스터의 숫자만큼 몬스터 생성
            while (currentMonsterCount < currentWave.wave_monsterClasses.Count)
                {

                    CheckSpawnMonster();
                    currentMonsterCount++;
                    spawnMonsterCount++;

                    Debug.LogWarning("currentMonsterCount " + currentMonsterCount);
                    yield return new WaitForSeconds(currentWave.wave_interval);
                }

            Debug.LogWarning("repeatNum " + repeatNum);
            repeatNum++;
        }
        yield return null;
        orcListIndex = 0;
        spawnMonsterCount = 0;
    }

    private void CheckSpawnMonster()
    {
        List<MonsterUnitClass> monsterSpawnFactorys = currentWave.wave_monsterClasses;
        int monsterKindsIndex = currentWave.monsterKindIndex;

        switch (monsterSpawnFactorys[monsterKindsIndex]) // 몬스터 종류 배열의 현재 몬스터 배열 인덱스
        {
            case Orc:
                CreateOrc();
                break;

        }
    }

    //오크 생산자
    private void CreateOrc()
    {
        //GameObject spawnOrc = null;
        bool cantSetActive=false;
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