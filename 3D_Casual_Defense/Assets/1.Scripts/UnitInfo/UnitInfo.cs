//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
//      | 플레이어 유닛이라는 공통점을 묶어서 하나의 클래스로 만들어서 한번에 관리|
//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;


// 함수이름은 명사말고 동사 먼저, enum은 변수이름 앞에 소문자 e 작성, 변수는 카멜표기법으로 소문자 이후 단어 첫글자 대문자
[Serializable]
public struct unit_Data    // 유닛 데이터 가져오는 구조체
{
    public string _unit_Name;            // 유닛 이름
    public float _unit_Health;             // 유닛 체력
    public eUnit_Attack_Property_States _eUnit_Attack_Property;    // 유닛 공격속성
    public float _unit_Attack_Damage;    // 유닛 공격 데미지
    public float _unit_Skill_Attack_Damage;    // 유닛 스킬 공격 데미지
    public eUnit_Defense_Property_States _eUnit_Defense_Property; // 유닛 방어속성
    public string _unit_Description;       // 유닛 설명
    public string _unit_Type;              // 유닛 타입
    public float _unit_MoveSpeed;          // 유닛 이동속도
    public float _unit_Outlook;            // 유닛 시야
    public float _unit_Attack_Range;       // 유닛 공격 범위
    public float _unit_Attack_Speed;        // 유닛 공격 속도
    public float _unit_Attack_CoolTime;     // 유닛 기본 공격 쿨타임
    public float _unit_Current_Skill_CoolTime;     // 유닛 현재 스킬 공격 쿨타임
    public float _unit_Skill_CoolTime;     // 유닛 스킬 공격 쿨타임

}

public enum eUnit_Attack_Property_States  // 유닛 공격 타입
{
    Default = 0,
    slash_Attack,       // 베기 공격
    piercing_Attack,    // 관통 공격
    crushing_attack     // 분쇄 공격
}

public enum eUnit_Defense_Property_States // 유닛 방어 타입
{
    Default = 0,
    plate_Armor,       // 판금 갑옷
    gambeson_Armor,    // 천갑옷
    mail_Armor         // 쇠사슬 갑옷
}

// 공격 킬타입에 따른 몬스터 탐지 함수 추상클래스로 생성 후 상속하여 킬타입에 해당하는 몬스터 탐지함수 실행

public enum eUnit_Action_States           // 유닛 행동
{
    Default = 0,
    unit_Idle,          // 대기
    unit_Move,          // 이동
    unit_Tracking,      // 추적
    unit_Attack,        // 공격
    unit_Boundary       // 홀드 후 주변 경계
}


public abstract class UnitInfo : MonoBehaviour, ISearchForNearEnemy
{
    public unit_Data _unitData; // 유닛 데이터 구조체 변수

    public eUnit_Action_States _enum_Unit_Action_Type = eUnit_Action_States.unit_Idle;   // 유닛 행동 상태 변수
    public eUnit_Action_States _enum_Unit_Attack_Type;   // 플레이어가 지정해준 현재 유닛 공격 상태 변수
    public ArmorCalculate _this_Unit_Armor_Property;

    public NavMeshAgent nav;    // 내비메쉬
    public Animator anim;       // 애니메이터
    public bool isSearch = false;   // 적을 탐지했는지 확인하는 변수
    public Vector3 movePos;

    public bool _can_Base_Attack;    // 기본 공격 가능 불가능 확인하는 변수
    public bool _can_Skill_Attack;   // 스킬 공격 가능 불가능 확인하는 변수



