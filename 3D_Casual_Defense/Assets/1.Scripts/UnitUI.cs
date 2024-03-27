using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    RectTransform rectParent;

    [SerializeField]
    RectTransform rectHp;

    [SerializeField]
    Transform targetTr;

    [SerializeField]
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        canvas = GetComponentInParent<Canvas>();
        rectParent = canvas.GetComponent<RectTransform>();
        //rectHp = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset); // 몬스터의 월드 3d좌표를 스크린좌표로 변환

        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, mainCamera, out localPos); // 스크린 좌표를 다시 체력바 UI 캔버스 좌표로 변환

        rectHp.localPosition = localPos; // 체력바 위치조정
        rectHp.forward = mainCamera.transform.forward;

        //transform.LookAt(mainCamera.transform);
        //transform.Rotate(transform.eulerAngles.x, 0f, 180);
    }
}