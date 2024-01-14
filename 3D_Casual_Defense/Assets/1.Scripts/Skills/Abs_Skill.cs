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

    public void Set_Init_Skill(SkillDataManager.SkillData sad)
    {
        skill_Id = sad.skill_id;
        skill_Name = sad.skill_name;
        skill_Description = sad.skill_script;

        // 시전 / 사용 조건
        _skill_CoolTm = sad.coolTm;
        _target_Type = sad.targetType;      //enum 으로 만들기
        _use_Range = sad.use_Range;       //enum 으로 만들기
        _casting_Type = sad.castingType;    //enum 으로 만들기

        // 적중 판정
        _trace_Type = sad.traceType;  //enum 으로 만들기
        _area_Shape = sad.areaShape;  //enum 으로 만들기
        _area_Length = sad.areaLength;
        _area_Width = sad.areaWidth;

        // 적용 효과
        _damgeType = sad.damageType;
        _base_Value = sad.baseValue;
        _critical_Dmg = sad.criticDamage;
        _link_Id = sad.link_id;
    }
}

