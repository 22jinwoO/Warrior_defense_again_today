using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    [SerializeField]
    private GameObject sfxOb;

    // 피격 이펙트 활성화 시키는 함수
    public void Test3(Vector3 sfxPos)
    {
        GameObject ad = Instantiate(sfxOb, transform.position + -sfxPos * 0.7f, Quaternion.identity);
        Quaternion rot2 = Quaternion.LookRotation(-(sfxPos.normalized));

        ad.transform.rotation = rot2;

        ad.transform.rotation = Quaternion.Euler(0, rot2.eulerAngles.y, 0);
    }

}
