//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
//      | ArmorCalculate 스크립트를 상속 받은 판금 갑옷 스크립트 (아머타입이 판금 갑옷 타입일 때 사용)   |
//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateArmor : ArmorCalculate
{
    public override float CalculateDamaged(eUnit_Attack_Property_States attackType, new_Unit_Data ArmorType, float attackDmg)  // 공격당했을 때 호출하는걸 전제로 함
    {
        if (ArmorType._eUnit_Defense_Property != eUnit_Defense_Property_States.plate_Armor) // 방어 타입이 판금 갑옷이 아니면
        {
            Debug.LogWarning(ArmorType._eUnit_Defense_Property);
            return 0f;
        }
        float attackDamge = 0;

        switch (attackType)  // 공격 타입 구분
        {
            case eUnit_Attack_Property_States.slash_Attack: // 베기 공격 일 때
                attackDamge = attackDmg * 0.6f;
                break;

            case eUnit_Attack_Property_States.piercing_Attack: // 관통공격 일 때
                attackDamge = attackDmg * 1.5f;
                break;

            case eUnit_Attack_Property_States.crushing_attack: // 분쇄공격 일 때
                attackDamge = attackDmg * 1.0f;
                break;

            default:
                break;
        }

        return attackDamge;


    }
}
