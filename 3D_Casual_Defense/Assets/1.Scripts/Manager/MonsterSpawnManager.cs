using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{

    [SerializeField]
    Vector3 spawnPoint;

    [SerializeField]
    AbsMonsterUnitFactory[] monsterUnitFactorys;

    public NavMeshSurface nms;

    [Header("오크 프리팹 리스트 인덱스")]
    private int orcListIndex = 0;

    [Header("오크 프리팹 리스트")]
    public List<Orc> orcList = new List<Orc>();


    [SerializeField]
    private MonsterUnitClass spawnMonster;  //스폰된 몬스터

    [SerializeField]
    private Wave currentWave;   // 웨이브

    [SerializeField]
    private int currentMonsterIndex = 0;  // 웨이브 몬스터 종류 배열 인덱스

    [SerializeField]
    // 현재 웨이브에서 생성한 몬스터 숫자
    private int spawnMonsterCount = 0;


    [Header("몬스터 종류")]
    public MonsterUnitClass[] monsters;

    public Dictionary<string, MonsterUnitClass> d_MonsterDictonary=new Dictionary<string, MonsterUnitClass>();

    [SerializeField]
    private WaveSystem waveSys;


    public TextAsset data;  // Json 데이터 에셋  이 파일은 유니티 상에서 드래그해서 넣어줍니다.
    public AllData datas;  // 변환 데이터형   이게 문제 아니였나요



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
        StartCoroutine(C_DynamicBake());

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            waveSys.StartWave();
        }
    }


    public void StartWave(Wave wave)
    {
        currentWave.wave_monsterClasses.Clear();
        // 매개변수로 받아온 웨이브 정보 저장
        currentWave = wave;

        // 현재 웨이브 시작
        StartCoroutine(SpawnMonster());
    }

    private IEnumerator SpawnMonster()    // 몬스터 생성해주는 코루틴 함수
    {



        // 현재 생성될 몬스터의 숫자
        //int currentMonsterCount = 0;

        // 현재 몬스터 배열의 인덱스
        int monsterListIndex = 0;
        int repeatNum = 0;

        

        while (repeatNum<currentWave.wave_RepeatNum)
        {
            int currentMonsterCount = 0;
            // 현재 웨이브에서 생성되어야 하는 몬스터의 숫자만큼 몬스터 생성
            //while (spawnMonsterCount < currentWave.wave_maxMonsterCount)
            //{
            //    
            Debug.LogWarning(currentWave.wave_monsterClasses.Count);
            //currentMonsterCount = currentWave.wave_monsterKindCount[currentWave.monsterKindIndex];
            while (currentMonsterCount < currentWave.wave_monsterClasses.Count)
                {
                    //currentWave.monsterUnitClassFactory[monsterListIndex].orcClass = AbsMonsterUnitFactory.OrcClass.Orc;
                    //Debug.LogWarning(currentWave.wave_monsterKindCount[currentWave.monsterKindIndex]);
                    CheckSpawnMonster();
                    currentMonsterCount++;
                    spawnMonsterCount++;
                //currentMonsterCount++;

                Debug.LogWarning("currentMonsterCount " + currentMonsterCount);
                yield return new WaitForSeconds(currentWave.wave_interval);
                }
            //print("새로운 종류 몬스터 생성");
            //print("이전 몬스터 종류 수: "+currentWave.wave_monsterKindCount[currentWave.monsterKindIndex]);
            //currentWave.monsterKindIndex++;
            //print("현재 몬스터 종류 수: " + currentWave.wave_monsterKindCount[currentWave.monsterKindIndex]);

            Debug.LogWarning("repeatNum " + repeatNum);
            //}


            repeatNum++;
        }
        yield return null;
        orcListIndex = 0;
        spawnMonsterCount = 0;
    }

    //오크 생산자
    private void CreateOrc()
    {
        GameObject spawnOrc = null;
        //  유닛 생산자
        print("스페이스 바 눌림!");
        if (orcList.Count.Equals(0))
        {
            monsterUnitFactorys[0].orcClass = AbsMonsterUnitFactory.OrcClass.Orc;
            spawnMonster = monsterUnitFactorys[0].CreateMonsterUnit();
            spawnOrc = spawnMonster.gameObject;
            orcList.Add(spawnMonster.GetComponent<Orc>());
            print("프리팹 생성!");
        }

        else if (!orcList.Count.Equals(0) && !orcList[orcListIndex].gameObject.activeSelf && orcList.Count>spawnMonsterCount)   // orclistIndex 오브젝트가 활성화 상태라면
        {
            print(orcList.Count);
            print(spawnMonsterCount);
            orcList[spawnMonsterCount].gameObject.SetActive(true);
            spawnOrc = orcList[orcListIndex].gameObject;
            orcListIndex++; // 비활성화된 다음 오브젝트를 찾기 위한 인덱스 값 증가
            print("오브젝트 풀링 활성화");
        }

        else
        {
            monsterUnitFactorys[0].orcClass = AbsMonsterUnitFactory.OrcClass.Orc;
            spawnMonster = monsterUnitFactorys[0].CreateMonsterUnit();
            spawnOrc = spawnMonster.gameObject;
            orcList.Add(spawnMonster.GetComponent<Orc>());
            print("프리팹 생성!");
            orcListIndex++; // 비활성화된 다음 오브젝트를 찾기 위한 인덱스 값 증가
        }

        spawnOrc.transform.position = spawnPoint;
        spawnOrc.gameObject.name = "오크";
        spawnMonster = null;    //spawnMonster 변수 값 초기화



        //MonsterUnitClass orc = monsterUnitFactorys[0].CreateMonsterUnit();

        //gameObject.GetComponent<List<orc.GetComponent<Orc>().MonsterKind>>();
        //orcList.Add();


        //switch (spawnMonster.GetComponent<MonsterUnitClass>())
        //{
        //    case Orc:
        //        if (orcList.Count.Equals(0))
        //        {

        //        }


        //        break;

        //}

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
        //currentWave.mon
        //switch (currentWave.)
        //{
        //    default:
        //        break;
        //}
        //AbsMonsterUnitFactory monsterFactory = currentWave.monsterUnitClassFactory;
        //int monsterListIndex = currentWave.monsterKindIndex;


        //MonsterUnitClass monsterUnit = monsterFactory[monsterListIndex].CreateMonsterUnit();

        //switch (monsterUnit)
        //{
        //    default:
        //        break;
        //}
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
public class AllData    // 그럼 이걸 지워볼까요??
{
    public WaveData[] waveDatas;    // 그리고 waveDatas 이름은 시트 이름이랑 같아야해서 이렇게 했구요
}


[System.Serializable]
public class WaveData   // 이게 구글 시트에 해당하는 값을 갖오기 위한 클래스에요 
{
    public int waveNum;
    public int waveStartTime;
    public string waveName;
    public int interval;
    public string character1;
    //public int character1_rank;
    public string character2;
    //public int character2_rank;
    public string character3;
    //public int character3_rank;
    public string character4;
    //public int character4_rank;
    public string character5;
    //public int character5_rank;
    public int maxMonster_Count;
    public int repeatNum;
    

}