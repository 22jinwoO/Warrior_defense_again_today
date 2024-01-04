using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BurningStatus : StatusEffect
{
    public override IEnumerator Apply_Status_Effect(UnitInfo thisUnit, string linkId, int statusValue, float duration)
    {
        yield return null;
        isStatusApply = false;

        for (int i = 0; i < (int)duration; i++)
        {
            float times = 0f;

            while (times <= 1f)
            {
                if (isStatusApply)
                {
                    yield break;    // 리턴같은거
                }
                times += Time.deltaTime;

                yield return null;
            }

            thisUnit._unitData.hp -= statusValue;
            Debug.LogWarning("타겟 유닛 피" + i + " " + thisUnit._unitData.hp);

        }
    }
}

