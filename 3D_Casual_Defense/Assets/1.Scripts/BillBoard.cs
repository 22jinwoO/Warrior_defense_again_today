using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;


    // Start is called before the first frame update
    void Start()
    {
        // 메인카메라 가져오기
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        // 오브젝트의 정면은 카메라의 정면과 같은 방향을 바라봄
        transform.forward = mainCamera.transform.forward;
    }
}
