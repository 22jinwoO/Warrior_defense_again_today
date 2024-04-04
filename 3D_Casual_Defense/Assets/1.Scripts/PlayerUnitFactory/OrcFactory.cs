using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OrcFactory : AbsMonsterUnitFactory
{
    private void Awake()
    {
        // 오브젝트 풀링 셋팅하는 함수
        InitObjPool(monsterPrefab);
    }

    // 오크 클래스마다 생산될 유닛을 결정해주는 구상 생산자
    public override MonsterUnitClass CreateMonsterUnit()
    {
        MonsterUnitClass MonsterUnit = null;

        if (monsters.Count > 0)
        {
            MonsterUnit = monsters.Pop();
            print("스택 팝 실행");
        }

        else
        {
            print("오크 생산");
            MonsterUnit = Instantiate(monsterPrefab);
            MonsterUnit.monsterFactory = this;

        }

        return MonsterUnit;
    }
}
