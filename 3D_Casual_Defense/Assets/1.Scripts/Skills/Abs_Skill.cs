using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public eUnit_Attack_Property_States _skill_AtkType;
    public int _base_Value;
    public float _critical_Dmg;
    public string _link_Id;

    public GameObject _projectile_Prefab;

    public UnitInfo unitInfoCs;

    public UnitTargetSearch unitTargetSearchCs;

    public bool isRush;

    public abstract void Attack_Skill();

}
