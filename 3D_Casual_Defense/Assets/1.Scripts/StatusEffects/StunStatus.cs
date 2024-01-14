using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class StunStatus : Abs_StatusEffect
{


    public override IEnumerator Apply_Status_Effect(UnitInfo thisUnit, string linkId, int statusValue, float duration)
    {
        thisUnit._anim.ResetTrigger("isStun");

        yield return null;
        isStatusApply = false;
        float times = 0f;


        float defaultSpd = thisUnit._unitData.moveSpeed;
        float defaultAcc = thisUnit._unitData.moveAcc;
        thisUnit.canAct = false;
        thisUnit._anim.SetTrigger("isStun");
        thisUnit._nav.isStopped = true;
        thisUnit._nav.speed = 0f;
        thisUnit._nav.acceleration = 0f;
        thisUnit._nav.velocity = Vector3.zero;
        thisUnit._anim.SetBool("isMove", false);

        Debug.LogWarning("스턴시작 + ");
        while (times <= duration)
        {
            if (isStatusApply)
            {
                Debug.LogWarning("스턴 멈추고 재시작");

                break;
            }
            times += Time.deltaTime;
            //Debug.LogWarning("스턴중 + "+times);
            //Debug.LogWarning("스피드 + "+ thisUnit._nav.speed);
            //Debug.LogWarning("가속도 + "+ thisUnit._nav.acceleration);
            //Debug.LogWarning("velocity + "+ thisUnit._nav.velocity);

            yield return null;
        }
        Debug.LogWarning("스턴 끝 ");

        thisUnit.canAct = true;
        thisUnit._nav.isStopped = false;

        thisUnit._nav.speed = defaultSpd;
        thisUnit._nav.acceleration = defaultAcc;

        //yield return new WaitForSeconds(0.8f);
        //isStatusApply = false;

        //float defaultSpd = thisUnit._unitData.moveSpeed;
        //float defaultAcc = thisUnit._unitData.moveAcc;
        //Console.ReadLine();
        //int times = 0;

        //thisUnit._nav.speed = 0f;
        //thisUnit._nav.acceleration = 0f;

        //while (times < duration)
        //{
        //    if (isStatusApply)
        //    {
        //        yield return null;

        //        break;
        //    }

        //    times++;
        //    yield return new WaitForSeconds(1f);
        //}

    }

}

