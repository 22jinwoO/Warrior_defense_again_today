using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Knight : PlayerUnitClass
{
    [SerializeField]
    private bool isCollision;

    private void Awake()
    {
        
        navObs=GetComponent<NavMeshObstacle>();
        _anim = GetComponent<Animator>();
        _this_Unit_Armor_Property = new GambesonArmor();
        _nav = GetComponent<NavMeshAgent>();
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();
        actUnitCs=GetComponent<ActUnit>();
        _isClick = false;
        InitUnitInfoSetting();  // 유닛 정보 초기화 시켜주는 함수
    }

    public void Update()
    {
        transform.eulerAngles = Vector3.zero;

        if (_isClick&&Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _movePos = hit.point;
                _enum_Unit_Action_State = eUnit_Action_States.unit_Move;
            }
        }
        Unit_Attack_Skill_CoolTime();   // 유닛 기본 공격, 스킬 공격 쿨타임 돌려주는 함수
    }
    private void FixedUpdate()
    {
        Act_By_Unit();  // 유닛 행동 함수
    }
    //#region # Act_By_Unit() : 유닛 행동 구분지어주는 함수, IActByUnit 인터페이스 함수 정의

    //public void Act_By_Unit()  // 유닛 행동 구분지어주는 함수
    //{
    //    switch (_enum_Unit_Action_Mode) // 유닛 모드에 따라 행동
    //    {
    //        case eUnit_Action_States.unit_FreeMode: // 유닛 자유 모드일 때 행동 구분
    //            holdOb.SetActive(false);
    //            navObs.enabled = false;
    //            _nav.enabled = true;
    //            switch (_enum_Unit_Action_State)     // 현재 유닛 행동
    //            {
    //                case eUnit_Action_States.unit_Idle: // 유닛 대기 상태(탐지 상태)
    //                    _anim.SetBool("isMove", false);
    //                    if (!_isSearch)  // 적 탐지 못했을 때만 실행
    //                    {
    //                        actUnitCs.SearchTarget(target_Search_Type : _eUnit_Target_Search_Type);
    //                    }
    //                    break;

    //                case eUnit_Action_States.unit_Move: // 유닛 이동
    //                    _isSearch = false;
    //                    _isClick = false;
    //                    unitTargetSearchCs._targetUnit = null;
    //                    _nav.isStopped = false;
    //                    _anim.SetBool("isMove", true);   // 걷는 모션 애니메이션 실행
    //                    actUnitCs.MoveUnit(_movePos);
    //                    break;

    //                case eUnit_Action_States.unit_Tracking: // 유닛이 몬스터 추적
    //                    actUnitCs.TrackingTarget();
    //                    break;

    //                case eUnit_Action_States.unit_Attack:   // 유닛이 몬스터 공격
    //                    actUnitCs.ReadyForAttack(unit_Atk_State : eUnit_Action_States.unit_Tracking);
    //                    break;

    //            }
    //            break;

    //        case eUnit_Action_States.unit_HoldMode:

    //            holdOb.SetActive(true);
    //            navObs.enabled = true;
    //            _nav.enabled = false;
    //            switch (_enum_Unit_Action_State)
    //            {
    //                case eUnit_Action_States.unit_Idle: // 유닛 대기 상태
    //                    _anim.SetBool("isMove", false);
    //                    if (!_isSearch)  // 적 탐지 못했을 때만 실행
    //                    {
    //                        actUnitCs.SearchTarget(_eUnit_Target_Search_Type);
    //                    }

    //                    break;

    //                case eUnit_Action_States.unit_Attack:   // 유닛이 몬스터 공격
    //                    actUnitCs.ReadyForAttack(unit_Atk_State : eUnit_Action_States.unit_Boundary);
    //                    break;

    //                case eUnit_Action_States.unit_Boundary: // 유닛 홀드(제자리 경계)
    //                    actUnitCs.Boundary();
    //                    break;
    //            }
    //            break;
    //    }
    //}

    //#endregion  IActByUnit 함수 


    #region # InitUnitInfoSetting(): 유닛 정보 셋팅하는 함수
    public override void InitUnitInfoSetting()
    {
        _unitData._unit_Name = "용사";                                                        // 유닛 이름
        _unitData._unit_maxHealth = 200f;                                                       // 유닛 체력
        _unitData._eUnit_genSkill_Property = eUnit_Attack_Property_States.slash_Attack;      // 유닛 공격속성
        _unitData._unit_Attack_Damage = 1f;                                                  // 유닛 공격 데미지
        _unitData._unit_Skill_Attack_Damage = 6f;                                            // 유닛 공격 데미지
        _unitData._eUnit_Defense_Property = eUnit_Defense_Property_States.gambeson_Armor;    // 유닛 방어속성
        _unitData._unit_Description = "용사입니다";                                           // 유닛 설명
        _unitData._unit_Type = "용사";                                                       // 유닛 타입
        _unitData._unit_MoveSpeed = 1f;                                                      // 유닛 이동속도
        _unitData._unit_SightRange = 8f;                                                     // 유닛 시야
        _unitData._unit_Attack_Range = 4f;                                                   // 유닛 공격 범위
        _unitData._unit_Attack_Speed = 3f;                                                   // 유닛 공격 속도
        _unitData._unit_Attack_CoolTime = 5f;                                                // 유닛 기본 공격 쿨타임
        _unitData._unit_Skill_CoolTime = 8f;                                                 // 유닛 스킬 공격 쿨타임
    }
    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (unitTargetSearchCs._targetUnit != null && unitTargetSearchCs._targetUnit.Equals(other))
        {
            _nav.isStopped = true;
            print("트리거 콜라이더 충돌");

        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (unitTargetSearchCs._targetUnit != null && unitTargetSearchCs._targetUnit.Equals(other))
        {
            _nav.isStopped = false;
            print("트리거 콜라이더 나감");

        }

    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, _unitData._unit_SightRange);

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, _unitData._unit_Attack_Range);
    //}
}
