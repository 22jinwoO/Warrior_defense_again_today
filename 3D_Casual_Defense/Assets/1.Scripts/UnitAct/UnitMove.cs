using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMove : MonoBehaviour, IUnitActState
{
    [Header("유닛 정보 스크립트")]
    [SerializeField]
    private NewUnitInfo unitInfoCs;

    [Header("유닛 컨트롤러 스크립트")]
    [SerializeField]
    private UnitBaseController controllerCs;


    [Header("애니메이터")]
    [SerializeField]
    private Animator anim;

    [Header("네비 메쉬 에이전트")]
    [SerializeField]
    private NavMeshAgent nav;


    public void Enter()
    {
        unitInfoCs = GetComponent<NewUnitInfo>();
        nav = unitInfoCs.GetComponent<NavMeshAgent>();
        anim = unitInfoCs.GetComponent<Animator>();
        controllerCs = GetComponent<UnitBaseController>();
    }

    public void DoAction()
    {
        // 플레이어 유닛 이동
        MoveUnit(controllerCs.arrivePos);
    }

    public void Exit()
    {
        Destroy(this);

    }

    // 플레이어 유닛 이동
    #region # MoveUnit(Vector3 arrivePos) : 유닛 상태가 Move일 때 호출되는 함수 - 이동 시 호출
    public void MoveUnit(Vector3 arrivePos) // 유닛 이동
    {
        anim.SetBool("isMove", true);
        nav.SetDestination(arrivePos);

        float distance = Vector3.Distance(transform.position, arrivePos);
        if (distance <= 1.2f)
        {
            anim.SetBool("isMove", false);
            Exit();
        }
    }
    #endregion

    private void OnDestroy()
    {
        Debug.Log($"{this} 파괴2");
        controllerCs.unitActState = null;
        controllerCs.actionState = eUnit_Action_States.unit_Idle;
    }
}
