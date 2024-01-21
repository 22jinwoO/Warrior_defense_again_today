using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;


public abstract class Abs_Skill : MonoBehaviour
{
    public string skill_Id;
    public string skill_Name;
    public string skill_Description;

    // 시전 / 사용 조건
    public int _skill_CoolTm;
    public string _target_Type;      //enum 으로 만들기
    public string _use_Range;       //enum 으로 만들기
    public string _casting_Type;    //enum 으로 만들기

    // 적중 판정
    public string _trace_Type;  //enum 으로 만들기
    public string _area_Shape;  //enum 으로 만들기
    public int _area_Length;
    public int _area_Width;

    // 적용 효과
    public string _damgeType;
    public eUnit_Attack_Property_States _skill_AtkType;
    public int _base_Value;
    public float _critical_Dmg;
    public string _link_Id;
    //

    public Abs_StatusEffect _link_Skill;


    public GameObject _projectile_Prefab;

    public UnitInfo unitInfoCs;

    public UnitTargetSearch unitTargetSearchCs;

    public bool isRush;

    public abstract void Attack_Skill();

    public void Set_Init_Skill(SkillDataManager.SkillData skillData)
    {
        skill_Id = skillData.skill_id;
        skill_Name = skillData.skill_name;
        skill_Description = skillData.skill_script;

        // 시전 / 사용 조건
        _skill_CoolTm = skillData.coolTm;
        _target_Type = skillData.targetType;      //enum 으로 만들기
        _use_Range = skillData.use_Range;       //enum 으로 만들기
        _casting_Type = skillData.castingType;    //enum 으로 만들기

        // 적중 판정
        _trace_Type = skillData.traceType;  //enum 으로 만들기
        _area_Shape = skillData.areaShape;  //enum 으로 만들기
        _area_Length = skillData.areaLength;
        _area_Width = skillData.areaWidth;

        // 적용 효과
        _damgeType = skillData.damageType;
        _base_Value = skillData.baseValue;
        _critical_Dmg = skillData.criticDamage;
        _link_Id = skillData.link_id;
    }
}

