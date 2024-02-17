using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreatePlayerUnit : MonoBehaviour
{
    [SerializeField]
    private AbsPlayerUnitFactory[] playerUnitFactorys;

    [SerializeField]
    private Button clickKnightBtn;

    [SerializeField]
    private Button clickArcherBtn;

    [SerializeField]
    private Button clickUnitFreeBtn;

    [SerializeField]
    private Button clickUnitHoldBtn;


    public UnitInfo clikUnitInfo;

    [SerializeField]
    private UI_PopUpManager popUpMgr;

    void Start()
    {
        //clickKnightBtn.onClick.AddListener(CreateKnight);
        //clickArcherBtn.onClick.AddListener(CreateArcher);
        clickUnitFreeBtn.onClick.AddListener(ClickUnitFree);
        clickUnitHoldBtn.onClick.AddListener(ClickUnitHold);
    }

    // 드래그 끝났을 때 호출되도록?
    public PlayerUnitClass CreateKnight()
    {
        print("기사생산");

        // 소환진 활성화

        // 대기시간 다 지나면

        // 반환된 유닛 활성화


        // 기사 유닛 생산자
        playerUnitFactorys[0].knightClass = AbsPlayerUnitFactory.KnightClass.Knight;

        // 생산자 실행
        PlayerUnitClass knight = playerUnitFactorys[0].CreatePlayerUnit();
        //print(UnitDataManager.Instance._unitInfo_Dictionary[knight._unitData.unit_Id]);
        //Vector3 setPos = new Vector3(Input.mousePosition.x, 0f, Input.mousePosition.z);
        //knight.transform.position = setPos;
        knight.gameObject.name = "기사"+ knight._unitData.char_id;

        popUpMgr.isUseShop = false;
        StartCoroutine(popUpMgr.UseUnitPopUp());
        return knight;
    }

    public PlayerUnitClass CreateArcher()
    {
        print("궁수생산");
        // 궁수 유닛 생산자
        playerUnitFactorys[1].archerClass = AbsPlayerUnitFactory.ArcherClass.Archer;

        // 생산자 실행
        PlayerUnitClass Archor = playerUnitFactorys[1].CreatePlayerUnit();
        //Archor.transform.position = Vector3.zero;
        Archor.gameObject.name = "궁수";
        popUpMgr.isUseShop = false;
        StartCoroutine(popUpMgr.UseUnitPopUp());
        return Archor;

    }

    private void ClickUnitFree()
    {
        if (clikUnitInfo==null)
        {
            return;
        }
        print("클릭한 유닛 자유모드");
        clikUnitInfo._enum_Unit_Action_Mode = eUnit_Action_States.unit_FreeMode;
        clikUnitInfo._enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
        //likUnitInfo.transform.position = initPos;
        clikUnitInfo._isSearch = false;
        clikUnitInfo._enum_Unit_Attack_State=eUnit_Action_States.unit_Tracking;
        //clikUnitInfo.transform.position = initPos;

        clikUnitInfo._isClick = false;
        clikUnitInfo = null;
    }

    private void ClickUnitHold()
    {
        if (clikUnitInfo == null)
        {
            return;
        }
        print("클릭한 유닛 홀드모드");
        clikUnitInfo._enum_Unit_Action_Mode = eUnit_Action_States.unit_HoldMode;
        clikUnitInfo._enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
        clikUnitInfo._isSearch = false;
        clikUnitInfo._enum_Unit_Attack_State = eUnit_Action_States.unit_Boundary;
        //initPos = clikUnitInfo.transform.position;

        clikUnitInfo._isClick = false;
        clikUnitInfo = null;
    }

}
