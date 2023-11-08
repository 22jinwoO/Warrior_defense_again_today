using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : UnitInfo
{
    private void Awake()
    {
        m_this_Unit_Armor_Property = new GambesonArmor();

        InitUnitInfoSetting();

    }

    private void Update()
    {

    }

    public override void InitUnitInfoSetting()
    {

        m_unitData.m_unit_Name = "용사";            // 유닛 이름
        m_unitData.m_unit_Health = 200f;             // 유닛 체력
        m_unitData.m_eUnit_Attack_Property = eUnit_Attack_Property_States.slash_Attack;    // 유닛 공격속성
        m_unitData.unit_Attack_Damage = 1f;    // 유닛 공격 데미지
        m_unitData.m_eUnit_Defense_Property = eUnit_Defense_Property_States.gambeson_Armor;   // 유닛 방어속성
        m_unitData.m_unit_Description = "용사입니다";     // 유닛 설명
        m_unitData.m_unit_Type = "용사";            // 유닛 타입
        m_unitData.m_unit_MoveSpeed = 1f;        // 유닛 이동속도
        m_unitData.m_unit_Outlook = 1f;          // 유닛 시야
        m_unitData.m_unit_Attack_Range = 1f;     // 유닛 공격 범위
    }




}

