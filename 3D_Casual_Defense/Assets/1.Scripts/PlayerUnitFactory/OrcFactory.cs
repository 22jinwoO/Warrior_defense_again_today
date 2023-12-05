using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbsPlayerUnitFactory;

public class OrcFactory : AbsMonsterUnitFactory
{
    // 오크 프리팹
    public Orc orcPrefab;



    // 기사 클래스마다 생산될 유닛을 결정해주는 구상 생산자
    public override MonsterUnitClass CreateMonsterUnit()
    {
        MonsterUnitClass MonsterUnit = null;

        switch (orcClass)
        {
            case OrcClass.Orc:
                print("오크 생산");
                MonsterUnit = Instantiate(orcPrefab);
                break;


            default:
                break;
        }
        return MonsterUnit;
    }
}
