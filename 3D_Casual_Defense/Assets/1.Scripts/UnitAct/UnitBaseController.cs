using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBaseController : MonoBehaviour
{
    // Concrete클래스들의 접근점(인터페이스)
    public IUnitModeState unitModeState;

    public IUnitActState unitActState;

    [Header("리지드 바디")]
    public Rigidbody rigid;

    public eUnit_Action_States actionState;

    public eUnit_Action_States modeState;

    public Vector3 arrivePos;

    private void Awake()
    {
        modeState = eUnit_Action_States.unit_FreeMode;
        actionState = eUnit_Action_States.unit_Idle;
    }


    void Update()
    {
        switch (modeState)
        {
            case eUnit_Action_States.unit_FreeMode:

                setModeType(eUnit_Action_States.unit_FreeMode);

                break;

            case eUnit_Action_States.unit_HoldMode:

                setModeType(eUnit_Action_States.unit_HoldMode);
                break;
        }
    }



    // 상태 클래스 교환...
    public void setModeType(eUnit_Action_States state)
    {

        // 현재 상태 저장
        this.modeState = state;

        // 다양한 상태 중에 어떤 것을 가져와야 할 지 모르므로
        // 인터페이스를 대표로 해서 가져온다.


        switch (state)
        {
            //자유 모드일 때
            case eUnit_Action_States.unit_FreeMode:

                if (unitModeState == null)
                {
                    unitModeState = gameObject.AddComponent<UnitFreeMode>();

                    Debug.Log("출력");
                    unitModeState.EnterMode();
                }

                unitModeState.DoModeAction();

                Debug.Log("출력2");
                break;

            // 홀드 모드일 때
            case eUnit_Action_States.unit_HoldMode:

                if (unitModeState == null)
                {
                    unitModeState = gameObject.AddComponent<UnitHoldMode>();

                    unitModeState.EnterMode();
                }

                unitModeState.DoModeAction();

                break;

            default:
                break;
        }
    }


}
