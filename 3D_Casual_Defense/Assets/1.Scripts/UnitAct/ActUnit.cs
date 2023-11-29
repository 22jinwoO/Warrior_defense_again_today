/*
    ===========================================================================
   ㅣ           플레이어 유닛이 몬스터를 공격하는 것을 구현한 스크립트            ㅣ
    ===========================================================================
 
 */



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActUnit : MonoBehaviour
{
    private NavMeshAgent nav;
    private UnitInfo unitInfoCs;
    private Animator anim;
    private UnitTargetSearch unitSearchCs;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        unitInfoCs = GetComponent<UnitInfo>();
        anim = GetComponent<Animator>();
        unitSearchCs = GetComponent<UnitTargetSearch>();
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
        unit_Data otherUnitData = other.GetComponent<UnitInfo>()._unitData;
        print(otherUnitData._eUnit_Defense_Property);
        unitInfoCs._unitData._unit_Health -= unitInfoCs._this_Unit_Armor_Property.CalculateDamaged(unitInfoCs._unitData, otherUnitData, attack_Dmg);
        //print(otherUnitData.);
        print(unitInfoCs._this_Unit_Armor_Property);
        print(attack_Dmg);
        print(unitInfoCs._unitData._unit_Health);


        // CalculateDamaged 함수에서 이미 방어구 타입별로 구분해줬기 때문에 여기서 스위치문을 사용하지 않아도 됨.
        //switch (unitInfoCs._unitdata._eunit_defense_property)
        //{
        //    case eunit_defense_property_states.default:
        //        break;

        //    case eunit_defense_property_states.plate_armor:

        //        break;

        //    case eunit_defense_property_states.gambeson_armor:  // 몬스터 방어 타입이 천 갑옷 일 때
        //        //print(gameobject.name);
        //        //print(other.name);
        //        unit_data otherunitdata = other.getcomponent<unitinfo>()._unitdata;
        //        //print(_this_unit_armor_property.calculatedamaged(_unitdata, otherunitdata, attack_dmg));
        //        unitinfocs._unitdata._unit_health -= unitinfocs._this_unit_armor_property.calculatedamaged(unitinfocs._unitdata, otherunitdata, attack_dmg);
        //        //print(gameobject.name + "의 현재 체력" + _unitdata._unit_health);
        //        break;

        //    case eunit_defense_property_states.mail_armor:
        //        unit_data mail_armor_unitdata = other.getcomponent<unitinfo>()._unitdata;
        //        unitinfocs._unitdata._unit_health -= unitinfocs._this_unit_armor_property.calculatedamaged(unitinfocs._unitdata, mail_armor_unitdata, attack_dmg);
        //        break;

        //    default:
        //        break;
        //}

    }
    #endregion
}
