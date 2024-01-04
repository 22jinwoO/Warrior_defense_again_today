using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class StunStatus : StatusEffect
{


    public override IEnumerator Apply_Status_Effect(UnitInfo thisUnit, string linkId, int statusValue, float duration)
    {
        yield return null;
        isStatusApply = false;
        float times = 0f;


        float defaultSpd = thisUnit._unitData.moveSpeed;
        float defaultAcc = thisUnit._unitData.moveAcc;

        thisUnit._nav.speed = 0f;
        thisUnit._nav.acceleration = 0f;

        while (times <= duration)
        {
            if (isStatusApply)
            {
                break;
            }
            times += Time.deltaTime;
            Debug.LogWarning(times);

            yield return null;
        }


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