    #region # Act_By_Unit() : 유닛 행동 구분지어주는 함수
    public virtual void Act_By_Unit()  // 유닛 행동 구분지어주는 함수
    {
        switch (_enum_Unit_Action_Type)  // 유닛 행동 구분
        {
            case eUnit_Action_States.unit_Idle: // 유닛 대기 상태

                if (!isSearch)  // 적 탐지 않았을 때만 실행
                {

                    Search_For_Near_Enemy();
                }

                break;

            case eUnit_Action_States.unit_Move: // 유닛 이동
                anim.SetBool("isMove", true);   // 걷는 모션 애니메이션 실행
                nav.SetDestination(movePos);
                float distance = Vector3.Distance(transform.position, movePos);
                if (distance <= 2f)
                {

                    _enum_Unit_Action_Type = eUnit_Action_States.unit_Idle;
                }
                if (distance <= 1f)
                {
                    anim.SetBool("isMove", false);
                }
                break;

            case eUnit_Action_States.unit_Tracking: // 유닛이 몬스터 추적
                                                    //print("타겟 위치"+_targetUnit.position);
                nav.isStopped = false;

                distance = Vector3.Distance(transform.position, _targetUnit.position);
                print("거리 : " + distance);

                Look_At_The_Target();

                if (distance >= _unitData._unit_Attack_Range && distance <= _unitData._unit_Outlook)   // 유닛 시야범위보다 작다면
                {
                    anim.SetBool("isMove", true);
                    nav.SetDestination(_targetUnit.position);
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
                    _targetUnit = null;
                    nav.isStopped = false;
                    _enum_Unit_Action_Type = eUnit_Action_States.unit_Idle;
                }

                break;
            case eUnit_Action_States.unit_Attack:   // 유닛이 몬스터 공격
                distance = Vector3.Distance(transform.position, _targetUnit.position);
                if (distance > _unitData._unit_Attack_Range)
                {
                    _enum_Unit_Action_Type = _enum_Unit_Attack_Type;
                }
                //공격모션을 실행하고
                Look_At_The_Target();


                break;

            case eUnit_Action_States.unit_Boundary: // 유닛 홀드(제자리 경계)

                distance = Vector3.Distance(transform.position, _targetUnit.position);
                Look_At_The_Target();
                if (distance <= _unitData._unit_Attack_Range)
                {
                    print("공격 타입으로 변환");
                    _enum_Unit_Action_Type = eUnit_Action_States.unit_Attack;
                }
                // 시야 범위 밖으로 적이 사라졌을 때
                else if (distance > _unitData._unit_Outlook)
                {
                    isSearch = false;
                    _targetUnit = null;
                    _enum_Unit_Action_Type = eUnit_Action_States.unit_Idle;
                }
                break;

            default:
                print("case 예외 됐음");
                break;
        }
    }
    #endregion

    #region # BeAttacked_By_OtherUnit(Transform other,float attack_Dmg) : 다른 유닛으로부터의 공격으로 피해를 입을 때 호출되는 함수
    public void BeAttacked_By_OtherUnit(Transform other, float attack_Dmg) // 기본공격 일 때와 스킬 공격 일 때 를 나눠야 함...
    {
        print("충돌했음");

        switch (_unitData._eUnit_Defense_Property)
        {
            case eUnit_Defense_Property_States.Default:
                break;

            case eUnit_Defense_Property_States.plate_Armor:
                break;

            case eUnit_Defense_Property_States.gambeson_Armor:  // 몬스터 방어 타입이 천 갑옷 일 때
                                                                //print(gameObject.name);
                                                                //print(other.name);
                unit_Data otherUnitData = other.GetComponent<UnitInfo>()._unitData;
                //print(_this_Unit_Armor_Property.CalculateDamaged(_unitData, otherUnitData, attack_Dmg));
                _unitData._unit_Health -= _this_Unit_Armor_Property.DecreaseDamaged(_unitData, otherUnitData, attack_Dmg);
                //print(gameObject.name + "의 현재 체력" + _unitData._unit_Health);
                break;

            case eUnit_Defense_Property_States.mail_Armor:
                break;

            default:
                break;
        }

    }
    #endregion

    // 구조체 필드 이니셜 라이징은 C# 9.0 에서 지원을 하지 않기 때문에 클래스를 따로 만듦

    public LayerMask _layerMask = 0;   // 오버랩스피어 레이어 마스크 변수

    public Transform _targetUnit = null;   // 유닛이 타겟으로 할 대상

