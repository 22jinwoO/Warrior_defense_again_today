using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttack : MonoBehaviour, IUnitActState
{
    [Header("유닛 정보 스크립트")]
    [SerializeField]
    private NewUnitInfo unitInfoCs;

    [Header("유닛 컨트롤러 스크립트")]
    [SerializeField]
    private UnitBaseController controllerCs;


    [Header("유닛 컨트롤러 스크립트")]
    [SerializeField]
    private UnitAttackEvent atkEventCs;


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

        atkEventCs = GetComponent<UnitAttackEvent>();

        anim.ResetTrigger("isAttack");
        anim.ResetTrigger("isSkillAtk");

    }

    public void DoAction()
    {
        if (unitInfoCs._can_genSkill_Attack)
        {
            anim.SetTrigger("isAttack");
            unitInfoCs._unitData._unit_Attack_Speed = 0f;
            Exit();
        }

        else if (unitInfoCs._can_SpcSkill_Attack)
        {
            anim.SetTrigger("isSkillAtk");
            unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;
            Exit();
        }
    }

    public void Exit()
    {
        Destroy(this);

    }


    private void OnDestroy()
    {
        Debug.Log($"{this} 파괴2");
        controllerCs.unitActState = null;
        controllerCs.actionState = eUnit_Action_States.unit_Tracking;
    }
}
