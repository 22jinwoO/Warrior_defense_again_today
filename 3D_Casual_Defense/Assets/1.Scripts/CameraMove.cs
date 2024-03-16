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
    private Player player;

    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    public float speed = 15f;

    public MonsterSpawnManager spawnManagerCs;

    private void Awake()
    {
        StartCoroutine(CameraProduction());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, 0f, 55f));

        if (Application.platform==RuntimePlatform.Android&&!player.isChoice && player.canPlay)
        {
            OnCameraMove();
            OnCameraZoomMode();
            OnUnitMove();


        }

        if(player.isChoice && player.canPlay)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, player.clickUnitInfo.transform.position.z);
        }

        if (!player.isChoice &&player.canPlay)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(0, 0, speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(0, 0, -speed * Time.deltaTime);
            }

        }

        float wheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (wheelInput > 0)
        {        // 휠을 밀어 돌렸을 때의 처리 ↑   
                 mainCam.fieldOfView -= 1f * zoomSpeed;

        }
        else if (wheelInput < 0)    
        
        {        // 휠을 당겨 올렸을 때의 처리 ↓   
            mainCam.fieldOfView += 1f * zoomSpeed;


        }


        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    mainCam.fieldOfView += 1f * zoomSpeed;
        //    textMeshProUGUI.text = "카메라 줌 아웃";
        //}
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    mainCam.fieldOfView -= 1f * zoomSpeed;
        //    textMeshProUGUI.text = "카메라 줌인";
        //}
    }
    private void OnUnitMove()
    {
        if(!player.isMove.Equals(true))
            return;

        Touch firstTouch = Input.GetTouch(0);   // 첫번째 터치 정보

        // 직전 프레임의 터치 위치를 구하기 위해 "현재 위치 - 위치 변화량" 을 계산
        Vector2 previousTouch = (firstTouch.position - firstTouch.deltaPosition);

        // 카메라 움직일 방향 구하기
        Vector2 dir = firstTouch.position - previousTouch;

        // 방향 적용한 벡터
        Vector3 vec = new Vector3(dir.x, 0, dir.y);

        // 카메라 움직임 적용 (플래그가 이동해야 할 방향으로 카메라가 움직여야 하기 때문에 + 해줘야함)
        camTr.position += vec * zoomSpeed * Time.deltaTime;
        textMeshProUGUI.text = "유닛 움직임 카메라 이동중\n " + " " + dir + "\n " + vec.magnitude;
    }


    private void OnCameraMove()
    {
        if (Input.touchCount != 1||player.isMove.Equals(true))
            return;

        Touch firstTouch = Input.GetTouch(0);   // 첫번째 터치 정보

        // 직전 프레임의 터치 위치를 구하기 위해 "현재 위치 - 위치 변화량" 을 계산
        Vector2 previousTouch = (firstTouch.position - firstTouch.deltaPosition);

        // 카메라 움직일 방향 구하기
        Vector2 dir = firstTouch.position - previousTouch;

        // 방향 적용한 벡터
        Vector3 vec = new Vector3(dir.x, 0, dir.y);

        // 카메라 움직임 적용 (드래그한 방향 반대로 카메라가 움직여야 하기 때문에 - 해줘야함)
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

    // 시작 시 카메라 연출 기능하는 스크립트
    private IEnumerator CameraProduction()
    {
        while(transform.position.z>0) 
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,transform.position.z-1);
            yield return new WaitForSecondsRealtime(0.04f);
        }

        yield return new WaitForSecondsRealtime(1.5f);


        while (54>transform.position.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
            yield return new WaitForSecondsRealtime(0.04f);
        }
        spawnManagerCs.waveSys.StartWave();
        player.canPlay = true;

    }
}
