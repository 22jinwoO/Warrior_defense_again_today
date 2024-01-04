using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut : GeneralSkill
{

    public override void Attack_Skill()
    {
        print("애니메이션 호출 함수");
        unitTargetSearchCs._targetUnit.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(this,_skill_AtkType, unitTargetSearchCs._targetUnit, _base_Value);
        //unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
    }
}
