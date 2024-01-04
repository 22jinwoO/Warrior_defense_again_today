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
    [Header("유닛 이름")]
    public string _unit_Name;                           // 유닛 이름
    public float _unit_maxHealth;                       // 유닛 최대 체력
    public float _unit_currentHealth;                   // 유닛 현재 체력 유닛 최대체력 넣어주기
    public int _unit_Level;                             // 유닛 레벨

    public int no; // 캐릭터 넘버
    public string char_id;   // 캐릭터 id
    public string unit_class; // 유닛 클래스
    public int level;    // 레벨
    public int hp;   // 체력
    public string defenseType;   // 방어 타입
    public int moveSpeed;   // 이동속도
    public int moveAcc;   // 이동 가속도

    //public int sightRange;   // 시야 범위
    //public int attackRange;   // 공격 범위

    public int criticRate;    // 크리티컬 확률
    public string generalSkill;   // 일반스킬
    public string generalSkillName;   // 일반스킬 이름
    public string specialSkill1;   // 특수 스킬 , 자유모드 일 때 사용하는 스킬
    public string specialSkill1Name;   // 특수 스킬 1 이름
    public string specialSkill2;   // 특수 스킬 , 홀드모드 일 때 사용하는 스킬
    public string specialSkill2Name;   // 특수 스킬 2 이름
    public string targetSelectType;   // 유닛 설정 타입

    public eUnit_Attack_Property_States _eUnit_genSkill_Property;    // 유닛 일반 스킬속성
    public float _unit_General_Skill_Dmg;                   // 유닛 일반 스킬 데미지
    public float _unit_Special_Skill_Dmg;                   // 유닛 특수 스킬 데미지
    public eUnit_Defense_Property_States _eUnit_Defense_Property; // 유닛 방어속성
    public string _unit_Description;                        // 유닛 설명
    public string _unit_Type;                               // 유닛 타입
    public float _unit_MoveSpeed;                           // 유닛 이동속도
    public float sightRange;                          // 유닛 시야
    public float attackRange;                        // 유닛 공격 범위
    public float _unit_Attack_Speed;                        // 유닛 공격 속도
    public float _unit_Attack_CoolTime;                     // 유닛 기본 공격 쿨타임
    public float _unit_Current_Skill_CoolTime;              // 유닛 현재 스킬 공격 쿨타임
    public float _unit_Skill_CoolTime;                      // 유닛 스킬 공격 쿨타임
    public int _unit_CriticalRate;                          // 유닛 크리티컬 확률
    public eUnit_targetSelectType _unit_targetSelectType;   // 타겟 선정 타입
    public string unit_Id;                                  // 유닛 Id
}

public abstract class UnitInfo : MonoBehaviour
{
    [Header("유닛 데이터 구조체 변수")]
    public unit_Data _unitData; // 유닛 데이터 구조체 변수

    [Header("유닛 행동 모드 상태 변수")]
    public eUnit_Action_States _enum_Unit_Action_Mode;   // 유닛 모드 상태 변수

    [Header("유닛 행동 상태 변수")]
    public eUnit_Action_States _enum_Unit_Action_State;   // 유닛 행동 상태 변수

    [Header("플레이어가 지정해준 현재 유닛 공격 상태 변수")]
    public eUnit_Action_States _enum_Unit_Attack_State;   // 플레이어가 지정해준 현재 유닛 공격 상태 변수

    [Header("유닛의 공격 상태 (근거리, 원거리)")]
    public eUnit_Action_States _enum_Unit_Attack_Type;   // 플레이어가 지정해준 현재 유닛 공격 상태 변수

    [Header("유닛의 타겟 선정 타입")]
    public eUnit_targetSelectType _eUnit_Target_Search_Type;   // 유닛의 타겟 선정 타입

    [Header("유닛 방어구 속성")]
    public ArmorCalculate _this_Unit_Armor_Property;


    [Header("유닛 바디 트랜스폼")]
    public Transform body_Tr;    // 유닛 바디 트랜스폼

    [Header("내비메쉬 에이전트")]
    public NavMeshAgent _nav;    // 내비메쉬

    [Header("애니메이터")]
    public Animator _anim;       // 애니메이터

    [Header("유닛을 클릭했는지 확인하는 변수")]
    public bool _isClick;       // 유닛을 클릭했는지 확인하는 변수


    [Header("적을 탐지했는지 확인하는 변수")]
    public bool _isSearch = false;   // 적을 탐지했는지 확인하는 변수

    [Header("유닛이 도착할 위치를 의미하는 벡터변수")]
    public Vector3 _movePos;

    [Header("기본 공격 가능 불가능 확인하는 변수")]
    public bool _can_Base_Attack;    // 기본 공격 가능 불가능 확인하는 변수

    [Header("스킬 공격 가능 불가능 확인하는 변수")]
    public bool _can_Skill_Attack;   // 스킬 공격 가능 불가능 확인하는 변수


    // 게임 이펙트 부분*********************************

    [Header("베기 공격 피격시 생성되는 이펙트")]
    public GameObject _hit_Effect_SlashAtk;   // 스킬 공격 가능 불가능 확인하는 변수

    [Header("관통 공격 피격 시 생성되는 이펙트")]
    public GameObject _hit_Effect_PierceAtk;   // 스킬 공격 가능 불가능 확인하는 변수

    [Header("분쇄 공격 피격 시 생성되는 이펙트")]
    public GameObject _hit_Effect_CrushAtk;   // 스킬 공격 가능 불가능 확인하는 변수

    // 게임 이펙트 부분=====================================

    // 발사체 프리팹 *************************
    [Header("일반스킬 사용할 때의 발사체 게임 오브젝트 프리팹")]
    public GameObject _projectile_Prefab;   // 스킬 공격 가능 불가능 확인하는 변수

    // 발사체 프리팹 =========================
    [Header("일반스킬 사용할 때의 발사체 게임 오브젝트 프리팹 생성 위치")]
    public Transform _projectile_startPos;   // 스킬 공격 가능 불가능 확인하는 변수

    // 스킬 1,2 ===============================

    [Header("일반 스킬")]
    public Abs_Skill gen_skill;

    [Header("특수 스킬 - 1")]
    public Abs_Skill spe_skill_1;

    [Header("특수 스킬 - 2")]
    public Abs_Skill spe_skill_2;


    // 사운드 *************************

    // 사운드 =========================

    // 히트 시 출력되는 텍스트 *************************

    // 히트 시 출력되는 텍스트 =========================
    #region # Unit_Attack_Skill_CoolTime() : 유닛 기본공격, 스킬공격 쿨타임 돌려주는 함수
    public void Unit_Attack_Skill_CoolTime()
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



}

