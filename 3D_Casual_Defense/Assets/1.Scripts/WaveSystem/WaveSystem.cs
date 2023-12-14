using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


// 여기를 없앴어요 아니에요 Ui 문제인건 포실님이 말슴해주셔서,, 그럼 이부분은 포기해야하나요?;; wave를 먼저 앞에서 선언해준게 문제가 될까요?
// 메인보다 





public class WaveSystem : MonoBehaviour

{
    

    [SerializeField]
    private Wave[] waves;   // 현재 스테이지의 모든 웨이브 정보
    
    [SerializeField]
    private MonsterSpawnManager spawnManager;
    private int currentWaveIndex = -1;  // 현재 웨이브 인덱스


    private void Awake()
    {

        //waves = new Wave[10];
        //waves = new Wave[spawnManager.datas.waveDatas.Length];
        //for (int i = 0; i < spawnManager.datas.waveDatas.Length; i++)
        //{
        //    print(waves[3].wave_Name);
        //    print("i넘버" + i);
        //    waves[i].wave_Name = spawnManager.datas.waveDatas[i].waveName;
        //    waves[i].wave_interval = spawnManager.datas.waveDatas[i].interval;
        //    waves[i].wave_maxMonsterCount = spawnManager.datas.waveDatas[i].maxMonster_Count;

        //    waves[i].wave_RepeatNum = spawnManager.datas.waveDatas[i].repeatNum;

        //    print(spawnManager.datas.waveDatas[5].character3);



        //    if (!spawnManager.datas.waveDatas[i].character1.Equals("") && !spawnManager.datas.waveDatas[i].character1.Equals(null))
        //        waves[i].wave_monsterClasses.Add(spawnManager.d_MonsterDictonary[spawnManager.datas.waveDatas[i].character1]);
        //    if (!spawnManager.datas.waveDatas[i].character2.Equals(""))
        //        waves[i].wave_monsterClasses.Add(spawnManager.d_MonsterDictonary[spawnManager.datas.waveDatas[i].character2]);
        //    if (!spawnManager.datas.waveDatas[i].character3.Equals(""))
        //        waves[i].wave_monsterClasses.Add(spawnManager.d_MonsterDictonary[spawnManager.datas.waveDatas[i].character3]);


        //    if (!spawnManager.datas.waveDatas[i].character4.Equals(""))
        //        waves[i].wave_monsterClasses.Add(spawnManager.d_MonsterDictonary[spawnManager.datas.waveDatas[i].character4]);
        //    if (!spawnManager.datas.waveDatas[i].character5.Equals(""))
        //        waves[i].wave_monsterClasses.Add(spawnManager.d_MonsterDictonary[spawnManager.datas.waveDatas[i].character5]);

        //}
    }

