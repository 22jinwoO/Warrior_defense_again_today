using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherFactory : AbsPlayerUnitFactory
{
    // 궁수 프리팹
    public Archer ArcherPrefab;

    //
    // 궁수 클래스마다 생산될 유닛을 결정해주는 구상 생산자
    public override PlayerUnitClass CreatePlayerUnit()
    {
        PlayerUnitClass playerUnit = null;

        switch (archerClass)
        {
            case ArcherClass.Archer:
                print("궁수 생산");
                playerUnit = Instantiate(ArcherPrefab);

                // 궁수 유닛 ID에 해당하는 제이슨 파일 찾아서 필요한 데이터 할당
                playerUnit.InitUnitInfoSetting(UnitDataManager.Instance._unitInfo_Dictionary["hum_arch01"]);
                break;


            default:
                break;
        }
        return playerUnit;
    }
}
