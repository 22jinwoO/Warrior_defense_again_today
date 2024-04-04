using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public abstract class Abs_StatusEffect 
{
    // 상태이상 객체마다 갖고 있어야 함
    public string link_id;
    public string link_name;
    public string link_script;
    public int linkValue_ps;
    public int duration_s;
    public int moveSpeedreduce;
    public bool isStatusApply;

    //상태이상 효과 적용하는 함수
    public abstract IEnumerator Apply_Status_Effect(UnitInfo thisUnit, string linkId, int statusValue, float duration);
}
