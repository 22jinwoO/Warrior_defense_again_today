//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
//      | 유닛 정보를 하나의 클래스로 만들어서 한번에 관리|
//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
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


public abstract class UnitInfo : MonoBehaviour
{
    [Header("유닛 데이터 구조체 변수")]
    public unit_Data _unitData; // 유닛 데이터 구조체 변수

    [Header("유닛 행동 상태 변수")]
    public eUnit_Action_States _enum_Unit_Action_Type = eUnit_Action_States.unit_Idle;   // 유닛 행동 상태 변수

    [Header("플레이어가 지정해준 현재 유닛 공격 상태 변수")]
    public eUnit_Action_States _enum_Unit_Attack_Type;   // 플레이어가 지정해준 현재 유닛 공격 상태 변수

    [Header("유닛 방어구 속성")]
    public ArmorCalculate _this_Unit_Armor_Property;


    [Header("내비메쉬 에이전트")]
    public NavMeshAgent nav;    // 내비메쉬

    [Header("애니메이터")]
    public Animator anim;       // 애니메이터

    [Header("적을 탐지했는지 확인하는 변수")]
    public bool isSearch = false;   // 적을 탐지했는지 확인하는 변수

    [Header("유닛이 도착할 위치를 의미하는 벡터변수")]
    public Vector3 movePos;

    [Header("기본 공격 가능 불가능 확인하는 변수")]
    public bool _can_Base_Attack;    // 기본 공격 가능 불가능 확인하는 변수

    [Header("스킬 공격 가능 불가능 확인하는 변수")]
    public bool _can_Skill_Attack;   // 스킬 공격 가능 불가능 확인하는 변수

}
