using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraMove : MonoBehaviour
{
    [Header("카메라 줌 모드")]

    [SerializeField]
    private Transform camTr;

    [SerializeField]
    private Camera mainCam;

    [SerializeField]
    private float zoomSpeed = 6.0f;

    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    // Update is called once per frame
    void Update()
    {
        OnCameraMove();
        OnCameraZoomMode();
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            mainCam.fieldOfView += 1f * zoomSpeed;
            textMeshProUGUI.text = "카메라 줌 아웃";
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            mainCam.fieldOfView -= 1f * zoomSpeed;
            textMeshProUGUI.text = "카메라 줌인";
        }
    }

    private void OnCameraMove()
    {
        if (Input.touchCount != 1)
            return;

        Touch firstTouch = Input.GetTouch(0);   // 첫번째 터치 정보

        // 직전 프레임의 터치 위치를 구하기 위해 "현재 위치 - 위치 변화량" 을 계산
        Vector2 previousTouch = (firstTouch.position - firstTouch.deltaPosition);

        Vector2 dir = firstTouch.position - previousTouch;

        Vector3 vec = new Vector3(dir.x, 0, dir.y);




        camTr.position -= vec * zoomSpeed * Time.deltaTime;
        textMeshProUGUI.text = "카메라 이동중\n " + " " + dir + "\n " + vec.magnitude;

    }

    private void OnCameraZoomMode()
    {
        if(Input.touchCount!=2)
            return;

        Touch firstTouch = Input.GetTouch(0);   // 첫번째 터치 정보
        Touch secondTouch = Input.GetTouch(1);  // 두번째 터치 정보

        // 직전 프레임의 터치 위치를 구하기 위해 "현재 위치 - 위치 변화량" 을 계산
        Vector2 firstTouchPreviousPosition = firstTouch.position - firstTouch.deltaPosition;
        Vector2 secondTouchPreviousPosition = secondTouch.position - secondTouch.deltaPosition;

        // 직전 프레임에서 터치 위치 거리 값
        float previousPositionDistance = (firstTouchPreviousPosition - secondTouchPreviousPosition).magnitude;

        // 현재 프레임에서의 터치 위치 거리 값
        float currentPositionDistance = (firstTouch.position - secondTouch.position).magnitude;

        // 직전 프레임과 현재 프레임의 위치 변화량 (줌 수치)
        float zommModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude;

        // 직전 프레임의 거리 값이 현재 프레임의 거리 값보다 클 때  = 줌 아웃
        if (previousPositionDistance > currentPositionDistance)
        {
            mainCam.fieldOfView += 1f * zoomSpeed;
            textMeshProUGUI.text = "카메라 줌 아웃\n"+ mainCam.fieldOfView;

        }

        else if(previousPositionDistance < currentPositionDistance)
        {
            mainCam.fieldOfView -= 1f * zoomSpeed;
            textMeshProUGUI.text = "카메라 줌\n" + mainCam.fieldOfView;

        }

    }
}
