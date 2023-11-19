using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonsterUnitClass
{
    [SerializeField]
    Text hpText;

    public override void Act_By_Unit()
    {
        throw new System.NotImplementedException();
    }



    // Start is called before the first frame update
    void Awake()
    {
        _this_Unit_Armor_Property = new MailArmor();
        InitUnitInfoSetting();
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text="몬스터 체력 : "+ Mathf.CeilToInt(_unitData._unit_Health).ToString();
        //Act_By_Unit();
    }

    public override void InitUnitInfoSetting()
    {

        _unitData._unit_Name = "몬스터";            // 유닛 이름
        _unitData._unit_Health = 200f;             // 유닛 체력
        _unitData._eUnit_Attack_Property = eUnit_Attack_Property_States.slash_Attack;    // 유닛 공격속성
        _unitData._unit_Attack_Damage = 1f;    // 유닛 공격 데미지
        _unitData._eUnit_Defense_Property = eUnit_Defense_Property_States.mail_Armor;   // 유닛 방어속성
        _unitData._unit_Description = "몬스터입니다";     // 유닛 설명
        _unitData._unit_Type = "몬스터";            // 유닛 타입
        _unitData._unit_MoveSpeed = 1f;        // 유닛 이동속도
        _unitData._unit_Outlook = 1f;          // 유닛 시야
        _unitData._unit_Attack_Range = 1f;     // 유닛 공격 범위
        _unitData._unit_Attack_Speed = 3f;        // 유닛 공격 속도
        _unitData._unit_Attack_CoolTime = 3f;     // 유닛 기본 공격 쿨타임
        _unitData._unit_Skill_CoolTime = 8f;     // 유닛 스킬 공격 쿨타임
    }




}
