using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet_Arrow : Abs_Bullet
{

    private void Awake()
    {
        print(_start_Pos.rotation);
        transform.SetParent(_start_Pos.transform.parent);
        transform.position = _start_Pos.position;
        print(_start_Pos.localRotation.y);
        //transform.rotation = UnityEngine.Quaternion.Euler(-90, _start_Pos.transform.eulerAngles.y, _start_Pos.transform.eulerAngles.z);

    }
    // Update is called once per frame
    void Update()
    {
        //Vector3 dir = _target_Unit.position - transform.position;
        ////dir.Normalize();
        ////dir.y = 0;

        //Quaternion _lookRotation = Quaternion.LookRotation(dir.normalized);  // 타겟 쪽으로 바라보는 각도

        //transform.localRotation = Quaternion.Euler(-90, _lookRotation.y, _lookRotation.z);
        //transform.rotation = Quaternion.LookRotation().normalized;
        transform.LookAt(new UnityEngine.Vector3(_target_Unit.position.x,0f,_target_Unit.position.z));
        transform.position = UnityEngine.Vector3.Lerp(transform.position, _target_Unit.position, Time.deltaTime*4f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.Equals(_target_Unit.transform))
        {
            other.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(other.transform,atkDmg);
            Destroy(gameObject);
        }
    }
}
