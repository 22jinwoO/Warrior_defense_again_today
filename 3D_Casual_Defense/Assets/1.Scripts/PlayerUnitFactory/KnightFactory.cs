using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightFactory : AbsPlayerUnitFactory
{
    // 기사 프리팹
    public Knight knightPrefab;

    // 기사 클래스마다 생산될 유닛을 결정해주는 구상 생산자
    public override PlayerUnitClass CreatePlayerUnit()
    {
        PlayerUnitClass playerUnit = null;

        switch (knightClass)
        {
            case KnightClass.Knight:
                print("나이트 생산");
                playerUnit = Instantiate(knightPrefab);
                break;


            default:
                break;
        }
        return playerUnit;
    }
}
