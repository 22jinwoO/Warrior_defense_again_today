using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage1_TextManager : Singleton<Stage1_TextManager>
{
    public MonsterSpawnManager monsterSpawnManagerCs;

    // 몬스터 생성 주기
    public TextMeshProUGUI currentWave_IntervalTxt;

    // 현재 웨이브 텍스트
    public TextMeshProUGUI currentWaveTxt;

    // 현재 웨이브 처치된 몬스터 수 텍스트
    public TextMeshProUGUI currentWaveDeadMonsterTxt;

    // 성 내구도 텍스트
    public TextMeshProUGUI castleHpTxt;

    // 팝업 메세지 텍스트
    public TextMeshProUGUI popUpMessageTxt;

    // Update is called once per frame
    //void Update()
    //{
    //    //currentWaveTxt.text = monsterSpawnManagerCs.currentWave.wave_Name;
    //    currentWave_IntervalTxt.text= "웨이브 대기 시간 : "+monsterSpawnManagerCs.currentWave.wave_StartTime.ToString();

    //}

    public void PlayerWinTxt()
    {
        popUpMessageTxt.text = "플레이어가 승리하셨습니다!!";
    }

    public void PlayerLoseTxt()
    {
        popUpMessageTxt.text = "플레이어가 패배하였습니다...";
    }


    public void CountDeadMonster()
    {
        currentWaveDeadMonsterTxt.text = ($"처치한 몬스터 수 {monsterSpawnManagerCs.currentWave.deathMonsterCnt} / {monsterSpawnManagerCs.currentWave.wave_maxMonsterCount}");
    }

    public void ShowCastleHpTxt()
    {
        castleHpTxt.text = ($"성 체력 : {Castle.Instance._castle_Hp}");
    }
}
