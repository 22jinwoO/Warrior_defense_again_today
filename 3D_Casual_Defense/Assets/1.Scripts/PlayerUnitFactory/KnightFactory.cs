using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightFactory : AbsPlayerUnitFactory
{
    // 기사 프리팹
    public Knight knightPrefab;

    private void Awake()
    {
        // 기사 유닛 ID에 해당하는 제이슨 파일 찾아서 필요한 데이터 할당
        // knightPrefab.InitUnitInfoSetting(UnitDataManager.Instance._unitInfo_Dictionary["hum_warr01"]);

    }


    // 기사 클래스마다 생산될 유닛을 결정해주는 구상 생산자
    public override PlayerUnitClass CreatePlayerUnit()
    {
        PlayerUnitClass playerUnit = null;

        switch (knightClass)
        {
            case KnightClass.Knight:
                print("나이트 생산");
                playerUnit = Instantiate(knightPrefab);

                // 기사 유닛 ID에 해당하는 제이슨 파일 찾아서 필요한 데이터 할당
                playerUnit.InitUnitInfoSetting(UnitDataManager.Instance._unitInfo_Dictionary["hum_warr01"]);

                break;


            default:
                break;
        }
        return playerUnit;
    }
}
