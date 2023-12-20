using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePlayerUnit : MonoBehaviour
{
    [SerializeField]
    AbsPlayerUnitFactory[] playerUnitFactorys;

    [SerializeField]
    Button clickKnightBtn;

    [SerializeField]
    Button clickUnitFreeBtn;

    [SerializeField]
    Button clickUnitHoldBtn;


    public UnitInfo clikUnitInfo;

    // Start is called before the first frame update
    void Start()
    {
        clickKnightBtn.onClick.AddListener(CreateKnight);
        clickUnitFreeBtn.onClick.AddListener(ClickUnitFree);
        clickUnitHoldBtn.onClick.AddListener(ClickUnitHold);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{   
        //    // 기사 유닛 생산자
        //    print("스페이스 바 눌림!");
        //    playerUnitFactorys[0].knightClass = AbsPlayerUnitFactory.KnightClass.Knight;
        //    PlayerUnitClass knight = playerUnitFactorys[0].CreatePlayerUnit();
        //    knight.transform.position = Vector3.zero;
        //    knight.gameObject.name = "기사";
        //}
    }

    private void CreateKnight()
    {
        // 기사 유닛 생산자
        print("스페이스 바 눌림!");
        playerUnitFactorys[0].knightClass = AbsPlayerUnitFactory.KnightClass.Knight;
        PlayerUnitClass knight = playerUnitFactorys[0].CreatePlayerUnit();
        knight.transform.position = Vector3.zero;
        knight.gameObject.name = "기사";
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
        clikUnitInfo._isSearch = false;
        clikUnitInfo._enum_Unit_Attack_State=eUnit_Action_States.unit_Tracking;
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
        clikUnitInfo._isClick = false;
        clikUnitInfo = null;
    }
}
