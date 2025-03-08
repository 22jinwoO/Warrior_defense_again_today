using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTracking : MonoBehaviour, IUnitActState
{
    [SerializeField]
    private NewUnitInfo unitInfoCs;

    [SerializeField]
    private UnitBaseController contorllerCs;

    [SerializeField]
    private AbsSearchTarget searchTargetCs;

    [SerializeField]
    private eUnit_Action_States nextActionState;

    public void Enter()
    {
        unitInfoCs = GetComponent<NewUnitInfo>();
        contorllerCs = GetComponent<UnitBaseController>();
        searchTargetCs = GetComponent<AbsSearchTarget>();

    }

    public void DoAction()
    {
        TrackingTarget();
    }

    public void Exit()
    {
        Destroy(this);
    }

    private void OnDestroy()
    {
        Debug.Log($"{this} 파괴2");
        contorllerCs.unitActState = null;
        contorllerCs.actionState = nextActionState;

    }


    #region # TrackingTarget() : 유닛이 타겟을 추적하는 상태일 때 호출되는 함수
    public void TrackingTarget()    // 타겟 추적 상태 시 호출하는 함수
    {
        unitInfoCs._nav.isStopped = false;

        float distance = Vector3.Distance(transform.position, searchTargetCs._targetUnit.position);

        //print("거리 : " + distance);

        //print(distance);
        //unitTargetSearchCs.Look_At_The_Target(next_ActionState);

        if (distance > 0.5f + unitInfoCs._unitData.attackRange && distance <= unitInfoCs._unitData.sightRange)   // 유닛 시야범위보다 작다면
        {
            //if (nav.velocity.Equals(0))
            //    print("오크이동안됨");
            unitInfoCs._anim.SetBool("isMove", true);
            //if (gameObject.CompareTag("Monster"))
            //{
            //    print("몬스터 이동");
            //    nav.SetDestination(unitTargetSearchCs._targetUnit.position - new Vector3(0f, 0f, unitInfoCs._unitData.attackRange - 1f));
            //}
            //else if (gameObject.CompareTag("Player"))
            //{
            //    nav.SetDestination(unitTargetSearchCs._targetUnit.position);
            //    print(unitInfoCs._nav.isStopped);
            //    print(unitTargetSearchCs._targetUnit.position);

            //}
            unitInfoCs._nav.SetDestination(searchTargetCs._targetUnit.position);
        }

        //공격 범위에 적이 들어왔을 때
        else if (distance <= unitInfoCs._unitData.attackRange + 0.5f)
        {
            unitInfoCs._nav.isStopped = true;
            unitInfoCs._anim.SetBool("isMove", false);

            nextActionState = eUnit_Action_States.unit_Attack;

            Exit();
            //unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Attack;
        }

        // 시야밖으로 적이 사라졌을 때
        else if (distance > unitInfoCs._unitData.sightRange)
        {
            unitInfoCs._nav.isStopped = false;
            unitInfoCs._anim.SetBool("isMove", false);

            unitInfoCs._isSearch = false;
            searchTargetCs._targetUnit = null;
            searchTargetCs._target_Body = null;

            nextActionState = eUnit_Action_States.unit_Idle;
            Exit();
        }
    }
    #endregion

}
