using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperArrow : GeneralSkill
{
    public override void Attack_Skill()
    {
        _projectile_Prefab.GetComponent<Abs_Bullet>()._target_Unit = unitTargetSearchCs._targetUnit;
        _projectile_Prefab.GetComponent<Abs_Bullet>()._target_BodyTr = unitTargetSearchCs._target_Body;

        if (_projectile_Prefab.GetComponent<Abs_Bullet>()._target_Unit!=null)
        {
            //_projectile_Prefab.SetActive(false);

            Vector3 _target_Direction = unitTargetSearchCs._target_Body.position - unitInfoCs.transform.position;

            Quaternion rot = Quaternion.LookRotation(_target_Direction.normalized);

            //transform.rotation = rot;
            print(unitInfoCs);
            print(unitInfoCs._projectile_startPos.position);
            GameObject test = Instantiate(_projectile_Prefab, unitInfoCs._projectile_startPos.position, rot);
            test.GetComponent<Bullet_Arrow>().unitInfoCs = unitInfoCs;
            test.transform.SetParent(unitInfoCs.transform);
            test.SetActive(true);
            print("공격 실행");
            unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;
            unitInfoCs._unitData._unit_Attack_CoolTime = 0f;

        }
    }
}