    private void Start()
    {
        // 이거요?? ㅅ순서 말씀하시는 거 아닌가요 위에서부터 아래로 한다는 가정하에
        waves = new Wave[spawnManager.datas.waveDatas.Length]; //여기 식으로 해줬었는데, spawnManager.datas.waveDatas.Length 이 부분의 값이 잘 들어오는데
        print(spawnManager.datas.waveDatas.Length);
        // 여기서 넣어줘요 네 길이를 값 넣어주고 나서 여기 부분이 지금은 초기화 해주는 분이 없어요,, 네 그거때메 그런건가
        // 맞아요,,, 원래 위에꺼 대로 한번 실행하는거 한번 보여드릴까요
        for (int i = 0; i < spawnManager.datas.waveDatas.Length; i++)   // 이게 datas 배열 길이만큼 반복하여 값을 넣어주는 구문입니다
        {
            print(waves[3].wave_Name);
            print("i넘버" + i);           // 네네네
            waves[i].wave_Name = spawnManager.datas.waveDatas[i].waveName;
            waves[i].wave_interval = spawnManager.datas.waveDatas[i].interval;
            waves[i].wave_maxMonsterCount = spawnManager.datas.waveDatas[i].maxMonster_Count;

            waves[i].wave_RepeatNum = spawnManager.datas.waveDatas[i].repeatNum;

            print(spawnManager.datas.waveDatas[5].character3);
            // 이 오류가 인스펙터 창 관련된 거라는 그 유니티 글도 봤엉써요
            // 이게 웃긴게 될 때도 있고 안될 떄도 있어서 뭐가 문젠지 좀 찾기 힘드네요 지금 독서실 와서 한번도 안떴어요;;;
            // 그래서 오류가 났을 땐 네네 같은 노트북이에요 그 오류가 났을 땐 제이슨 파일 불러오는 부분이 오류가 나서 Datas 부분에 값이 하나도 안들어가더라구요
            // 넵 오오오오,, 네 아 근데 제 스프레드 시트에 있는걸 엑스포트해서 값을 미리 저장해서 가져오는거라 수정부분은 상관이 없을 것 같아요
            // 지금 오류가 너무 안나서 한번 껏다가 킬께요

            waves[i].wave_monsterClasses = new List<MonsterUnitClass>();    // 이거 맞나요??

            if (!spawnManager.datas.waveDatas[i].character1.Equals("") && !spawnManager.datas.waveDatas[i].character1.Equals(null))
            {
                waves[i].wave_monsterClasses.Add(spawnManager.d_MonsterDictonary[spawnManager.datas.waveDatas[i].character1]);
            }

            if (!spawnManager.datas.waveDatas[i].character2.Equals("") && !spawnManager.datas.waveDatas[i].character2.Equals(null))
                waves[i].wave_monsterClasses.Add(spawnManager.d_MonsterDictonary[spawnManager.datas.waveDatas[i].character2]);

            if (!spawnManager.datas.waveDatas[i].character3.Equals("") && !spawnManager.datas.waveDatas[i].character3.Equals(null))
                waves[i].wave_monsterClasses.Add(spawnManager.d_MonsterDictonary[spawnManager.datas.waveDatas[i].character3]);


            if (!spawnManager.datas.waveDatas[i].character4.Equals("") && !spawnManager.datas.waveDatas[i].character4.Equals(null))
                waves[i].wave_monsterClasses.Add(spawnManager.d_MonsterDictonary[spawnManager.datas.waveDatas[i].character4]);

            if (!spawnManager.datas.waveDatas[i].character5.Equals("") && !spawnManager.datas.waveDatas[i].character5.Equals(null))
                waves[i].wave_monsterClasses.Add(spawnManager.d_MonsterDictonary[spawnManager.datas.waveDatas[i].character5]);

        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWave()
    {
        // 현재 맵에 적이 없고, Wave가 남아있다면
        if (currentWaveIndex<waves.Length-1)
        {
            // 인덱스의 시작이 -1이기 때문에 wave 인덱스 증가를 제일 먼저함
            currentWaveIndex++;
            // SpawnManager의 StartWave() 함수 호출, 현재 웨이브 정보 제공
            spawnManager.StartWave(waves[currentWaveIndex]);
            print("커렌트웨이브 인덱스 "+currentWaveIndex);
        }
    }
}

[System.Serializable]
public struct Wave
{
    [Header("웨이브 이름")]
    public string wave_Name;    // 몬스터 생성 주기

    [Header("몬스터 생성 주기")]
    public int wave_interval;    // 몬스터 생성 주기

    [Header("몬스터 종류")]
    public List<MonsterUnitClass> wave_monsterClasses; // 몬스터 종류

    //[Header("유닛 종류별 생성 수")]
    //public int[] wave_monsterKindCount;  // 유닛 종류별 생성 수

    [Header("유닛 스폰되는 몬스터 종류 리스트 인덱스")]
    public int monsterKindIndex;  // 유닛 스폰되는 몬스터 종류 리스트 인덱스

    [Header("최대 몬스터 생성 수")]
    public int wave_maxMonsterCount;   // 최대 몬스터 생성 수

    [Header("생성 반복 횟수")]
    public int wave_RepeatNum;   // 최대 몬스터 생성 수

}

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();


    [SerializeField]
    private List<TValue> values = new List<TValue>();


    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }


    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();


        if (keys.Count != values.Count)
            throw new Exception("there are " + keys.Count + " keys and " + values.Count + " values after deserialization. Make sure that both key and value types are serializable.");


        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }

    static public T ToEnum<T>(string str)
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        foreach (T t in A)
        {
            if (t.ToString() == str)
                return t;
        }
        return default(T);
    }
}