    public bool isTracking; // 추적 확인 변수
    #region # Search_For_Near_Enemy() : 가장 가까운 적 탐지하는 함수 , 시야 범위에서 적 인식
    public void Search_For_Near_Enemy() // 가장 가까운 적 탐지하는 함수 , 시야 범위에서 적 인식
    {
        Collider[] _cols = Physics.OverlapSphere(transform.position, _unitData._unit_Outlook, _layerMask); // 오버랩 스피어 생성
        Transform _shortestTarget = null;  // 가장 가까운 적을 의미하는 변수

        //print(_cols.Length);  
        if (_cols.Length <= 0)  // 탐지된 적이 없다면 함수 탈출
        {
            //print("적 없음!");
            return;
        }

        //CancelInvoke(); // 근데 이게 안돼

        float _shortestDistance = Mathf.Infinity;

        foreach (var _colTarget in _cols)
        {
            float _distance = Vector3.SqrMagnitude(transform.position - _colTarget.transform.position);
            if (_shortestDistance > _distance)
            {
                _shortestDistance = _distance;
                _shortestTarget = _colTarget.transform;
            }
        }
        //
        _targetUnit = _shortestTarget; // 거리가 가장 가까운 적 타겟을 _targetUnit 변수에 할당

        //print(_targetUnit.name);
        isSearch = true;
        _enum_Unit_Action_Type = _enum_Unit_Attack_Type;    // ㅇㅇ 업데이트에서 FSM 상태 실행중

    }
    #endregion


    #region # Look_At_The_Target() : 유닛이 타겟을 감지 했을 때 타겟 쪽으로 몸을 회전하여 타겟을 바라보는 함수
    public void Look_At_The_Target()    // 유닛이 타겟을 감지 했을 때 타겟 쪽으로 몸을 회전하여 타겟을 바라보는 함수
    {
        Vector3 dir = _targetUnit.position - transform.position;
        //dir.Normalize();
        //dir.y = 0;

        Quaternion _lookRotation = Quaternion.LookRotation(dir.normalized);  // 타겟 쪽으로 바라보는 각도

        Vector3 _euler = Quaternion.RotateTowards(transform.rotation, _lookRotation, 100f * Time.deltaTime).eulerAngles;

        transform.rotation = Quaternion.Euler(0, _euler.y, 0);

        Quaternion _fireRotation = Quaternion.Euler(0, _lookRotation.eulerAngles.y, 0); // 유닛이 발사할 수 있는 방향의 각도


        if (Quaternion.Angle(transform.rotation, _fireRotation) <= 5f)
        {
            _euler.y = 0;
            print("용사와 몬스터의 각도 값 : " + Quaternion.Angle(transform.rotation, _fireRotation));

            if (_enum_Unit_Action_Type != eUnit_Action_States.unit_Attack)
                return;

            Attack_Unit();
            // 1. 공격 속도 쿨타임 감소

            // 2. 공격속도 쿨타임이 0보다 작아졌다면 발사 후 공격속도 변수에 초기값 다시 넣어줌

        }
    }
    #endregion

    #region # Attack_Unit() : 유닛이 공격할 때 호출되는 함수
    public void Attack_Unit()
    {
        nav.isStopped = true;
        if (_can_Base_Attack)
        {
            anim.SetTrigger("isAttack");
            if (_can_Skill_Attack)
            {
                Debug.Log("스킬공격!!!");
                if (_targetUnit == null)
                {
                    return;
                }
                _targetUnit.GetComponent<UnitInfo>().BeAttacked_By_OtherUnit(_targetUnit, _unitData._unit_Skill_Attack_Damage);
                _unitData._unit_Current_Skill_CoolTime = 0f;
                _unitData._unit_Attack_CoolTime = 0f;
                _enum_Unit_Action_Type = _enum_Unit_Attack_Type;
                return;
            }

            else
            {
                Debug.Log("기본공격!!!");
                if (_targetUnit == null)
                {
                    return; // 타겟이 없으면 함수 탈출
                }
                _targetUnit.GetComponent<UnitInfo>().BeAttacked_By_OtherUnit(_targetUnit, _unitData._unit_Attack_Damage);
                _unitData._unit_Attack_CoolTime = 0f;
                _enum_Unit_Action_Type = _enum_Unit_Attack_Type;
            }

        }
    }
    #endregion

}
