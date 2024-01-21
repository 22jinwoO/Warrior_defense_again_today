using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

//[RequireComponent(typeof(Animator))]
public class Warrior : UnitInfo
{
    private void Awake()
    {
        _anim=GetComponent<Animator>();
        _this_Unit_ArmorCalculateCs = new PaddingArmor();
        _nav = GetComponent<NavMeshAgent>();
        //InitUnitInfoSetting();

    }

    private void Update()
    {

        // 기본 공격이 가능한지 확인
        _can_genSkill_Attack = _unitData._unit_Attack_CoolTime >= _unitData._unit_Attack_Speed ? true : false;

        // 스킬 공격이 가능한지 확인
        _can_SpcSkill_Attack =  _unitData._unit_Current_Skill_CoolTime >= _unitData._unit_Skill_CoolTime ? true : false;

        //현재 스킬 공격 쿨타임이 유닛의 스킬 공격 쿨타임 보다 낮다면 쿨타임 돌려주기
        if (_unitData._unit_Skill_CoolTime>= _unitData._unit_Current_Skill_CoolTime)
        {
            _unitData._unit_Current_Skill_CoolTime += Time.deltaTime;
        }

        //현재 기본 공격 쿨타임이 유닛의 기본 공격속도 보다 낮다면 쿨타임 돌려주기
        if (_unitData._unit_Attack_Speed>=_unitData._unit_Attack_CoolTime)
        {
            _unitData._unit_Attack_CoolTime += Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray,out RaycastHit hit))
            {
                _movePos = hit.point;
                _enum_Unit_Action_State = eUnit_Action_States.unit_Move;
            }
        }
        //Act_By_Unit();
    }


//    public override void InitUnitInfoSetting()
//    {

//        _unitData._unit_Name = "용사";            // 유닛 이름
//        _unitData._unit_maxHealth = 200f;             // 유닛 체력
//        _unitData._eUnit_genSkill_Property = eUnit_Attack_Property_States.slash_Attack;    // 유닛 공격속성
//        _unitData._unit_General_Skill_Dmg = 1f;    // 유닛 공격 데미지
//        _unitData._unit_Special_Skill_Dmg = 6f;    // 유닛 공격 데미지
//        _unitData._eUnit_Defense_Property = eUnit_Defense_Property_States.padding_Armor;   // 유닛 방어속성
//        _unitData._unit_Description = "용사입니다";     // 유닛 설명
//        _unitData._unit_Type = "용사";            // 유닛 타입
//        _unitData._unit_MoveSpeed = 1f;        // 유닛 이동속도
//        _unitData.sightRange = 8f;          // 유닛 시야
//        _unitData.attackRange = 4f;     // 유닛 공격 범위
//        _unitData._unit_Attack_Speed=3f;        // 유닛 공격 속도
//        _unitData._unit_Attack_CoolTime = 0f;     // 유닛 기본 공격 쿨타임
//        _unitData._unit_Skill_CoolTime = 8f;     // 유닛 스킬 공격 쿨타임
//}


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _unitData.sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _unitData.attackRange);
    }
    private void OnTriggerEnter(Collider other)
    {

        // 타겟 값이 잇으니까 타겟 이름 비교하는거
        // 얘가 가면서 충돌인식을 다 해 근데 충돌한 애들을 리스트에 넣어줘. 유닛인포 타입으로 list.where //


        //if (other.CompareTag("Orc"))
        //{
        //    if (other.name==_targetUnit.name)
        //    {
        //        //dd 유닛 인포 타입이잔ㅇ하. 
        //        other.GetComponent<UnitInfo>().BeAttacked_By_OtherUnit(transform);
        //    }


        //}
    }
}

