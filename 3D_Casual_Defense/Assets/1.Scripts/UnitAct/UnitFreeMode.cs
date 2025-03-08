using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFreeMode : MonoBehaviour, IUnitModeState
{
    [SerializeField]
    private NewUnitInfo unitInfoCs;

    [SerializeField]
    private UnitBaseController controllerCs;

    public void DoModeAction()
    {
        setActionType(controllerCs.actionState);
    }

    public void EnterMode()
    {
        unitInfoCs = GetComponent<NewUnitInfo>();
        controllerCs = GetComponent<UnitBaseController>();
    }

    public void ExitMode()
    {
        throw new System.NotImplementedException();
    }

    #region
    public void setActionType(eUnit_Action_States state)
    {

        // 현재 상태 저장
        this.controllerCs.actionState = state;

        // 다양한 상태 중에 어떤 것을 가져와야 할 지 모르므로
        // 인터페이스를 대표로 해서 가져온다.

        // 자유 모드 상태 중 유닛 행동 상태에 따른 FSM
        switch (state)
        {
            case eUnit_Action_States.unit_Idle:
                if (controllerCs.unitActState == null)
                {
                    controllerCs.unitActState = gameObject.AddComponent<UnitIdle>();

                    Debug.Log("출력");
                    controllerCs.unitActState.Enter();
                }

                if (!unitInfoCs._isSearch)
                {
                    controllerCs.unitActState.DoAction();
                }

                if (unitInfoCs._isSearch)
                {
                    controllerCs.unitActState.Exit();
                }
                Debug.Log("출력2");
                break;

            case eUnit_Action_States.unit_Tracking:
                if (controllerCs.unitActState == null)
                {
                    controllerCs.unitActState = gameObject.AddComponent<UnitTracking>();

                    controllerCs.unitActState.Enter();
                }

                controllerCs.unitActState.DoAction();
                break;

            case eUnit_Action_States.unit_Attack:
                if (controllerCs.unitActState == null)
                {
                    controllerCs.unitActState = gameObject.AddComponent<UnitAttack>();

                    controllerCs.unitActState.Enter();
                }

                controllerCs.unitActState.DoAction();
                break;

            default:
                break;
        }
    }
    #endregion

}
