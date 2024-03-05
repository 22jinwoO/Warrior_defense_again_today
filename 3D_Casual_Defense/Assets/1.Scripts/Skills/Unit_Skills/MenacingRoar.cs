using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenacingRoar : SpecialSkill
{
    private void Start()
    {
        if (unitInfoCs!=null)
        {
            skillVfx = Instantiate(skillVfx);
            skillVfx.SetActive(false);
            skillVfx.transform.SetParent(unitInfoCs.transform);
            skillVfx.transform.position=unitInfoCs.transform.position;
        }


    }
    public override void Attack_Skill()
    {

        StartCoroutine(VfxOnOFF());
        Collider[] colls = Physics.OverlapSphere(unitInfoCs.transform.position, 10f, unitTargetSearchCs._layerMask);
        print("B눌림");
        for (int i = 0; i < colls.Length; i++)
        {
            UnitInfo unitInfo = colls[i].GetComponent<UnitInfo>();
            print(colls[i].name);
            unitInfo.unitTargetSearchCs._targetUnit = unitInfoCs.transform;
            unitInfo.unitTargetSearchCs._target_Body = unitInfoCs.body_Tr;
            //unitInfo.unitTargetSearchCs._target_Body = this.transform;

            unitInfo._enum_Unit_Action_Mode = eUnit_Action_States.monster_AngryPhase;
            unitInfo._enum_Unit_Action_State = eUnit_Action_States.unit_Attack;
            //unitInfo.unitTargetSearchCs._target_Body = 

        }

    }
    IEnumerator VfxOnOFF()
    {
        skillVfx.SetActive(false);
        yield return new WaitForSecondsRealtime(0.01f);

        skillVfx.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        skillVfx.SetActive(false);

    }
}
