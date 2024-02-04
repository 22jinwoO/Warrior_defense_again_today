using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public abstract class Abs_StatusEffect 
{
    // 상태이상 객체마다 갖고 있어야 함..
    public string link_id;
    public string link_name;
    public string link_script;
    public int linkValue_ps;
    public int duration_s;
    public int moveSpeedreduce;
    //public GameObject linkVfx;
    //public GameObject linkSfx;
    //public GameObject icon;
    public bool isStatusApply;

    //상태이상 효과 적용하는 함수
    public abstract IEnumerator Apply_Status_Effect(UnitInfo thisUnit, string linkId, int statusValue, float duration);

    //bool isPoison;
    //bool isStun;
    //bool isBleed;
    //bool isBurn;
    //bool isReduceMoveSpd;

    //public unit_Data unit_Data;

    // 중독 상태 효과
    //public IEnumerator Get_Posion(UnitInfo thisUnit, string linkId, int statusValue, int duration)
    //{
    //    yield return new WaitForSeconds(1f);
    //    isPoison = false;

    //    int times = 0;

    //    while (times < duration)
    //    {
    //        if (isPoison)
    //        {
    //            yield return null;

    //            break;
    //        }
    //        thisUnit._unitData.hp -= statusValue;

    //        times++;
    //        yield return new WaitForSeconds(1f);
    //    }
    //}

    //public IEnumerator Get_ReduceSpd(UnitInfo thisUnit, string linkId, int statusValue, int duration)
    //{
    //    yield return new WaitForSeconds(0.8f);
    //    isReduceMoveSpd = false;
    //    float defaultSpd = thisUnit._nav.speed;
    //    float defaultAcc = thisUnit._nav.acceleration;
    //    int times = 0;
    //    thisUnit._nav.speed = defaultSpd / 2;
    //    thisUnit._nav.acceleration = defaultAcc / 2;
    //    while (times < duration)
    //    {
    //        if (isPoison)
    //        {
    //            yield return null;

    //            break;
    //        }

    //        times++;
    //        yield return new WaitForSeconds(1f);
    //    }
    //    thisUnit._nav.speed = defaultSpd;
    //    thisUnit._nav.acceleration = defaultAcc;
    //}

}

// 중독

// 기절

// 출혈

// 화염피해

// 이동속도 감소
