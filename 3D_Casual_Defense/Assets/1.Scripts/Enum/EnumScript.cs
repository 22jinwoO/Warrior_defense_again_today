using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eUnit_Attack_Property_States  // 유닛 공격 타입
{
    Default = 0,
    slash_Attack,       // 베기 공격
    piercing_Attack,    // 관통 공격
    crushing_attack,     // 분쇄 공격
    Length
}

public enum eUnit_Defense_Property_States // 유닛 방어 타입
{
    Default = 0,
    plate_Armor,       // 판금 갑옷
    padding_Armor,    // 천갑옷
    chain_Armor,        // 쇠사슬 갑옷
    Length
}


public enum eUnit_targetSelectType // 유닛의 타겟 선정 타입
{
    Default = 0,
    fixed_Target,              // 고정 타겟
    nearest_Target,            // 가장 가까운 타겟
    low_Health_Target,         // 가장 낮은 체력을 가진 타겟
    Length
}


// 공격 킬타입에 따른 몬스터 탐지 함수 추상클래스로 생성 후 상속하여 킬타입에 해당하는 몬스터 탐지함수 실행

public enum eUnit_Action_States           // 유닛 행동
{
    Default = 0,
    unit_FreeMode,      // 유닛 자유 모드(추격)
    unit_HoldMode,      // 유닛 홀드 모드

    monster_NormalMode,   // 몬스터 일반 모드
    monster_AngryMode, // 몬스터 분노 모드
    monster_AttackCastleMode, // 몬스터 성 공격 모드

    unit_Idle,          // 대기
    unit_Move,          // 이동
    unit_AttackReady,   // 공격 준비
    unit_Tracking,      // 추적
    unit_Attack,        // 공격
    unit_Boundary,       // 홀드 후 주변 경계

    close_Range_Atk,    // 근거리
    long_Range_Atk,     // 원거리
    Length
}

