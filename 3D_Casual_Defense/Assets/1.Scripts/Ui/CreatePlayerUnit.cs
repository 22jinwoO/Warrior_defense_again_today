using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        clickKnightBtn.onClick.AddListener(CreateKnight);
        clickArcherBtn.onClick.AddListener(CreateArcher);
        clickUnitFreeBtn.onClick.AddListener(ClickUnitFree);
        clickUnitHoldBtn.onClick.AddListener(ClickUnitHold);
    }

    // Update is called once per frame


    private void CreateKnight()
    {
        print("기사생산");


        // 기사 유닛 생산자
        playerUnitFactorys[0].knightClass = AbsPlayerUnitFactory.KnightClass.Knight;

        // 생산자 실행
        PlayerUnitClass knight = playerUnitFactorys[0].CreatePlayerUnit();
        //print(UnitDataManager.Instance._unitInfo_Dictionary[knight._unitData.unit_Id]);
        knight.transform.position = Vector3.zero;
        knight.gameObject.name = "기사";

        popUpMgr.isUseShop = false;
        StartCoroutine(popUpMgr.UseUnitPopUp());
    }
    private void CreateArcher()
    {
        print("궁수생산");
        // 궁수 유닛 생산자
        playerUnitFactorys[1].archerClass = AbsPlayerUnitFactory.ArcherClass.Archer;

        // 생산자 실행
        PlayerUnitClass knight = playerUnitFactorys[1].CreatePlayerUnit();
        knight.transform.position = Vector3.zero;
        knight.gameObject.name = "궁수";
        popUpMgr.isUseShop = false;
        StartCoroutine(popUpMgr.UseUnitPopUp());

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
