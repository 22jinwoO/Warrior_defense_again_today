/*
    ===================================================
   ㅣ           유닛의 행동을 구현한 스크립트            ㅣ
    ===================================================
 
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
    private Animator anim;

    [SerializeField]
    private UnitTargetSearch unitTargetSearchCs;

    
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        unitInfoCs = GetComponent<UnitInfo>();
        //print(_unitData._unit_Attack_CoolTime);
        anim = GetComponent<Animator>();
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();

    }
    private void Start()
    {
        _unitData = unitInfoCs._unitData;

    }

    #region # Attack_Unit() : 유닛이 공격할 때 호출되는 함수
    public void Attack_Unit(eUnit_Action_States next_Action_State = eUnit_Action_States.Default)   // 매개변수로 공격 후 다음에 어떤상태로 변환할지 넣어주기
    {
        anim.ResetTrigger("isAttack");

        if (nav.enabled)    // 네비메쉬 에이전트가 활성화 되어 있다면
        {
            nav.isStopped = true;
        }
        if (unitInfoCs._can_Base_Attack)
        {
            if (unitInfoCs._can_Skill_Attack)    // 스킬은 근거리 원거리 나눌 필요 x, UseSkill 함수 호출 플레이어 유닛이라면 
            {
                Debug.Log("스킬공격!!!");
                if (unitTargetSearchCs._targetUnit == null)
                {
                    return;
                }
                anim.SetTrigger("isAttack");
                unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;

                // 몬스터 유닛일 땐 공격 후 딜레이를 주어 다음 상태로 변환
                if (gameObject.CompareTag("Monster")&&unitInfoCs._enum_Unit_Action_Mode.Equals(eUnit_Action_States.monster_NormalMode))
                {
                    StartCoroutine(Change_MonsterState(next_Action_State));
                }

                // 플레이어 유닛은 바로 다음상 상태로 변환
                else
                    unitInfoCs._enum_Unit_Action_State = next_Action_State;

                return; // 함수 탈출

            }

            else
            {
                Debug.Log("기본공격!!!");
                if (unitTargetSearchCs._targetUnit == null)
                {
                    return;
                }
                anim.SetTrigger("isAttack");
                unitInfoCs._unitData._unit_Attack_CoolTime = 0f;


                // 몬스터 유닛일 땐 공격 후 딜레이를 주어 다음 상태로 변환
                if (gameObject.CompareTag("Monster") && unitInfoCs._enum_Unit_Action_Mode.Equals(eUnit_Action_States.monster_NormalMode))
                {
                    StartCoroutine(Change_MonsterState(next_Action_State));
                }
                // 플레이어 유닛은 바로 다음상 상태로 변환
                else
                    unitInfoCs._enum_Unit_Action_State = next_Action_State;

            }
            //anim.ResetTrigger("isAttack");

        }
        if (nav.enabled)    // 네비메쉬 에이전트가 활성화 되어 있다면
        {
            print(nav.gameObject.name);
            nav.isStopped = false;  // 이동 가능 상태로 변환
        }
    }
    #endregion

    public void Anim_LongRangAtk()
    {
        //anim.SetTrigger("isAttack");

        unitInfoCs._projectile_Prefab.GetComponent<Abs_Bullet>()._target_Unit = unitTargetSearchCs._targetUnit;
        unitInfoCs._projectile_Prefab.GetComponent<Abs_Bullet>()._target_BodyTr = unitTargetSearchCs._target_Body;
        unitInfoCs._projectile_Prefab.GetComponent<Abs_Bullet>()._start_Pos = unitInfoCs._projectile_startPos;

        Instantiate(unitInfoCs._projectile_Prefab);
        unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
    }

    public void Anim_LongRang_Skill_Atk()
    {
        //anim.SetTrigger("isAttack");

        unitInfoCs._projectile_Prefab.GetComponent<Abs_Bullet>()._target_Unit = unitTargetSearchCs._targetUnit;
        unitInfoCs._projectile_Prefab.GetComponent<Abs_Bullet>()._target_BodyTr = unitTargetSearchCs._target_Body;
        unitInfoCs._projectile_Prefab.GetComponent<Abs_Bullet>()._start_Pos = unitInfoCs._projectile_startPos;

        Instantiate(unitInfoCs._projectile_Prefab);
        print("공격 실행");
        unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;
        unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
    }

    #region # Change_MonsterState(eUnit_Action_States next_Action_State) : 몬스터가 공격 후 다음 상태로 변환 시 딜레이를 주기 호출되는 함수
    IEnumerator Change_MonsterState(eUnit_Action_States next_Action_State)
    {
        unitInfoCs._isSearch = false;
        unitTargetSearchCs._targetUnit = null;
        yield return new WaitForSeconds(1.5f);

        nav.isStopped = true;

        unitInfoCs._enum_Unit_Action_State = next_Action_State;
    }
    #endregion

    #region # AnimEvent_Normal_Atk() : 일반 스킬 애니메이션 동작 시 호출되는 애니메이션 이벤트 함수
    public void AnimEvent_Normal_Atk()  // 일반 스킬 사용 시 호출되는 애니메이션
    {
        //if (!unitInfoCs._enum_Unit_Attack_Type.Equals(eUnit_Action_States.close_Range_Atk))
        //    return;
        print("애니메이션 호출 함수");
        unitInfoCs.gen_skill.unitInfoCs = unitInfoCs;   // 나중에 유닛 awake 문에서 한번만 실행하도록 변경하기
        unitInfoCs.gen_skill.unitTargetSearchCs = unitTargetSearchCs;
        unitInfoCs.gen_skill.Attack_Skill();
        ////unitTargetSearchCs._targetUnit.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(,unitTargetSearchCs._targetUnit, unitInfoCs._unitData._unit_General_Skill_Dmg);
        //unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
        //unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;


        // 스킬 공격
        //unitTargetSearchCs._targetUnit.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(unitTargetSearchCs._targetUnit, unitInfoCs._unitData._unit_Special_Skill_Dmg);
        //unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;
        //unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
    }
    #endregion

    #region # BeAttacked_By_OtherUnit(Transform other,float attack_Dmg) : 다른 유닛으로부터의 공격으로 피해를 입을 때 호출되는 함수
    public void BeAttacked_By_OtherUnit(eUnit_Attack_Property_States myAtkType, Transform other, float attack_Dmg) // 기본공격 일 때와 스킬 공격 일 때 를 나눠야 함...
    {
        print("충돌했음");
        print(other.gameObject.name);
        print(other.GetComponent<UnitInfo>()._unitData);
        unit_Data otherUnitData = other.GetComponent<UnitInfo>()._unitData;
        print(otherUnitData._eUnit_Defense_Property);
        print(unitInfoCs._unitData._unit_maxHealth);
        unitInfoCs._unitData._unit_maxHealth -= unitInfoCs._this_Unit_Armor_Property.CalculateDamaged(attackType : myAtkType, ArmorType : otherUnitData, attack_Dmg : attack_Dmg);
        //print(otherUnitData.);
        print(unitInfoCs._this_Unit_Armor_Property);
        print(attack_Dmg);
        print(unitInfoCs._unitData._unit_maxHealth);

    }
    #endregion

    #region # SearchTarget(매개변수 : 유닛 탐지 타입) : 유닛이 Idle 상태일 때 타겟 탐지 시 호출되는 함수
    public void SearchTarget(eUnit_targetSelectType target_Search_Type = eUnit_targetSelectType.Default)  // 유닛 탐지
    {
        switch (target_Search_Type)
        {
            case eUnit_targetSelectType.fixed_Target:
                unitTargetSearchCs.Search_For_Fixed_Target();
                break;

            case eUnit_targetSelectType.nearest_Target:
                unitTargetSearchCs.Search_For_Nearest_Target();
                break;

            case eUnit_targetSelectType.low_Health_Target:
                unitTargetSearchCs.Search_For_Lowhealth_Target();
                break;
        }
        
    }
    #endregion

    #region # MoveUnit(Vector3 arrivePos) : 유닛 상태가 Move일 때 호출되는 함수 - 이동 시 호출
    public void MoveUnit(Vector3 arrivePos) // 유닛 이동
    {
        nav.SetDestination(arrivePos);
        float distance = Vector3.Distance(transform.position, arrivePos);
        if (distance <= 1.2f)
        {
            anim.SetBool("isMove", false);

            unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
        }
    }
    #endregion

    #region # TrackingTarget(eUnit_Action_States next_ActionState = eUnit_Action_States.Default) : 유닛이 타겟을 추적하는 상태일 때 호출되는 함수
    public void TrackingTarget(eUnit_Action_States next_ActionState = eUnit_Action_States.Default)    // 타겟 추적 상태 시 호출하는 함수
    {
        nav.isStopped = false;

        float distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);
        //print("거리 : " + distance);

        //print(distance);
        unitTargetSearchCs.Look_At_The_Target(next_ActionState);

        if (distance >= _unitData.attackRange && distance <= _unitData.sightRange)   // 유닛 시야범위보다 작다면
        {
            print(240);
            anim.SetBool("isMove", true);
            nav.SetDestination(unitTargetSearchCs._targetUnit.position);
        }

        // 공격 범위에 적이 들어왔을 때
        else if (distance <= _unitData.attackRange)
        {
            print("공격 타입으로 변환");
            nav.SetDestination(transform.position);
            anim.SetBool("isMove", false);
            unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Attack;
        }

        // 시야밖으로 적이 사라졌을 때
        else if (distance > _unitData.sightRange)
        {
            nav.SetDestination(transform.position);
            anim.SetBool("isMove", false);

            unitInfoCs._isSearch = false;
            unitTargetSearchCs._targetUnit = null;
            unitTargetSearchCs._target_Body = null;
            nav.isStopped = false;
            unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
        }
    }

    #endregion

    #region # ReadyForAttack(eUnit_Action_States unit_Atk_State = eUnit_Action_States.Default) : 유닛 상태가 Attack 상태일 때 호출, 현재 공격 지정 상태를 매개변수로 함
    public void ReadyForAttack(eUnit_Action_States unit_Atk_State = eUnit_Action_States.Default)  // Attack 상태일 때 호출 ,기본으로 추적상태를 매개변수로 함
    {
        float distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);

        // 거리가 공격범위보다 크면 유닛 추적
        if (distance > _unitData.attackRange)
        {
            unitInfoCs._enum_Unit_Action_State = unitInfoCs._enum_Unit_Attack_State;
        }

        //공격모션을 실행하고
        unitTargetSearchCs.Look_At_The_Target(unit_Atk_State);
    }
    #endregion

    #region # Boundary() : 플레이어 유닛이 홀드상태일 때 호출되는 함수
    public void Boundary()  // 유닛 홀드 상태일 때
    {
        print("호출");
        float distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);
        unitTargetSearchCs.Look_At_The_Target(next_Action_State : eUnit_Action_States.unit_Boundary);
        if (distance <= _unitData.attackRange)
        {
            print("공격 타입으로 변환");
            unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Attack;
        }
        // 시야 범위 밖으로 적이 사라졌을 때
        else if (distance > _unitData.sightRange)
        {
            unitInfoCs._isSearch = false;
            unitTargetSearchCs._targetUnit = null;
            unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
        }
    }
    #endregion

}
