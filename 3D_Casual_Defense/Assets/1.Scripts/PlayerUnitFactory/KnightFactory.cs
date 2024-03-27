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

                // 기사 유닛 ID에 해당하는 제이슨 파일 찾아서 필요한 데이터 할당
                playerUnit.InitUnitInfoSetting(UnitDataManager.Instance._unitInfo_Dictionary["hum_warr01"]);
                break;
        }
        return playerUnit;
    }


    // 유닛 데이터 매니저의 유닛데이터 딕셔너리 겟키즈로 키값들 배열로 가져와서 할당
    // 버튼 인덱스마다 해당하는 인덱스 값 부여
    // 어떤 유닛을 생산할지 유닛 버튼을 클릭했을 때 유닛 데이터 키값 전달

    // 드래그 끝났을 때 호출되도록?

    //public PlayerUnitClass CreateUnit()
    //{
    //    print("기사생산");

    // 소환진 활성화
    // 이 때 어떤 유닛 생산될지 정해주는 함수 호출 (기능) :어떤 유닛 생산할지 UnitDataManager.Instance._unitInfo_Dictionary[knight._unitData.unit_Id] 로 데이터 할당 후, 게임오브젝트로 복사 후 그 유닛 비활성화하고 유닛 생산하는 함수에 전달

    //    // 대기시간 다 지나면

    //    // 반환된 유닛 활성화 
    // 매개변수로 받은 초기값 할당된 플레이어 유닛 활성화

    //    // 기사 유닛 생산자
    //    playerUnitFactorys[0].knightClass = AbsPlayerUnitFactory.KnightClass.Knight;

    //    // 생산자 실행
    //    PlayerUnitClass knight = playerUnitFactory.CreateUnit();

    //    //print(UnitDataManager.Instance._unitInfo_Dictionary[knight._unitData.unit_Id]);
    //    //Vector3 setPos = new Vector3(Input.mousePosition.x, 0f, Input.mousePosition.z);
    //    //knight.transform.position = setPos;

    //    knight.gameObject.name = "기사" + knight._unitData.char_id;

    //    popUpMgr.isUseShop = false;

    //    // 유닛 생산 팝업창 닫기
    //    StartCoroutine(popUpMgr.UseUnitPopUp());
    //    return knight;
    //}


    // 어떤 유닛을 생산할지 결정해주는 구상 생산자
    //public override PlayerUnitClass CreateUnit(string unitId)
    //{

    //            playerUnit = Instantiate(knightPrefab);

    //            기사 유닛 ID에 해당하는 제이슨 파일 찾아서 필요한 데이터 할당
    //            playerUnit.InitUnitInfoSetting(UnitDataManager.Instance._unitInfo_Dictionary[unitId]);
    //            break;

    //    return playerUnit;
    //}
}
