using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Knight : PlayerUnitClass, IActByUnit
{
    [SerializeField]
    GameObject holdOb;

    [SerializeField]
    NavMeshObstacle navObs;

    private void Awake()
    {
        navObs=GetComponent<NavMeshObstacle>();
        anim = GetComponent<Animator>();
        _this_Unit_Armor_Property = new GambesonArmor();
        nav = GetComponent<NavMeshAgent>();
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();
        isClick = false;
        InitUnitInfoSetting();  // 유닛 정보 초기화 시켜주는 함수
    }

    public void Update()
    {

        if (isClick&&Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                movePos = hit.point;
                _enum_Unit_Action_Type = eUnit_Action_States.unit_Move;
            }
        }
        Unit_Attack_Skill_CoolTime();   // 유닛 기본 공격, 스킬 공격 쿨타임 돌려주는 함수
        Act_By_Unit();  // 유닛 행동 함수
    }

    #region # Act_By_Unit() : 유닛 행동 구분지어주는 함수, IActByUnit 인터페이스 함수 정의

    public void Act_By_Unit()  // 유닛 행동 구분지어주는 함수
    {
        switch (_enum_Unit_Action_Mode) // 유닛 모드에 따라 행동
        {
            case eUnit_Action_States.unit_FreeMode: // 유닛 자유 모드일 때 행동 구분
                holdOb.SetActive(false);
                navObs.enabled = false;
                nav.enabled = true;
                switch (_enum_Unit_Action_Type)     // 현재 유닛 행동
                {
                    case eUnit_Action_States.unit_Idle: // 유닛 대기 상태
                        anim.SetBool("isMove", false);
                        if (!isSearch)  // 적 탐지 않았을 때만 실행
                        {

                            unitTargetSearchCs.Search_For_Near_Enemy();
                        }

                        break;

                    case eUnit_Action_States.unit_Move: // 유닛 이동
                        isSearch = false;
                        isClick = false;
                        unitTargetSearchCs._targetUnit = null;
                        nav.isStopped = false;
                        anim.SetBool("isMove", true);   // 걷는 모션 애니메이션 실행
                        nav.SetDestination(movePos);
                        float distance = Vector3.Distance(transform.position, movePos);
                        if (distance <= 1.2f)
                        {
                            anim.SetBool("isMove", false);

                            _enum_Unit_Action_Type = eUnit_Action_States.unit_Idle;
                        }

                        break;

                    case eUnit_Action_States.unit_Tracking: // 유닛이 몬스터 추적
                                                            //print("타겟 위치"+_targetUnit.position);
                        nav.isStopped = false;

                        distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);
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
                            anim.SetBool("isMove", false);
                            _enum_Unit_Action_Type = eUnit_Action_States.unit_Attack;
                        }

                        // 시야밖으로 적이 사라졌을 때
                        else if (distance > _unitData._unit_Outlook)
                        {
                            nav.SetDestination(transform.position);
                            anim.SetBool("isMove", false);

                            isSearch = false;
                            unitTargetSearchCs._targetUnit = null;
                            nav.isStopped = false;
                            _enum_Unit_Action_Type = eUnit_Action_States.unit_Idle;
                        }
                        break;

                    case eUnit_Action_States.unit_Attack:   // 유닛이 몬스터 공격
                        distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);

                        // 거리가 공격범위보다 크면 유닛 추적
                        if (distance > _unitData._unit_Attack_Range)
                        {
                            _enum_Unit_Action_Type = _enum_Unit_Attack_Type;
                        }

                        //공격모션을 실행하고
                        unitTargetSearchCs.Look_At_The_Target();
                        break;

                }
                break;

            case eUnit_Action_States.unit_HoldMode:

                holdOb.SetActive(true);
                navObs.enabled = true;
                nav.enabled = false;
                switch (_enum_Unit_Action_Type)
                {
                    case eUnit_Action_States.unit_Idle: // 유닛 대기 상태
                        anim.SetBool("isMove", false);
                        if (!isSearch)  // 적 탐지 않았을 때만 실행
                        {

                            unitTargetSearchCs.Search_For_Near_Enemy();
                        }

                        break;

                    case eUnit_Action_States.unit_Attack:   // 유닛이 몬스터 공격
                        float distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);
                        if (distance > _unitData._unit_Attack_Range)
                        {
                            _enum_Unit_Action_Type = _enum_Unit_Attack_Type;
                        }
                        //공격모션을 실행하고
                        unitTargetSearchCs.Look_At_The_Target();


                        break;

                    case eUnit_Action_States.unit_Boundary: // 유닛 홀드(제자리 경계)

                        distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);
                        unitTargetSearchCs.Look_At_The_Target();
                        if (distance <= _unitData._unit_Attack_Range)
                        {
                            print("공격 타입으로 변환");
                            _enum_Unit_Action_Type = eUnit_Action_States.unit_Attack;
                        }
                        // 시야 범위 밖으로 적이 사라졌을 때
                        else if (distance > _unitData._unit_Outlook)
                        {
                            isSearch = false;
                            unitTargetSearchCs._targetUnit = null;
                            _enum_Unit_Action_Type = eUnit_Action_States.unit_Idle;
                        }
                        break;
                }
                break;
        }

        //switch (_enum_Unit_Action_Type)  // 유닛 행동 구분
        //{
        //    case eUnit_Action_States.unit_Idle: // 유닛 대기 상태

        //        if (!isSearch)  // 적 탐지 않았을 때만 실행
        //        {

        //            unitTargetSearchCs.Search_For_Near_Enemy();
        //        }

        //        break;

        //    case eUnit_Action_States.unit_Move: // 유닛 이동
        //        isSearch = false;
        //        unitTargetSearchCs._targetUnit = null;
        //        nav.isStopped = false;
        //        anim.SetBool("isMove", true);   // 걷는 모션 애니메이션 실행
        //        nav.SetDestination(movePos);
        //        float distance = Vector3.Distance(transform.position, movePos);
        //        if (distance <= 1.2f)
        //        {
        //            anim.SetBool("isMove", false);

        //            _enum_Unit_Action_Type = eUnit_Action_States.unit_Idle;
        //        }

        //        break;

        //    case eUnit_Action_States.unit_Tracking: // 유닛이 몬스터 추적
        //                                            //print("타겟 위치"+_targetUnit.position);
        //        nav.isStopped = false;

        //        distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);
        //        //print("거리 : " + distance);

        //        unitTargetSearchCs.Look_At_The_Target();

        //        if (distance >= _unitData._unit_Attack_Range && distance <= _unitData._unit_Outlook)   // 유닛 시야범위보다 작다면
        //        {
        //            anim.SetBool("isMove", true);
        //            nav.SetDestination(unitTargetSearchCs._targetUnit.position);
        //        }

        //        // 공격 범위에 적이 들어왔을 때
        //        else if (distance <= _unitData._unit_Attack_Range)
        //        {
        //            print("공격 타입으로 변환");
        //            anim.SetBool("isMove", false);
        //            _enum_Unit_Action_Type = eUnit_Action_States.unit_Attack;
        //        }

        //        // 시야밖으로 적이 사라졌을 때
        //        else if (distance > _unitData._unit_Outlook)
        //        {
        //            nav.SetDestination(transform.position);
        //            anim.SetBool("isMove", false);

        //            isSearch = false;
        //            unitTargetSearchCs._targetUnit = null;
        //            nav.isStopped = false;
        //            _enum_Unit_Action_Type = eUnit_Action_States.unit_Idle;
        //        }

        //        break;
        //    case eUnit_Action_States.unit_Attack:   // 유닛이 몬스터 공격
        //        distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);
        //        if (distance > _unitData._unit_Attack_Range)
        //        {
        //            _enum_Unit_Action_Type = _enum_Unit_Attack_Type;
        //        }
        //        //공격모션을 실행하고
        //        unitTargetSearchCs.Look_At_The_Target();


        //        break;

        //    case eUnit_Action_States.unit_Boundary: // 유닛 홀드(제자리 경계)

        //        distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);
        //        unitTargetSearchCs.Look_At_The_Target();
        //        if (distance <= _unitData._unit_Attack_Range)
        //        {
        //            print("공격 타입으로 변환");
        //            _enum_Unit_Action_Type = eUnit_Action_States.unit_Attack;
        //        }
        //        // 시야 범위 밖으로 적이 사라졌을 때
        //        else if (distance > _unitData._unit_Outlook)
        //        {
        //            isSearch = false;
        //            unitTargetSearchCs._targetUnit = null;
        //            _enum_Unit_Action_Type = eUnit_Action_States.unit_Idle;
        //        }
        //        break;

        //    default:
        //        print("case 예외 됐음");
        //        break;
        //}
    }

    #endregion  IActByUnit 함수 


    #region # InitUnitInfoSetting(): 유닛 정보 셋팅하는 함수
    public override void InitUnitInfoSetting()
    {
        _unitData._unit_Name = "용사";            // 유닛 이름
        _unitData._unit_Health = 200f;             // 유닛 체력
        _unitData._eUnit_Attack_Property = eUnit_Attack_Property_States.slash_Attack;    // 유닛 공격속성
        _unitData._unit_Attack_Damage = 1f;    // 유닛 공격 데미지
        _unitData._unit_Skill_Attack_Damage = 6f;    // 유닛 공격 데미지
        _unitData._eUnit_Defense_Property = eUnit_Defense_Property_States.gambeson_Armor;   // 유닛 방어속성
        _unitData._unit_Description = "용사입니다";     // 유닛 설명
        _unitData._unit_Type = "용사";            // 유닛 타입
        _unitData._unit_MoveSpeed = 1f;        // 유닛 이동속도
        _unitData._unit_Outlook = 8f;          // 유닛 시야
        _unitData._unit_Attack_Range = 4f;     // 유닛 공격 범위
        _unitData._unit_Attack_Speed = 3f;        // 유닛 공격 속도
        _unitData._unit_Attack_CoolTime = 0f;     // 유닛 기본 공격 쿨타임
        _unitData._unit_Skill_CoolTime = 8f;     // 유닛 스킬 공격 쿨타임
    }
    #endregion


    #region # Unit_Attack_Skill_CoolTime() : 유닛 기본공격, 스킬공격 쿨타임 돌려주는 함수
    public override void Unit_Attack_Skill_CoolTime()
    {
        // 기본 공격이 가능한지 확인
        _can_Base_Attack = _unitData._unit_Attack_CoolTime >= _unitData._unit_Attack_Speed ? true : false;

        // 스킬 공격이 가능한지 확인
        _can_Skill_Attack = _unitData._unit_Current_Skill_CoolTime >= _unitData._unit_Skill_CoolTime ? true : false;

        //현재 스킬 공격 쿨타임이 유닛의 스킬 공격 쿨타임 보다 낮다면 쿨타임 돌려주기
        if (_unitData._unit_Skill_CoolTime >= _unitData._unit_Current_Skill_CoolTime)
        {
            _unitData._unit_Current_Skill_CoolTime += Time.deltaTime;
        }

        //현재 기본 공격 쿨타임이 유닛의 기본 공격속도 보다 낮다면 쿨타임 돌려주기
        if (_unitData._unit_Attack_Speed >= _unitData._unit_Attack_CoolTime)
        {
            _unitData._unit_Attack_CoolTime += Time.deltaTime;
        }
    }

    #endregion

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, _unitData._unit_Outlook);

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, _unitData._unit_Attack_Range);
    //}
}
