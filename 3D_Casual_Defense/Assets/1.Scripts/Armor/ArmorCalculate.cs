//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
//      | 방어타입마다 공격력 계산을 실행시켜주는 추상 스크립트 (천갑옷, 판금갑옷, 쇠사슬 갑옷 스크립트가 상속)|
//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArmorCalculate
{
    // 방어구 데미지 값 계산하는 함수
    public abstract float CalculateDamaged(eUnit_Attack_Property_States attackType, unit_Data ArmorType, float attack_Dmg);
}
