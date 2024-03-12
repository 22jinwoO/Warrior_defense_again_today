using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcMinionFactory : AbsMonsterUnitFactory
{
    // 오크 주술사를 생산하도록 결정해주는 구상 생산자
    public override MonsterUnitClass CreateMonsterUnit()
    {
        MonsterUnitClass MonsterUnit = null;

        print("오크 주술사 생산");
        MonsterUnit = Instantiate(monsterPrefab, new Vector3(5.1f, 8.5f, 5.9f), Quaternion.identity);
        MonsterUnit.InitUnitInfoSetting(UnitDataManager.Instance._unitInfo_Dictionary["orc_sham01"]);


        return MonsterUnit;
    }
}
