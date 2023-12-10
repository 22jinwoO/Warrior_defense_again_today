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

    WaveSystem waveSys;
    private void Awake()
    {
        waveSys=GetComponent<WaveSystem>();
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

        // 현재 웨이브에서 생성되어야 하는 몬스터의 숫자만큼 몬스터 생성
        while (spawnMonsterCount < currentWave.maxMonsterCount)
        {
            int currentMonsterCount = 0;

            //currentMonsterCount = currentWave.monsterKindCount[currentWave.monsterKindIndex];
            while (currentMonsterCount <= currentWave.monsterKindCount[currentWave.monsterKindIndex]-1)
            {
                //currentWave.monsterUnitClassFactory[monsterListIndex].orcClass = AbsMonsterUnitFactory.OrcClass.Orc;
                Debug.LogWarning(currentWave.monsterKindCount[currentWave.monsterKindIndex]);
                CheckSpawnMonster();
                currentMonsterCount++;
                spawnMonsterCount++;
                //currentMonsterCount++;

                yield return new WaitForSeconds(currentWave.spawnDelayTime);
            }
            //print("새로운 종류 몬스터 생성");
            //print("이전 몬스터 종류 수: "+currentWave.monsterKindCount[currentWave.monsterKindIndex]);
            currentWave.monsterKindIndex++;
            //print("현재 몬스터 종류 수: " + currentWave.monsterKindCount[currentWave.monsterKindIndex]);


        }


        yield return null;
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

        else if (!orcList.Count.Equals(0) && orcList.Count>spawnMonsterCount)
        {
            print(orcList.Count);
            print(spawnMonsterCount);
            orcList[spawnMonsterCount].gameObject.SetActive(true);
            spawnOrc = orcList[orcListIndex].gameObject;
            orcListIndex++;
            print("오브젝트 풀링 활성화");
        }

        else
        {
            monsterUnitFactorys[0].orcClass = AbsMonsterUnitFactory.OrcClass.Orc;
            spawnMonster = monsterUnitFactorys[0].CreateMonsterUnit();
            spawnOrc = spawnMonster.gameObject;
            orcList.Add(spawnMonster.GetComponent<Orc>());
            print("프리팹 생성!");
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
        MonsterUnitClass[] monsterSpawnFactorys = currentWave.monsterClasses;
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
