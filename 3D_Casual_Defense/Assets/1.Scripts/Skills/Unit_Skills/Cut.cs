using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut : GeneralSkill
{

    public override void Attack_Skill()
    {
        print(_skill_AtkType);
        print(unitInfoCs);
        print(unitTargetSearchCs._targetUnit);
        print("애니메이션 호출 함수");
        if (unitTargetSearchCs._targetUnit!=null)
        {
            unitTargetSearchCs._targetUnit.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(this, _skill_AtkType, ref unitInfoCs, unitTargetSearchCs._targetUnit);
        }
        //unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
    }
}
