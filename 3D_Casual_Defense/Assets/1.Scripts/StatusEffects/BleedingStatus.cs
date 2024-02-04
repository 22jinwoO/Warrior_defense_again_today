using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BleedingStatus : Abs_StatusEffect
{
    public override IEnumerator Apply_Status_Effect(UnitInfo thisUnit, string linkId, int statusValue, float duration)
    {
        yield return null;
        isStatusApply = false;

        for (int i = 0; i < (int)duration; i++)
        {
            thisUnit._status_Effect_Bleeding.SetActive(false);

            float times = 0f;

            while (times <= 1f)
            {
                if (isStatusApply)
                {
                    thisUnit._status_Effect_Bleeding.SetActive(false);

                    yield break;    // 리턴같은거
                }
                times += Time.deltaTime;

                yield return null;
            }
            thisUnit._status_Effect_Bleeding.SetActive(true);
            thisUnit._unitData.hp -= statusValue;
            Debug.LogWarning("타겟 유닛 피"+i+" "+thisUnit._unitData.hp);

        }


        //    if (isStatusApply.Equals(true))
        //    {
        //        yield return new WaitForSeconds(0.8f);
        //        isStatusApply = false;
        //    }

        //    int times = 0;

        //    while (times < duration)
        //    {
        //        if (isStatusApply)
        //        {
        //            yield return null;

        //            break;
        //        }
        //        thisUnit._unitData.hp -= statusValue;

        //        times++;
        //        yield return new WaitForSeconds(1f);
        //    }
        //}
    }
}

