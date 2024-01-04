using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlowStatus : StatusEffect
{
    public override IEnumerator Apply_Status_Effect(UnitInfo thisUnit, string linkId, int statusValue, float duration)
    {
        yield return null;
        isStatusApply = false;
        float times = 0f;


        float defaultSpd = thisUnit._unitData.moveSpeed;
        float defaultAcc = thisUnit._unitData.moveAcc;


        thisUnit._nav.speed = defaultSpd/2;
        thisUnit._nav.acceleration = defaultAcc/2;

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

        //float defaultSpd = thisUnit._nav.speed;
        //float defaultAcc = thisUnit._nav.acceleration;

        //int times = 0;

        //thisUnit._nav.speed = defaultSpd/2f;
        //thisUnit._nav.acceleration = defaultAcc / 2f;

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

        //thisUnit._nav.speed = defaultSpd;
        //thisUnit._nav.acceleration = defaultAcc;
    }

}
