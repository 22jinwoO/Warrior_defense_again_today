using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Test : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    private float speed;

    [SerializeField]
    private bool isStart;

    [SerializeField]
    private int hp;

    [SerializeField]
    private bool isReStart;

    int sdfa = 0;
    // Start is called before the first frame update
    void Start()
    {
        hp = 100;
        speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isReStart = true;
            StartCoroutine(Tests());
        }
        //
        //transform.position=Vector3.Lerp(transform.position, target.position, Time.deltaTime*3f);
    }

    IEnumerator Tests() // 출혈 중독 화염피해 상태는 값만 다르고 효과는 다 똑같으므로 매개변수로 값을 받는 공통되게 적용되는 함수 구현하기
    {

        yield return new WaitForSeconds(1f);
        isStart = true;
        isReStart = false;


        //speed /= 2;
        print("상태이상 효과 적용");
        int times=0;

        while (times < 5)
        {
            if (isReStart)
            {
                print("함수탈출");
                yield return null;

                break;
            }
            hp -= 5;
            times++;
            yield return new WaitForSeconds(1f);
        }
        isStart = false;
        //isReStart = false;
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
