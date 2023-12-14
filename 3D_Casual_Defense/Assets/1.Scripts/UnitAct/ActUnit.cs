/*
    ===========================================================================
   ㅣ           유닛의 행동을 구현한 스크립트            ㅣ
    ===========================================================================
 
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
//
public class ActUnit : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent nav;
    
    [SerializeField]
    private UnitInfo unitInfoCs;

    [SerializeField]
    private unit_Data _unitData;

    [SerializeField]
    private eUnit_Action_States _actionStates;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private UnitTargetSearch unitTargetSearchCs;

    
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        unitInfoCs = GetComponent<UnitInfo>();
        _unitData = unitInfoCs._unitData;
        _actionStates = unitInfoCs._enum_Unit_Action_Type;
        anim = GetComponent<Animator>();
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();
    }


    #region # Attack_Unit() : 유닛이 공격할 때 호출되는 함수
    public void Attack_Unit()
    {
        if (nav.enabled)    // 네비메쉬 에이전트가 활성화 되어 있다면
        {
            nav.isStopped = true;
        }
        if (unitInfoCs._can_Base_Attack)
        {
            if (unitInfoCs._can_Skill_Attack)
            {
                Debug.Log("스킬공격!!!");
                if (unitTargetSearchCs._targetUnit == null)
                {
                    return;
                }
                switch (unitInfoCs._enum_Unit_Attack_Type)
                {
                    case eUnit_Action_States.close_Range_atk:   // 근거리 공격일 때
                        anim.SetTrigger("isAttack");

                        print("48" + unitTargetSearchCs._targetUnit);
                        print("49" + unitInfoCs._unitData._unit_Skill_Attack_Damage);
                        print("50" + unitTargetSearchCs._targetUnit.GetComponent<ActUnit>());  // 오류 x
                        print(unitTargetSearchCs._targetUnit.GetComponent<UnitInfo>()._unitData);
                        //print("51"+ unitSearchCs._targetUnit);
                        unitTargetSearchCs._targetUnit.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(unitTargetSearchCs._targetUnit, unitInfoCs._unitData._unit_Skill_Attack_Damage);
                        unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;
                        unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
                        unitInfoCs._enum_Unit_Action_Type = unitInfoCs._enum_Unit_Attack_State;

                        break;

                    case eUnit_Action_States.long_Range_atk:

                        break;
                }

                return; // 함수 탈출

            }

            else
            {
                Debug.Log("기본공격!!!");
                if (unitTargetSearchCs._targetUnit == null)
                {
                    return;
                }
                switch (unitInfoCs._enum_Unit_Attack_Type)
                {
                    case eUnit_Action_States.close_Range_atk:
                        anim.SetTrigger("isAttack");
                        unitTargetSearchCs._targetUnit.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(unitTargetSearchCs._targetUnit, unitInfoCs._unitData._unit_Attack_Damage);
                        unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
                        unitInfoCs._enum_Unit_Action_Type = unitInfoCs._enum_Unit_Attack_State;
                        break;

                    case eUnit_Action_States.long_Range_atk:

                        break;
                }

            }

        }
        if (nav.enabled)    // 네비메쉬 에이전트가 활성화 되어 있다면
        {
            print(nav.gameObject.name);
            nav.isStopped = false;
        }
    }
    #endregion

    public void AnimEvent_Normal_atk()  // 기본공격 시 호출되는 애니메이션
    {
        if (!unitInfoCs._enum_Unit_Attack_Type.Equals(eUnit_Action_States.close_Range_atk))
            return;

        unitTargetSearchCs._targetUnit.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(unitTargetSearchCs._targetUnit, unitInfoCs._unitData._unit_Attack_Damage);
        unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
        unitInfoCs._enum_Unit_Action_Type = unitInfoCs._enum_Unit_Attack_State;
    }


    #region # BeAttacked_By_OtherUnit(Transform other,float attack_Dmg) : 다른 유닛으로부터의 공격으로 피해를 입을 때 호출되는 함수
    public void BeAttacked_By_OtherUnit(Transform other, float attack_Dmg) // 기본공격 일 때와 스킬 공격 일 때 를 나눠야 함...
    {
        print("충돌했음");
        print(other.gameObject.name);
        print(other.GetComponent<UnitInfo>()._unitData);
        unit_Data otherUnitData = other.GetComponent<UnitInfo>()._unitData;
        print(otherUnitData._eUnit_Defense_Property);
        print(unitInfoCs._unitData._unit_Health);
        unitInfoCs._unitData._unit_Health -= unitInfoCs._this_Unit_Armor_Property.CalculateDamaged(unitInfoCs._unitData, otherUnitData, attack_Dmg);
        //print(otherUnitData.);
        print(unitInfoCs._this_Unit_Armor_Property);
        print(attack_Dmg);
        print(unitInfoCs._unitData._unit_Health);

    }
    #endregion

    public void SearchTarget()  // 유닛 탐지
    {
        unitTargetSearchCs.Search_For_Near_Enemy();
    }

    public void MoveUnit(Vector3 arrivePos) // 유닛 이동
    {
        nav.SetDestination(arrivePos);
        float distance = Vector3.Distance(transform.position, arrivePos);
        if (distance <= 1.2f)
        {
            anim.SetBool("isMove", false);

            _actionStates = eUnit_Action_States.unit_Idle;
        }
    }

    public void TrackingTarget()    // 타겟 추적 상태 시 호출하는 함수
    {
        nav.isStopped = false;

        float distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);
        //print("거리 : " + distance);

        unitTargetSearchCs.Look_At_The_Target();

        if (distance >= _unitData._unit_Attack_Range && distance <= _unitData._unit_Outlook)   // 유닛 시야범위보다 작다면
        {
            anim.SetBool("isMove", true);
            nav.SetDestination(unitTargetSearchCs._targetUnit.position);
        }

        // 공격 범위에 적이 들어왔을 때
        else if (distance <= _unitData._unit_Attack_Range)
        {
            print("공격 타입으로 변환");
            nav.SetDestination(transform.position);
            anim.SetBool("isMove", false);
            _actionStates = eUnit_Action_States.unit_Attack;
        }

        // 시야밖으로 적이 사라졌을 때
        else if (distance > _unitData._unit_Outlook)
        {
            nav.SetDestination(transform.position);
            anim.SetBool("isMove", false);

            unitInfoCs.isSearch = false;
            unitTargetSearchCs._targetUnit = null;
            nav.isStopped = false;
            _actionStates = eUnit_Action_States.unit_Idle;
        }
    }

    public void ReadyForAttack(eUnit_Action_States unit_Atk_State = eUnit_Action_States.unit_Tracking)  // Attack 상태일 때 호출 ,기본으로 추적상태를 매개변수로 함
    {
        float distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);

        // 거리가 공격범위보다 크면 유닛 추적
        if (distance > _unitData._unit_Attack_Range)
        {
            unitInfoCs._enum_Unit_Action_Type = unit_Atk_State;
        }

        //공격모션을 실행하고
        unitTargetSearchCs.Look_At_The_Target();
    }

    public void Boundary()  // 유닛 홀드 상태일 때
    {
        float distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);
        unitTargetSearchCs.Look_At_The_Target();
        if (distance <= _unitData._unit_Attack_Range)
        {
            print("공격 타입으로 변환");
            unitInfoCs._enum_Unit_Action_Type = eUnit_Action_States.unit_Attack;
        }
        // 시야 범위 밖으로 적이 사라졌을 때
        else if (distance > _unitData._unit_Outlook)
        {
            unitInfoCs.isSearch = false;
            unitTargetSearchCs._targetUnit = null;
            unitInfoCs._enum_Unit_Action_Type = eUnit_Action_States.unit_Idle;
        }
    }
}
