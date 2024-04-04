using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KnightFactory : AbsPlayerUnitFactory
{
    private void Awake()
    {
        unitId = "hum_warr01";

        // 오브젝트 풀링 셋팅하는 함수
        InitObjPool(playerUnitPrefab);
    }

    // 기사 클래스마다 생산될 유닛을 결정해주는 구상 생산자
    public override PlayerUnitClass CreatePlayerUnit()
    {
        PlayerUnitClass playerUnit = null;

        if(units.Count > 0) 
        {
            playerUnit=units.Pop();
            print("스택 팝 실행");
        }

        else
        {
            print("나이트 생산");
            playerUnit = Instantiate(playerUnitPrefab);
            playerUnit.unitFactory = this;

        }

        return playerUnit;
    }
}
