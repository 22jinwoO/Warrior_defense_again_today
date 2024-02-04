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
        if (thisUnit._unitData.hp>0f&&thisUnit._nav.enabled == true)
        {
            thisUnit._anim.SetBool("isStun",true);
            thisUnit._status_Effect_Stun_Start.SetActive(false);
            thisUnit._status_Effect_Stun.SetActive(false);

            yield return null;
            isStatusApply = false;
            float times = 0f;


            float defaultSpd = thisUnit._unitData.moveSpeed;
            float defaultAcc = thisUnit._unitData.moveAcc;
            thisUnit.canAct = false;
            //thisUnit._anim.SetTrigger("isStun");
            //thisUnit._nav.isStopped = true; 
            thisUnit._nav.speed = 0f;
            thisUnit._nav.acceleration = 0f;
            thisUnit._nav.velocity = Vector3.zero;
            thisUnit._anim.SetBool("isMove", false);
            if (thisUnit._nav.isOnNavMesh)
            {
                thisUnit._nav.isStopped = true;
            }
            Debug.LogWarning("스턴시작 + ");
            thisUnit._status_Effect_Stun_Start.SetActive(true);
            thisUnit._status_Effect_Stun.SetActive(true);

            while (times <= duration)
            {
                if (!thisUnit.gameObject.activeSelf || isStatusApply|| thisUnit._unitData.hp <= 0f)
                {
                    thisUnit._anim.SetBool("isStun", false);
                    thisUnit._status_Effect_Stun_Start.SetActive(false);
                    thisUnit._status_Effect_Stun.SetActive(false);
                    //thisUnit._nav.speed = 0f;
                    //thisUnit._nav.acceleration = 0f;
                    thisUnit._nav.velocity = Vector3.zero;
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
            thisUnit._status_Effect_Stun_Start.SetActive(false);
            thisUnit._status_Effect_Stun.SetActive(false);

            thisUnit._anim.SetBool("isStun", false);
            if (thisUnit._unitData.hp>0f)
            {
                if (thisUnit._nav.isOnNavMesh)
                {
                    thisUnit._nav.isStopped = false;
                }
                thisUnit.canAct = true;

            }

            //if (thisUnit.sprCol.enabled == true)
            //{

            //}


            thisUnit._nav.speed = defaultSpd;
            thisUnit._nav.acceleration = defaultAcc;

        }

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

