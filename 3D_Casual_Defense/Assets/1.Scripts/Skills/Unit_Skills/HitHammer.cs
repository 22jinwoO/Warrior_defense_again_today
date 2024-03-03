using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHammer : GeneralSkill
{
    public override void Attack_Skill()
    {
        if (unitTargetSearchCs._targetUnit != null)
        {
            unitTargetSearchCs._targetUnit.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(this, _skill_AtkType, ref unitInfoCs, unitTargetSearchCs._targetUnit);
        }
    }

}
