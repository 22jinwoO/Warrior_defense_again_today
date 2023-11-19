using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public abstract class PlayerUnitClass : UnitInfo
{

    [SerializeField]
    protected UnitTargetSearch unitTargetSearchCs;

    public abstract void InitUnitInfoSetting();     // 유닛 정보 초기화 시켜주는 함수

    public abstract void Unit_Attack_Skill_CoolTime();     // 유닛 정보 초기화 시켜주는 함수


}

