using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class Rush : SpecialSkill
{
    [SerializeField]
    private bool isArrive;

    [SerializeField]
    private float slerpValue=3f;

    [SerializeField]
    private float lerpValue = 8f;


    [SerializeField]
    private Rigidbody rigd;

    [SerializeField]
    private Vector3 startPosition;

    [SerializeField]
    private Vector3 endPosition;

    [SerializeField]
    private Vector3 center;


    [SerializeField]
    private Vector3 _target_Direction;

    [SerializeField]
    private Transform _target_BodyTr;

    [SerializeField]
    private SphereCollider sprCol;

    private void Awake()
    {
        lerpValue = 8f;
    }

    public override void Attack_Skill()
    {
        _target_BodyTr = unitTargetSearchCs._targetUnit;
        StartCoroutine(Rush_Attack());

    }

    IEnumerator Rush_Attack()
    {
        startPosition = unitInfoCs.transform.position;    // 발사체 시작 위치
        endPosition = _target_BodyTr.position;  // 발사체 도착 위치

        center = (startPosition + endPosition) * 0.5f;  // 시작위치와 도착위치를 합한 값 /2 를 하여 중간 위치값 구하기

        center.y -= 5; // 포물선이 위로 그려져야 하므로 y 값 - 해주기

        startPosition -= center;    //startposition 위치값을 center값을 기준으로 나타내기 위해 빼줌
        endPosition -= center;  //endPosition 위치값을 center값을 기준으로 나타내기 위해 빼줌

        for (float t = 0; t < 0.5f; t += Time.deltaTime * slerpValue)
        {

            Vector3 point = Vector3.Slerp(startPosition, endPosition, t);

            point += center;
            //transform.position = point;
            // 화살 촉이 다음에 이동할 위치 바라보도록
            //_target_Direction = point - unitInfoCs.transform.position;

            //Quaternion rot = Quaternion.LookRotation(_target_Direction.normalized);

            //unitInfoCs.transform.rotation = rot;
            print(t);
            unitInfoCs.transform.position = point;
            //print(transform.gameObject);
            yield return null;
        }

        for (float t = 0; t < 0.5f; t += Time.deltaTime * lerpValue)
        {
            transform.position = Vector3.Lerp(transform.position, _target_BodyTr.position, Time.deltaTime * lerpValue);
            yield return null;
        }


        yield return null;

        // 넉백 하기 위한 콜라이더 생성
        Collider[] hitCols = Physics.OverlapSphere(unitInfoCs.transform.position, 2.5f, unitTargetSearchCs._layerMask);

        Transform _longTarget = null;  // 가장 가까운 적을 의미하는 변수

        Debug.LogWarning(hitCols.Length);  

        yield return null;

        foreach (var _colTarget in hitCols)
        {
            Debug.LogWarning(_colTarget.gameObject.name);

            _colTarget.GetComponent<UnitInfo>().canAct = false;

            _colTarget.GetComponent<NavMeshAgent>().enabled = false;
            print(_colTarget.name);

            Vector3 targetRot = unitInfoCs.transform.position - _colTarget.transform.position;
            targetRot.y = 0f;

            Quaternion rot = Quaternion.LookRotation(targetRot.normalized);


            _colTarget.transform.rotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);

            StartCoroutine(DO_NuckBack(_colTarget.GetComponent<Rigidbody>(),_colTarget.transform));
        }


        print("애니메이션 호출 함수");

        // 돌격한 유닛이 타겟을 바라보는 방향의 각도를 구한 후 각도 전환
        _target_Direction = _target_BodyTr.position - unitInfoCs.transform.position;

        Quaternion rot2 = Quaternion.LookRotation(_target_Direction.normalized);

        unitInfoCs.transform.rotation = rot2;

        unitInfoCs.transform.rotation = Quaternion.Euler(0, rot2.eulerAngles.y, 0);

        unitInfoCs.transform.GetComponent<NavMeshAgent>().enabled = true;
    }


    IEnumerator DO_NuckBack(Rigidbody targetRigd, Transform other)
    {
        //yield return new WaitForSeconds(0.1f);


        Debug.LogWarning("넉백됨");
        float time = 0f;
        float nuckBackValue = 7.5f;
        float nuckBackValue2 = 200f;
        targetRigd.velocity = Vector3.zero;



        //y= ax + b (a: 넉백가속도 b: 초기 넉백속도. x: 넉백시간, y: 넉백속도)

        float y = 0f;
        // 벨로시티로 넉백 구현
        while (time < 0.2f)
        {
            targetRigd.velocity = (-(other.transform.forward) * nuckBackValue2 * Time.deltaTime);
            nuckBackValue2 -= 3.5f;

            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        UnitInfo monsterInfo = targetRigd.GetComponent<UnitInfo>();
        // 넉백된 몬스터들 행동하기 위해 필요한 값들 활성화
        monsterInfo.canAct = true;

        targetRigd.velocity = Vector3.zero;
        targetRigd.GetComponent<NavMeshAgent>().enabled = true;
        targetRigd.GetComponent<NavMeshAgent>().isStopped = false;

        if (monsterInfo._enum_mUnit_Action_BaseMode.Equals(eUnit_Action_States.monster_NormalPhase))
        {
            targetRigd.GetComponent<NavMeshAgent>().SetDestination(Castle.Instance.transform.position);
        }




        //targetRigd.rotation= Quaternion.Euler(0f,targetRigd.rotation.y,targetRigd.rotation.z);

    }
    // 충돌하면 공격 애미네이션 실행하고 데미지 들어가게 하도록


}
