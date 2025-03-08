using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet_Arrow : Abs_Bullet
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

    [Header("시간")]
    [Range(0, 1)]
    public float _t; // 시간

    private bool isArrive = false;  // 도착했는지 확인하기 위한 변수

    [Header("Slerp 함수로 이동할 시 의 속도 값")]
    // Slerp 함수로 이동할 시 의 속도 값
    [SerializeField]
    private float slerpValue;

    [Header("Lerp함수로 이동할 시의 속도 값")]
    // Lerp 함수로 이동할 시 의 속도 값
    [SerializeField]
    private float lerpValue;

    private void Awake()
    {
        slerpValue = 4f;
        lerpValue = 8f;
        transform.position = _start_Pos.position;
        transform.rotation = unitInfoCs.transform.rotation;
        //transform.SetParent(GameObject.FindGameObjectWithTag("HoldPrefabs").transform);
        StartCoroutine(Move_Slerp());
    }

    // Update is called once per frame
    void Update()
    {
        // 발사체가 타겟의 위치까지 중간까지 도착했을 때 Lerp함수를 통해 타겟까지 이동
        if (isArrive)
        {
            if (_target_Direction != Vector3.zero)
            {
                // 타겟을 향한 방향 정규화
                _target_Direction = _target_Direction.normalized;
            }
            Quaternion rot = Quaternion.LookRotation(_target_Direction);
            transform.rotation = rot;
            transform.position = Vector3.Lerp(transform.position, _target_BodyTr.position, Time.deltaTime * lerpValue);
            
        }

        // 타겟이 죽었을 경우 발사체 파괴
        if (_target_BodyTr == null || _target_Unit == null || _target_Unit.GetComponent<SphereCollider>().enabled == false)
        {
            Destroy(gameObject);
        }
        //if (unitInfoCs.unitTargetSearchCs._targetUnit == null)
        //{
        //    Destroy(gameObject);
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        // 타겟과 발사체 충돌 시 실행
        if (other.transform.Equals(_target_Unit))
        {
            print("충돌완룡");
            //타겟의 데미지 입는 함수 호출
            other.GetComponent<UnitHp>().NewBeAttacked_By_OtherUnit(skill: _newSkill, AtkType: _newSkill._skill_AtkType, attacker: ref unitInfoCs, other: other.transform);

            //// 도착 변수 false로 변경
            //isArrive = false;

            //// 발사체가 타겟을 따라다녀야 하기 떄문에, 발사체를 타겟의 자식으로 넣어줌
            //transform.SetParent(_target_Unit.transform);

            //// 발사체 4초뒤 파괴
            //Destroy(gameObject,4f);
        }
    }

    #region # Move_Slerp : 곡선이동 기능 구현한 함수
    // 곡선이동 기능 구현한 함수
    private IEnumerator Move_Slerp()
    {
        startPosition = _start_Pos.position;    // 발사체 시작 위치
        endPosition = _target_BodyTr.position;  // 발사체 도착 위치

        center = (startPosition + endPosition) * 0.5f;  // 시작위치와 도착위치를 합한 값 /2 를 하여 중간 위치값 구하기

        center.y -= 15; // 포물선이 위로 그려져야 하므로 y 값 - 해주기

        startPosition -= center;    //startposition 위치값을 center값을 기준으로 나타내기 위해 빼줌
        endPosition -= center;  //endPosition 위치값을 center값을 기준으로 나타내기 위해 빼줌

        // t의 값이 0.55보다 작을 때 동안 화살이동 (중간지점 까지 이동)
        for (float t = 0; t < 0.55f; t += Time.deltaTime* slerpValue)
            {
                Vector3 point = Vector3.Slerp(startPosition, endPosition, t);

                point += center;
                
                // 화살 촉이 다음에 이동할 위치 바라보도록
                _target_Direction = point - transform.position;

                // 타겟을 향한 방향 바라보는 각도 구하기
                Quaternion rot = Quaternion.LookRotation(_target_Direction.normalized);

                transform.rotation = rot;

                transform.position = point;

                yield return null;
            }
        isArrive = true;
    }
    #endregion
}
