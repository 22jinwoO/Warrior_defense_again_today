using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillDataManager;

public class NewSniperArrow : AbsNewSKill, IGenSkill
{

    public Transform m_startTr;

    public SkillDataManager skilldata;
    private void Awake()
    {
        unitInfoCs = GetComponent<NewUnitInfo>();
        _projectile_Prefab = Instantiate(_projectile_Prefab);
        _projectile_Prefab.SetActive(false);
        _projectile_Prefab.GetComponent<Abs_Bullet>().newUnitInfoCs = unitInfoCs;
        _projectile_Prefab.GetComponent<Abs_Bullet>()._start_Pos= m_startTr;

        unitTargetSearchCs = GetComponent<AbsSearchTarget>();
        skilldata = GameObject.FindObjectOfType<SkillDataManager>();
        Set_Init_Skill(skilldata.all_Skill_Datas.SkillDatas[1]);
    }
    public void Attack_Skill()
    {
        _projectile_Prefab.GetComponent<Abs_Bullet>()._newSkill = this;


        print(_projectile_Prefab.GetComponent<Abs_Bullet>()._target_Unit);
        print(_projectile_Prefab.GetComponent<Abs_Bullet>()._target_BodyTr);

        _projectile_Prefab.GetComponent<Abs_Bullet>()._target_Unit = unitTargetSearchCs._targetUnit;
        _projectile_Prefab.GetComponent<Abs_Bullet>()._target_BodyTr = unitTargetSearchCs._target_Body;

        if (_projectile_Prefab.GetComponent<Abs_Bullet>()._target_Unit != null)
        {
            //_projectile_Prefab.SetActive(false);

            Vector3 _target_Direction = unitTargetSearchCs._target_Body.position - unitInfoCs.transform.position;   

            Quaternion rot = Quaternion.LookRotation(_target_Direction.normalized);

            //transform.rotation = rot;
            print(unitInfoCs);
            print(unitInfoCs._projectile_startPos.position);

            print(_projectile_Prefab.GetComponent<Abs_Bullet>()._target_BodyTr);
            GameObject test = Instantiate(_projectile_Prefab, unitInfoCs._projectile_startPos.position, rot);
            //test.GetComponent<Bullet_Arrow>().unitInfoCs = unitInfoCs;
            //test.transform.SetParent(unitInfoCs.transform);
            test.SetActive(true);
            print("공격 실행");
            //unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;
            //unitInfoCs._unitData._unit_Attack_CoolTime = 0f;

        }
    }
}
