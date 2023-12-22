using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherFactory : AbsPlayerUnitFactory
{
    // 기사 프리팹
    public Archor ArcherPrefab;

    // 기사 클래스마다 생산될 유닛을 결정해주는 구상 생산자
    public override PlayerUnitClass CreatePlayerUnit()
    {
        PlayerUnitClass playerUnit = null;

        switch (archerClass)
        {
            case ArcherClass.Archer:
                print("궁수 생산");
                playerUnit = Instantiate(ArcherPrefab);
                break;


            default:
                break;
        }
        return playerUnit;
    }
}
