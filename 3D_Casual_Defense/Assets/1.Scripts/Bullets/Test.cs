using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.Windows.Speech;

public class Test : MonoBehaviour
{


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
    private float slerpValue=8f;

    [SerializeField]
    private SphereCollider sprCol;

    // Start is called before the first frame update
    void Start()
    {
        //hp = 100;
        //speed = 0;
        //startPosition = transform.position;    // 발사체 시작 위치
        //endPosition = _target_BodyTr.position;  // 발사체 도착 위치

        //center = (startPosition + endPosition) * 0.5f;  // 시작위치와 도착위치를 합한 값 /2 를 하여 중간 위치값 구하기

        //center.y -= 15; // 포물선이 위로 그려져야 하므로 y 값 - 해주기

        //startPosition -= center;    //startposition 위치값을 center값을 기준으로 나타내기 위해 빼줌
        //endPosition -= center;  //endPosition 위치값을 center값을 기준으로 나타내기 위해 빼줌
        rigd = GetComponent<Rigidbody>();
        sprCol = GetComponent<SphereCollider>();
        //Testgo();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(Move_Slerp());

        }

        //speed += 1;
        //if (isStart) 
        //{
        //    speed += 1;

        //    speed /= 2;
        //}

        //for (int i = sdfa; i < 5; sdfa++)
        //{
        //    if (sdfa==5)
        //    {
        //        return;
        //    }
        //    speed += Time.deltaTime;
        //    print(speed);
        //}

        //
        //transform.position=Vector3.Lerp(transform.position, target.position, Time.deltaTime*3f);


    }

    IEnumerator Move_Slerp()
    {
        startPosition = transform.position;    // 발사체 시작 위치
        endPosition = _target_BodyTr.position;  // 발사체 도착 위치

        center = (startPosition + endPosition) * 0.5f;  // 시작위치와 도착위치를 합한 값 /2 를 하여 중간 위치값 구하기

        center.y -= 5; // 포물선이 위로 그려져야 하므로 y 값 - 해주기

        startPosition -= center;    //startposition 위치값을 center값을 기준으로 나타내기 위해 빼줌
        endPosition -= center;  //endPosition 위치값을 center값을 기준으로 나타내기 위해 빼줌

        for (float t = 0; t < 0.9f; t += Time.deltaTime * slerpValue)
        {

            Vector3 point = Vector3.Slerp(startPosition, endPosition, t);

            point += center;
            //transform.position = point;
            // 화살 촉이 다음에 이동할 위치 바라보도록
            _target_Direction = point - transform.position;

            Quaternion rot = Quaternion.LookRotation(_target_Direction.normalized);

            transform.rotation = rot;
            print(t);
            transform.position = point;
            //print(transform.gameObject);
            yield return null;
        }
        yield return null;

        sprCol.enabled = true;

        transform.GetComponent<NavMeshAgent>().enabled= true;
        transform.rotation = Quaternion.Euler(Vector3.zero);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>()!=null)
        {
            Rigidbody DA     = other.GetComponent<Rigidbody>();
            print(DA.transform.gameObject.name);
            other.GetComponent<NavMeshAgent>().enabled = false;
            Vector3 targetRot = transform.position - other.transform.position;

            Quaternion rot = Quaternion.LookRotation(targetRot.normalized);

            //DA.rotation = Quaternion.Euler(0f,rot.y,rot.z);
            //DA.rotation = new Quaternion(0f,rot.y,0f,0f);
            DA.rotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);
            //DA.AddForce(-(other.transform.forward) * 20f * Time.deltaTime, ForceMode.Impulse);
            print(other.transform.gameObject.name);
            other.GetComponent<UnitInfo>().canAct = false;

            other.GetComponent<NavMeshAgent>().enabled = false;
            StartCoroutine(Aasdf(DA,other.transform));
            //DA.rotation= Quaternion.Euler(0f,DA.rotation.y,DA.rotation.z);
            //DA.AddForce(-(other.transform.forward) * 6f*Time.deltaTime, ForceMode.Impulse);
            //DA.AddExplosionForce(200f,transform.position,20f);
        }
    }
    IEnumerator Aasdf(Rigidbody DA, Transform other)
    {
        //yield return new WaitForSeconds(0.1f);

        float time = 0f;
        float nuckBackValue = 7.5f;
        float nuckBackValue2 = 700f;
        DA.velocity = Vector3.zero;
        other.GetComponent<UnitInfo>().canAct = false;

        other.GetComponent<NavMeshAgent>().enabled = false;
        //y= ax + b (a: 넉백가속도 b: 초기 넉백속도. x: 넉백시간, y: 넉백속도)

        float y=0f;

        // 벨로시티
        while (time < 0.2f)
        {

            //y = 70f * time + nuckBackValue2 * Time.deltaTime;

            DA.velocity = (-(other.transform.forward) * nuckBackValue2 * Time.deltaTime);
            nuckBackValue2 -= 10f;
            time += Time.deltaTime;
            yield return null;
        }

        //럴프 넉백
        //while (time < 0.15f)
        //{
        //    Vector3 KnockBackPos = other.transform.position + -(other.transform.forward); // 넉백 시 이동할 위치

        //    other.transform.position = Vector3.Lerp(other.transform.position, KnockBackPos, 10 * Time.deltaTime);
        //    time += Time.deltaTime;
        //    yield return null;
        //}


        //while (time < 0.3f)
        //{

        //    y = 70f * time + nuckBackValue;

        //    //DA.AddForce(-(other.transform.forward) * nuckBackValue * Time.deltaTime, ForceMode.Impulse);
        //    //DA.velocity = (-(other.transform.forward) * y * Time.deltaTime);

        //    DA.AddRelativeForce(-(Vector3.forward) * y * Time.deltaTime, ForceMode.Impulse);

        //    //애드포스
        //    //DA.AddForce(-(other.transform.forward) * y * Time.deltaTime, ForceMode.Impulse);

        //    ////nuckBackValue -= 0.125f;
        //    //print((-(other.transform.forward) * y * Time.deltaTime).magnitude);
        //    time += Time.deltaTime;
        //    yield return null;
        //}
        //yield return new WaitForSeconds(0.3f);
        //DA.AddForce(-(other.transform.forward) * 200f * Time.deltaTime, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        DA.GetComponent<UnitInfo>().canAct = true;

        DA.velocity = Vector3.zero;
        DA.GetComponent<NavMeshAgent>().enabled = true;
        DA.GetComponent<NavMeshAgent>().isStopped = false;
        //DA.rotation= Quaternion.Euler(0f,DA.rotation.y,DA.rotation.z);

    }


    void Testgo()
    {


        for (float t = 0; t < 0.55f; t += Time.deltaTime * slerpValue)
        {
            Vector3 point = Vector3.Slerp(startPosition, endPosition, t);

            point += center;
            print(point);
            // 화살 촉이 다음에 이동할 위치 바라보도록
            _target_Direction = point - transform.position;

            Quaternion rot = Quaternion.LookRotation(_target_Direction.normalized);

            transform.rotation = rot;

            transform.position = point;

        }
    }

}

public class asdf
{
    public Transform asd;
    IEnumerator asddf()
    {
        yield return null;
    }
}
