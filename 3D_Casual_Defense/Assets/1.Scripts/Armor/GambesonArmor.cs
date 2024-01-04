//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
//      | ArmorCalculate 스크립트를 상속 받은 천갑옷 스크립트 (아머타입이 천갑옷 타입일 때 사용) |
//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambesonArmor : ArmorCalculate
{
    
    public override float CalculateDamaged(eUnit_Attack_Property_States attackType, unit_Data ArmorType, float attack_Dmg)  // 공격당했을 때 호출하는걸 전제로 함 
    {
        if (ArmorType._eUnit_Defense_Property != eUnit_Defense_Property_States.padding_Armor) // 방어 타입이 천갑옷이 아니면 일 때
        {
            return 0f;
        }

        switch (attackType)  // 공격 타입 구분
        {
            case eUnit_Attack_Property_States.slash_Attack: // 베기 공격 일 때
                attack_Dmg *= 1.5f;
                break;

            case eUnit_Attack_Property_States.piercing_Attack: // 관통공격 일 때
                attack_Dmg *= 1.0f;
                break;

            case eUnit_Attack_Property_States.crushing_attack: // 분쇄공격 일 때
                attack_Dmg *= 0.6f;
                break;

            default:

                break;
        }

        return attack_Dmg;


    }
}
