//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
//      | 플레이어 유닛이라는 공통점을 묶어서 하나의 클래스로 만들어서 한번에 관리|
//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// 함수이름은 명사말고 동사 먼저, enum은 변수이름 앞에 소문자 e 작성, 변수는 카멜표기법으로 소문자 이후 단어 첫글자 대문자
[Serializable]
public struct unit_Data    // 유닛 데이터 가져오는 구조체
{
    public string m_unit_Name;            // 유닛 이름
    public float m_unit_Health;             // 유닛 체력
    public eUnit_Attack_Property_States m_eUnit_Attack_Property;    // 유닛 공격속성
    public float unit_Attack_Damage;    // 유닛 공격 데미지
    public eUnit_Defense_Property_States m_eUnit_Defense_Property; // 유닛 방어속성
    public string m_unit_Description;     // 유닛 설명
    public string m_unit_Type;            // 유닛 타입
    public float m_unit_MoveSpeed;        // 유닛 이동속도
    public float m_unit_Outlook;          // 유닛 시야
    public float m_unit_Attack_Range;     // 유닛 공격 범위

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
    public unit_Data m_unitData; // 유닛 데이터 구조체 변수

    public eUnit_Action_States m_enum_Unit_Action_Type;               // 유닛 행동 상태 변수
    public ArmorCalculate m_this_Unit_Armor_Property;



    public abstract void InitUnitInfoSetting();     // 유닛 정보 초기화 시켜주는 함수

    public virtual void Act_By_Unit()  // 유닛 행동 구분지어주는 함수
    {
        switch (m_enum_Unit_Action_Type)  // 유닛 행동 구분
        {
            case eUnit_Action_States.unit_Idle: // 유닛 대기 상태

                break;

            case eUnit_Action_States.unit_Move: // 유닛 이동

                break;

            case eUnit_Action_States.unit_Tracking: // 유닛이 몬스터 추적

                break;
            case eUnit_Action_States.unit_Attack:   // 유닛이 몬스터 공격

                break;

            case eUnit_Action_States.unit_Boundary: // 유닛 홀드(제자리 경계)

                break;

            default:
                print("case 예외 됐음");
                break;
        }
    }
    public void BeAttacked_By_OtherUnit(Collider other) // 기본공격 일 때와 스킬 공격 일 때 를 나눠야 함...
    {
        print("충돌했음");

        switch (m_unitData.m_eUnit_Defense_Property)
        {
            case eUnit_Defense_Property_States.Default:
                break;

            case eUnit_Defense_Property_States.plate_Armor:
                break;

            case eUnit_Defense_Property_States.gambeson_Armor:  // 몬스터 방어 타입이 천 갑옷 일 때
                unit_Data otherUnitData = other.GetComponent<UnitInfo>().m_unitData;
                print(m_this_Unit_Armor_Property.DecreaseDamaged(m_unitData, otherUnitData));
                m_unitData.m_unit_Health -= m_this_Unit_Armor_Property.DecreaseDamaged(m_unitData, otherUnitData);
                print(gameObject.name + "의 현재 체력" + m_unitData.m_unit_Health);
                break;

            case eUnit_Defense_Property_States.mail_Armor:
                break;

            default:
                break;
        }

    }

    // 구조체 필드 이니셜 라이징은 C# 9.0 에서 지원을 하지 않기 때문에 클래스를 따로 만듦

    public LayerMask m_layerMask = 0;   // 오버랩스피어 레이어 마스크 변수

    public Transform m_targetUnit = null;   // 유닛이 타겟으로 할 대상


    public void Search_For_Near_Enemy() // 가장 가까운 적 탐지하는 함수
    {
        Collider[] _cols = Physics.OverlapSphere(transform.position, m_unitData.m_unit_Attack_Range, m_layerMask); // 오버랩 스피어 생성

        Transform _shortestTarget = null;  // 가장 가까운 적을 의미하는 변수

        if (_cols.Length < 0)  // 탐지된 적이 없다면 함수 탈출
        {
            return;
        }

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

        m_targetUnit = _shortestTarget;
    }

    public void Look_At_The_Target()    // 유닛이 타겟을 감지 했을 때 타겟 쪽으로 몸을 회전하여 타겟을 바라보는 함수
    {
        Quaternion _lookRotation = Quaternion.LookRotation(m_targetUnit.position);  // 타겟 쪽으로 바라보는 각도

        Vector3 _euler = Quaternion.RotateTowards(transform.rotation, _lookRotation, 3f * Time.deltaTime).eulerAngles;

        transform.rotation = Quaternion.Euler(0, _euler.y, 0);

        Quaternion _fireRotation = Quaternion.Euler(0, _lookRotation.eulerAngles.y, 0); // 유닛이 발사할 수 있는 방향의 각도


        if (Quaternion.Angle(transform.rotation, _fireRotation) < 5f)
        {
            // 1. 공격 속도 쿨타임 감소

            // 2. 공격속도 쿨타임이 0보다 작아졌다면 발사 후 공격속도 변수에 초기값 다시 넣어줌

            Debug.Log("공격!!!");
        }
    }
}
