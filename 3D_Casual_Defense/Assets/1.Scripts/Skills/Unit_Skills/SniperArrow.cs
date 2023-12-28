using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperArrow : GeneralSkill
{
    public override void Attack_Skill()
    {
        _projectile_Prefab.GetComponent<Abs_Bullet>()._target_Unit = unitTargetSearchCs._targetUnit;
        _projectile_Prefab.GetComponent<Abs_Bullet>()._target_BodyTr = unitTargetSearchCs._target_Body;
        _projectile_Prefab.GetComponent<Abs_Bullet>()._start_Pos = unitInfoCs._projectile_startPos;

        Instantiate(_projectile_Prefab);
        print("공격 실행");
        //unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;
        //unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
    }
}
