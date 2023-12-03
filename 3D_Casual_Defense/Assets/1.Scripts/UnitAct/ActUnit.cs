/*
    ===========================================================================
   ㅣ           플레이어 유닛이 몬스터를 공격하는 것을 구현한 스크립트            ㅣ
    ===========================================================================
 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//
public class ActUnit : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent nav;
    
    [SerializeField]
    private UnitInfo unitInfoCs;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private UnitTargetSearch unitSearchCs;

    private void Awake()
    {
        nav = transform.root.GetComponent<NavMeshAgent>();
        unitInfoCs = transform.root.GetComponent<UnitInfo>();
        anim = transform.root.GetComponent<Animator>();
        unitSearchCs = transform.root.GetComponent<UnitTargetSearch>();
    }


    #region # Attack_Unit() : 유닛이 공격할 때 호출되는 함수
    public void Attack_Unit()
    {
        if (nav.enabled==true)
        {
            nav.isStopped = true;
        }
        if (unitInfoCs._can_Base_Attack)
        {
            anim.SetTrigger("isAttack");
            if (unitInfoCs._can_Skill_Attack)
            {
                Debug.Log("스킬공격!!!");
                if (unitSearchCs._targetUnit == null)
                {
                    return;
                }
                print("48"+ unitSearchCs._targetUnit);
                print("49"+ unitInfoCs._unitData._unit_Skill_Attack_Damage);
                print("50"+ unitSearchCs._targetUnit.GetComponent<ActUnit>());  // 오류 x
                print(unitSearchCs._targetUnit.root.GetComponent<UnitInfo>()._unitData);
                //print("51"+ unitSearchCs._targetUnit);
                unitSearchCs._targetUnit.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(unitSearchCs._targetUnit, unitInfoCs._unitData._unit_Skill_Attack_Damage);
                unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;
                unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
                unitInfoCs._enum_Unit_Action_Type = unitInfoCs._enum_Unit_Attack_Type;
                return;
            }

            else
            {
                Debug.Log("기본공격!!!");
                if (unitSearchCs._targetUnit == null)
                {
                    return;
                }
                unitSearchCs._targetUnit.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(unitSearchCs._targetUnit, unitInfoCs._unitData._unit_Attack_Damage);
                unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
                unitInfoCs._enum_Unit_Action_Type = unitInfoCs._enum_Unit_Attack_Type;
            }

        }
    }
    #endregion

    #region # BeAttacked_By_OtherUnit(Transform other,float attack_Dmg) : 다른 유닛으로부터의 공격으로 피해를 입을 때 호출되는 함수
    public void BeAttacked_By_OtherUnit(Transform other, float attack_Dmg) // 기본공격 일 때와 스킬 공격 일 때 를 나눠야 함...
    {
        print("충돌했음");
        print(other.gameObject.name);
        print(other.root.GetComponent<UnitInfo>()._unitData);
        unit_Data otherUnitData = other.root.GetComponent<UnitInfo>()._unitData;
        print(otherUnitData._eUnit_Defense_Property);
        print(unitInfoCs._unitData._unit_Health);
        unitInfoCs._unitData._unit_Health -= unitInfoCs._this_Unit_Armor_Property.CalculateDamaged(unitInfoCs._unitData, otherUnitData, attack_Dmg);
        //print(otherUnitData.);
        print(unitInfoCs._this_Unit_Armor_Property);
        print(attack_Dmg);
        print(unitInfoCs._unitData._unit_Health);

    }
    #endregion
}
