using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage1_TextManager : MonoBehaviour
{
    //    - public TMP_Text tmp;
    //    - public TextMeshProUGUI tmp2;
    [SerializeField]
    private MonsterSpawnManager monsterSpawnManagerCs;

    [SerializeField]
    private TextMeshProUGUI currentWave_IntervalTxt;

    [SerializeField]
    private TextMeshProUGUI currentWaveTxt;

    [SerializeField]
    private TextMeshProUGUI currentWaveDeadMonsterTxt;

    [SerializeField]
    private TextMeshProUGUI castleHpTxt;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentWaveTxt.text = monsterSpawnManagerCs.currentWave.wave_Name;
        currentWaveDeadMonsterTxt.text = ($"처치한 몬스터 수 {monsterSpawnManagerCs.currentWave.deathMonsterCnt} / {monsterSpawnManagerCs.currentWave.wave_maxMonsterCount}");
        castleHpTxt.text = ($"성 체력 : {Castle.Instance._castle_Hp}");
        currentWave_IntervalTxt.text= "웨이브 대기 시간 : "+monsterSpawnManagerCs.currentWave.wave_StartTime.ToString();

    }
}
