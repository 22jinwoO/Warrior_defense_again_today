using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_ : Abs_Bullet
{
    [Header("시작위치")]
    [SerializeField]
    private Vector3 startPosition;  // 시작 위치

    [Header("도착위치")]
    [SerializeField]
    private Vector3 endPosition;    // 도착 위치

    [Header("시작위치와 도착위치의 중간 위치값")]
    [SerializeField]
    private Vector3 center;     // 시작위치와 도착위치의 중간 위치값

    [Range(0, 1)]
    public float t;

    bool isArrive = false;

    private void Awake()
    {
        transform.position = _start_Pos.position;
        t = 20f * Time.deltaTime;
    }


    void Update()
    {
        transform.position=Vector3.Lerp(transform.position, _target_Unit.position, t);

        if (_target_BodyTr == null || _target_Unit == null || _target_Unit.GetComponent<SphereCollider>().enabled == false)
        {
            Destroy(gameObject);
        }
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
