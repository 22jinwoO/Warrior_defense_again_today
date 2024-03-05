using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_ : Abs_Bullet
{
    [SerializeField]
    private Vector3 startPosition;

    [SerializeField]
    private Vector3 endPosition;

    [SerializeField]
    private Vector3 center;

    [Range(0, 1)]
    public float t;

    LineRenderer lr;

    bool isArrive = false;
    //
    [SerializeField]
    private float slerpValue;

    private void Awake()
    {
        transform.position = _start_Pos.position;
        t = 20f * Time.deltaTime;
    }


    void Update()
    {
        transform.position=Vector3.Lerp(transform.position, _target_Unit.position, t);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.Equals(_target_Unit))
        {
            Debug.LogWarning(other.name);
            other.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(_skill, _skill._skill_AtkType, ref unitInfoCs, other.transform);
            isArrive = true;
            //transform.SetParent(_target_Unit.transform);
            Destroy(gameObject);
        }
    }


}
