using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : GeneralSkill
{

    public override void Attack_Skill()
    {
        if (unitTargetSearchCs._targetUnit != null)
        {
            unitTargetSearchCs._targetUnit.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(this, _skill_AtkType, ref unitInfoCs, unitTargetSearchCs._targetUnit);
        }        //unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
    }
}
