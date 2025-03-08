using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class WaveSystem : MonoBehaviour

{
    

    [SerializeField]
    private Wave[] waves;   // 현재 스테이지의 모든 웨이브 정보를 갖고 있는 wave 구조체 배열

    [SerializeField]
    private MonsterSpawnManager spawnManager;

    public int currentWaveIndex = -1;  // 현재 웨이브 인덱스

    [SerializeField]
    private UI_PopUpManager uiManager;


    private void Awake()
    {
        // WaveDatas 구글 시트를 Json 파일로 변환한 값을 불러와 WaveSystem.cs의 Waves[]배열에 Json파일의 i번째 Wave데이터들을 넣어주는 함수
        LoadWaveData();
    }

    //private void Update()
    //{

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        currentWaveIndex = 10;
    //        StartWave();
    //    }
    //}
    #region # LoadWaveData() : WaveDatas 구글 시트를 Json 파일로 변환한 값을 불러와 WaveSystem.cs의 Waves[]배열에 Json파일의 i번째 Wave데이터들을 넣어줌
    private void LoadWaveData()
    {
        // waves : 현재 스테이지의 모든 웨이브 정보를 갖고 있는 wave 구조체 배열 길이 = Json 텍스트 파일의 배열 크기
        waves = new Wave[spawnManager.datas.waveDatas.Length];

        // WaveDatas 구글시트 Json 파일을 텍스트로 변환한 datas 배열 길이만큼 반복하여 값을 넣어줌
        for (int i = 0; i < spawnManager.datas.waveDatas.Length; i++)
        {
            // waves[i]의 웨이브 이름 = 웨이브 데이터 Json파일 배열 i번째 값의 웨이브 이름
            waves[i].wave_Name = spawnManager.datas.waveDatas[i].waveName;

            // waves[i]의 몬스터 생성주기 = 웨이브 데이터 Json파일 배열 i번째 값의 몬스터 생성주기
            waves[i].wave_interval = spawnManager.datas.waveDatas[i].interval;

            // waves[i]의 웨이브 대기시간 = 웨이브 데이터 Json파일 배열 i번째 값의 웨이브 대기시간
            waves[i].wave_StartTime = spawnManager.datas.waveDatas[i].waveStartTime;

            // waves[i]의 몬스터 최대 생성 수 = 웨이브 데이터 Json파일 배열 i번째 값의 최대 생성 수
            waves[i].wave_maxMonsterCount = spawnManager.datas.waveDatas[i].maxMonster_Count;

            // waves[i]의 생성 반복 횟수 = 웨이브 데이터 Json파일 배열 i번째 값의 생성 반복 횟수
            waves[i].wave_RepeatNum = spawnManager.datas.waveDatas[i].repeatNum;

            // 몬스터 종류 리스트 초기화
            waves[i].wave_monsterClasses = new List<string>();

            // 웨이브 데이터 Json파일 배열 i번째 값의 첫번째 종류 몬스터 값이 공백 && Null 이 아닐 때
            if (!spawnManager.datas.waveDatas[i].character1.Equals("") && !spawnManager.datas.waveDatas[i].character1.Equals(null))

                // 스폰 매니저의 딕셔너리 Key값에 웨이브 데이터 Json파일 배열 i번째 값의 첫번째 종류 몬스터 이름에 해당하는 몬스터를 찾아서 
                // waves[i]의 몬스터 종류 리스트에 해당 몬스터 추가
                waves[i].wave_monsterClasses.Add(spawnManager.datas.waveDatas[i].character1);

            // 웨이브 데이터 Json파일 배열 i번째 값의 두번째 종류 몬스터 값이 공백 && Null 이 아닐 때
            if (!spawnManager.datas.waveDatas[i].character2.Equals("") && !spawnManager.datas.waveDatas[i].character2.Equals(null))

                // 스폰 매니저의 딕셔너리 Key값에 웨이브 데이터 Json파일 배열 i번째 값의 두번째 종류 몬스터 이름에 해당하는 몬스터를 찾아서 
                // waves[i]의 몬스터 종류 리스트에 해당 몬스터 추가
                waves[i].wave_monsterClasses.Add(spawnManager.datas.waveDatas[i].character2);

            // 웨이브 데이터 Json파일 배열 i번째 값의 두번째 종류 몬스터 값이 공백 && Null 일 떄 아래 값들도 없으므로
            // for문의 다음 i번째 값을 실행하도록 함
            else
                continue;

            // 웨이브 데이터 Json파일 배열 i번째 값의 세번째 종류 몬스터 값이 공백 && Null 이 아닐 때
            if (!spawnManager.datas.waveDatas[i].character3.Equals("") && !spawnManager.datas.waveDatas[i].character3.Equals(null))

                // 스폰 매니저의 딕셔너리 Key값에 웨이브 데이터 Json파일 배열 i번째 값의 세번째 종류 몬스터 이름에 해당하는 몬스터를 찾아서 
                // waves[i]의 몬스터 종류 리스트에 해당 몬스터 추가
                waves[i].wave_monsterClasses.Add(spawnManager.datas.waveDatas[i].character3);

            // 웨이브 데이터 Json파일 배열 i번째 값의 세번째 종류 몬스터 값이 공백 && Null 일 떄 아래 값들도 없으므로
            // for문의 다음 i번째 값을 실행하도록 함
            else
                continue;

            // 웨이브 데이터 Json파일 배열 i번째 값의 네번째 종류 몬스터 값이 공백 && Null 이 아닐 때
            if (!spawnManager.datas.waveDatas[i].character4.Equals("") && !spawnManager.datas.waveDatas[i].character4.Equals(null))

                // 스폰 매니저의 딕셔너리 Key값에 웨이브 데이터 Json파일 배열 i번째 값의 네번째 종류 몬스터 이름에 해당하는 몬스터를 찾아서 
                // waves[i]의 몬스터 종류 리스트에 해당 몬스터 추가
                waves[i].wave_monsterClasses.Add(spawnManager.datas.waveDatas[i].character4);

            // 웨이브 데이터 Json파일 배열 i번째 값의 네번째 종류 몬스터 값이 공백 && Null 일 떄 아래 값들도 없으므로
            // for문의 다음 i번째 값을 실행하도록 함
            else
                continue;

            // 웨이브 데이터 Json파일 배열 i번째 값의 다섯번째 종류 몬스터 값이 공백 && Null 이 아닐 때
            if (!spawnManager.datas.waveDatas[i].character5.Equals("") && !spawnManager.datas.waveDatas[i].character5.Equals(null))

                // 스폰 매니저의 딕셔너리 Key값에 웨이브 데이터 Json파일 배열 i번째 값의 다섯번째 종류 몬스터 이름에 해당하는 몬스터를 찾아서 
                // waves[i]의 몬스터 종류 리스트에 해당 몬스터 추가
                waves[i].wave_monsterClasses.Add(spawnManager.datas.waveDatas[i].character5);

            // 웨이브 데이터 Json파일 배열 i번째 값의 다섯번째 종류 몬스터 값이 공백 && Null 일 떄 아래 값들도 없으므로
            // for문의 다음 i번째 값을 실행하도록 함
            else
                continue;
        }
    }

    #endregion
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

        // Waver가 없다면 클리어 UI 호출
        else
        {
            // 플레이어 승리 시 팝업창 활
            // 성화
            uiManager.PlayerWinPopUp();
        }
    }
}

[System.Serializable]
public struct Wave  // Wave 구조체
{
    [Header("웨이브 이름")]
    public string wave_Name;    

    [Header("몬스터 생성 주기")]
    public int wave_interval;    // 몬스터 생성 주기

    [Header("몬스터 생성 주기")]
    public int wave_StartTime;    // 몬스터 생성 주기

    [Header("몬스터 종류")]
    public List<string> wave_monsterClasses; // 몬스터 종류

    [Header("유닛 스폰되는 몬스터 종류 리스트 인덱스")]
    public int monsterKindIndex;  // 유닛 스폰되는 몬스터 종류 리스트 인덱스

    [Header("최대 몬스터 생성 수")]
    public int wave_maxMonsterCount;   // 최대 몬스터 생성 수

    [Header("생성 반복 횟수")]
    public int wave_RepeatNum;   // 최대 몬스터 생성 수

    [Header("몬스터 죽은 숫자")]
    public int deathMonsterCnt; // 몬스터 죽은 숫자


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

    internal Abs_Skill TryGetValue()
    {
        throw new NotImplementedException();
    }
}


