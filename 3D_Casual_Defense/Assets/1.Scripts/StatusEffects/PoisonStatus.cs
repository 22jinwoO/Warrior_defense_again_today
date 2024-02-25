using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoisonStatus : Abs_StatusEffect
{
    public override IEnumerator Apply_Status_Effect(UnitInfo thisUnit, string linkId, int statusValue, float duration)
    {
        yield return null;
        isStatusApply = false;

        thisUnit._status_Effect_Poison.SetActive(false);

        yield return null;

        thisUnit._status_Effect_Poison.SetActive(true);

        for (int i = 0; i < (int)duration; i++)
        {
            float times = 0f;

            while (times <= 1f)
            {
                if (isStatusApply)
                {
                    thisUnit._status_Effect_Poison.SetActive(false);

                    yield break;    // 리턴같은거
                }
                times += Time.deltaTime;

                yield return null;
            }

            thisUnit._unitData.hp -= statusValue;

            if (thisUnit._unitData.hp<=0f&&thisUnit.canAct)
            {
                thisUnit.actUnitCs.DeadCheck();
                yield break;    // 리턴같은거
            }

            Debug.LogWarning("타겟 유닛 피" + i + " " + thisUnit._unitData.hp);        }
    }
}
