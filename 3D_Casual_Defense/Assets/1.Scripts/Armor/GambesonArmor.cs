//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
//      | ArmorCalculate 스크립트를 상속 받은 천갑옷 스크립트 (아머타입이 천갑옷 타입일 때 사용) |
//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambesonArmor : ArmorCalculate
{
    public override float DecreaseDamaged(unit_Data attackType, unit_Data ArmorType)  // 공격당했을 때 호출하는걸 전제로 함
    {
        if (ArmorType.m_eUnit_Defense_Property != eUnit_Defense_Property_States.gambeson_Armor) // 방어 타입이 천갑옷이 아니면 일 때
        {
            return 0f;
        }
        float attackDamge = 0;

        switch (attackType.m_eUnit_Attack_Property)  // 공격 타입 구분
        {
            case eUnit_Attack_Property_States.slash_Attack: // 베기 공격 일 때
                attackDamge = attackType.unit_Attack_Damage * 1.5f;
                break;

            case eUnit_Attack_Property_States.piercing_Attack: // 관통공격 일 때
                attackDamge = attackType.unit_Attack_Damage * 1.0f;
                break;

            case eUnit_Attack_Property_States.crushing_attack: // 분쇄공격 일 때
                attackDamge = attackType.unit_Attack_Damage * 0.6f;
                break;

            default:
                break;
        }

        return attackDamge;


    }
}
