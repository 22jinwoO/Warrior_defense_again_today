using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



[System.Serializable]
public struct Wave
{
    [Header("몬스터 생성 주기")]
    public float spawnDelayTime;    // 몬스터 생성 주기
    [Header("최대 몬스터 생성 수")]
    public int maxMonsterCount;   // 최대 몬스터 생성 수

    [Header("몬스터 종류")]
    public MonsterUnitClass[] monsterClasses; // 몬스터 종류

    [Header("유닛 종류별 생성 수")]
    public int[] monsterKindCount;  // 유닛 종류별 생성 수
    //[Header("몬스터 딕셔너리 값")]
    //public SerializableDictionary<MonsterUnitClass, int> spawnMonsterKinds;


    [Header("유닛 스폰되는 몬스터 종류 리스트 인덱스")]
    public int monsterKindIndex;  // 유닛 스폰되는 몬스터 종류 리스트 인덱스
    //[Header("몬스터 생산자 종류")]
    //public AbsMonsterUnitFactory[] monsterUnitClassFactorys; // 몬스터 생산자 종류
}


public class WaveSystem : MonoBehaviour

{
    [SerializeField]
    private Wave[] waves;   // 현재 스테이지의 모든 웨이브 정보

    [SerializeField]
    private MonsterSpawnManager spawnManager;
    private int currentWaveIndex = -1;  // 현재 웨이브 인덱스



    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }
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


