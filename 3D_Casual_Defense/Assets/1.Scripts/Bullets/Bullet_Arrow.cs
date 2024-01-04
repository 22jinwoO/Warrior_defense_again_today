using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet_Arrow : Abs_Bullet
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

    [SerializeField]
    private float slerpValue;

    [SerializeField]
    private float lerpValue;

    private void Awake()
    {
        slerpValue = 4f;
        lerpValue = 8f;
        //print(_start_Pos.rotation);
        //transform.SetParent(_start_Pos.transform.parent);
        transform.position = _start_Pos.position;
        transform.rotation = unitInfoCs.transform.rotation;
        //_target_Direction = _target_BodyTr.position - transform.position;
        //print(_target_Direction.normalized);
        //Quaternion rot = Quaternion.LookRotation(_target_Direction.normalized);
        //transform.rotation = rot;

        //print(_start_Pos.localRotation.y);
        //transform.rotation = UnityEngine.Quaternion.Euler(-90, _start_Pos.transform.eulerAngles.y, _start_Pos.transform.eulerAngles.z);
        StartCoroutine(Move_Slerp());
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isArrive)
        {
            _target_Direction = _target_BodyTr.position - transform.position;
            print(_target_Direction.normalized);
            Quaternion rot = Quaternion.LookRotation(_target_Direction.normalized);
            transform.rotation = rot;
            transform.position = Vector3.Lerp(transform.position, _target_BodyTr.position, Time.deltaTime * lerpValue);

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.Equals(_target_Unit))
        {
            other.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(_skill, _skill._skill_AtkType,other.transform, _skill._base_Value);
            Destroy(gameObject);
        }
    }

    IEnumerator Move_Slerp()
    {
        startPosition = _start_Pos.position;    // 발사체 시작 위치
        endPosition = _target_BodyTr.position;  // 발사체 도착 위치

        center = (startPosition + endPosition) * 0.5f;  // 시작위치와 도착위치를 합한 값 /2 를 하여 중간 위치값 구하기

        center.y -= 15; // 포물선이 위로 그려져야 하므로 y 값 - 해주기

        startPosition -= center;    //startposition 위치값을 center값을 기준으로 나타내기 위해 빼줌
        endPosition -= center;  //endPosition 위치값을 center값을 기준으로 나타내기 위해 빼줌

        for (float t = 0; t < 0.55f; t += Time.deltaTime* slerpValue)
            {
                Vector3 point = Vector3.Slerp(startPosition, endPosition, t);

                point += center;
                
                // 화살 촉이 다음에 이동할 위치 바라보도록
                _target_Direction = point - transform.position;

                Quaternion rot = Quaternion.LookRotation(_target_Direction.normalized);

                transform.rotation = rot;

                transform.position = point;

                yield return null;
            }
        isArrive = true;
    }
}
