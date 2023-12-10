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

    public Dictionary<string, MonsterUnitClass> d_FindMonsterList=new Dictionary<string, MonsterUnitClass>();

    private WaveSystem waveSys;


    public TextAsset data;  // Json 데이터 에셋
    public AllData datas;  // 변환 데이터형



    private void Awake()
    {
        print(monsters[0]);
        print(d_FindMonsterList);
        d_FindMonsterList.Add("ORC_war01", monsters[0]);

        waveSys=GetComponent<WaveSystem>();

        datas = JsonUtility.FromJson<AllData>(data.text);

    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(C_DynamicBake());    // 동적 베이크 함수 실행
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            waveSys.StartWave();
        }
    }


    public void StartWave(Wave wave)    //웨이브 시작하는 함수
    {
        currentWave.wave_monsterClasses.Clear();    // 현재 몬스터 종류 리스트 값 초기화

        // 매개변수로 받아온 웨이브 정보 저장
        currentWave = wave;

        // 현재 웨이브 시작
        StartCoroutine(SpawnMonster());
    }

    private IEnumerator SpawnMonster()    // 몬스터 생성해주는 코루틴 함수
    {

        int repeatNum = 0;  //반복 횟수 변수

        spawnMonsterCount = 0;  // 현재 웨이브에서 생성된 몬스터 수


        while (repeatNum < currentWave.wave_RepeatNum)  // 현재 웨이브의 반복 횟수만큼 while문 반복
        {
            int currentMonsterCount = 0;    // 현재 웨이브에서 생성된 몬스터의 수

            currentWave.monsterKindIndex = 0;   //현재 웨이브의 몬스터 종류 인덱스 값

            Debug.LogWarning(currentWave.wave_monsterClasses.Count);

            // 현재 웨이브에서 생성되어야 하는 몬스터의 숫자만큼 몬스터 생성
            while (currentMonsterCount < currentWave.wave_monsterClasses.Count) // 현재 생성된 몬스터의 수가 현재 웨이브의 몬스터 종류 수보다 작을 때 동안 반복
            {
                CheckSpawnMonster();    // 어떤 종류의 몬스터인지 체크 후 몬스터 생성
                currentMonsterCount++;  // 현재 몬스터 수 증가
                spawnMonsterCount++;    // 현재 웨이브에서 생성된 몬스터의 수 증가

                Debug.LogWarning("currentMonsterCount " + currentMonsterCount);
                yield return new WaitForSeconds(currentWave.wave_interval); // 현재 웨이브의 몬스터 생성주기만큼 기다림
            }
            Debug.LogWarning("repeatNum " + repeatNum);
            repeatNum++;
        }
        yield return null;
        currentWave.monsterKindIndex = 0;   //현재 웨이브의 몬스터 종류 인덱스 값 초기화

        orcListIndex = 0;   // 오크 프리팹 리스트 값 초기화
    }

    IEnumerator C_DynamicBake() // 동적 베이크 실행하는 코루틴 함수
    {
        yield return new WaitForSeconds(5f);
        print("초기화");
        nms.BuildNavMesh();
        StartCoroutine(C_DynamicBake());    //재귀함수 실행
    }



    private void CheckSpawnMonster()    // 어떤 종류의 몬스터인지 체크하는 함수
    {
        List<MonsterUnitClass> monsterSpawnFactorys = currentWave.wave_monsterClasses;
        int monsterKindsIndex = currentWave.monsterKindIndex;

        switch (monsterSpawnFactorys[monsterKindsIndex]) // 몬스터 종류 배열의 현재 몬스터 배열 인덱스
        {
            case Orc:
                CreateOrc();
                break;

        }
        currentWave.monsterKindIndex++;   // 오크 프리팹 리스트 인덱스 값 증가
    }

    //오크 생산자
    private void CreateOrc()
    {
        GameObject spawnOrc = null;

        //  유닛 생산자
        print("스페이스 바 눌림!");
        if (orcList.Count.Equals(0))    // 오크 프리팹 리스트에 값이 하나도 없다면
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

    }
}

[System.Serializable]
public class AllData
{
    public WaveData[] waveDatas;
}


[System.Serializable]
public class WaveData
{
    public int waveNum; // 웨이브 넘버
    public int waveStartTime;   //  웨이브 시작대기 시간
    public string waveName; // 웨이브 이름
    public int interval;    // 웨이브의 몬스터 생성주기
    public string character1;   // 첫번째 종류 몬스터
    public string character2;   // 두번째 종류 몬스터
    public string character3;   // 세번째 종류 몬스터
    public string character4;   // 네번째 종류 몬스터
    public string character5;   // 다섯번째 종류 몬스터
    public int maxMonster_Count;    // 몬스터 최대 생성 수
    public int repeatNum;   // 몬스터 생성 반복 주기
}