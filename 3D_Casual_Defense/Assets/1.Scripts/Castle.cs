using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Singleton<Castle>
{
    public Transform _castle_Pos;
    public float _castle_Hp;

    private void Awake()
    {
        _castle_Hp = 100f;
    }
    
    public void Damaged_Castle()
    {
        _castle_Hp -= 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<UnitInfo>()._anim.SetBool("isMove", false);

            other.GetComponent<UnitInfo>()._enum_Unit_Action_Mode = eUnit_Action_States.monster_AttackCastlePhase;
            other.GetComponent<UnitInfo>()._enum_Unit_Action_State = eUnit_Action_States.unit_Attack;
        }
    }
}
