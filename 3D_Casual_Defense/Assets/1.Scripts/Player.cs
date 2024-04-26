using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using EnumTypes;
using System;

namespace EnumTypes
{
    public enum AttackTypes
    {
        None, Melee, Range
    }

    public enum CardRanks
    {
        Normal, Special, Rare
    }

    public enum CardHowToUses
    {
        Normal, TargetGround, TargetEntity
    }

    public enum CardAfterUses
    {
        Discard, Destruct, Spawn
    }

    public enum GameFlowState
    {
        InitGame, SelectStage, Setting, Wave, EventFlow, Ending
    }
}

public class Player : MonoBehaviour, IDragHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private Material _flag_Material;


    [SerializeField]
    private Material[] _Materials;

    [SerializeField]
    private CreatePlayerUnit clickUnitCs;

    public PlayerUnitClass clickUnitInfo;

    public Transform flagTr;

    [SerializeField]
    public TextMeshProUGUI textMeshProUGUI;

    public bool isChoice;
    public bool isMove;
    public bool canPlay;

    [SerializeField]
    private Transform emptyParent;

    public GameObject unitCtrlCanvas;

    [SerializeField]
    private float xValue, yValue, zValue;

    [SerializeField]
    private AudioSource audioPlayer;


    [SerializeField]
    private AudioClip[] audioSources;

    [SerializeField]
    private Button holdImg;

    [SerializeField]
    private Button freeImg;

    [SerializeField]
    private float times = 0f;

    [SerializeField]
    private LayerMask layermask;

    private void Awake()
    {

        if (TryGetComponent(out AudioSource audioSource))
        {
            audioPlayer = audioSource;
        }


        _flag_Material = flagTr.GetComponent<Material>();
        canPlay = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (canPlay)
        {
            // 유닛 클릭 지속 시 이동상태로 전환시켜주는 구문 (유닛 선택 O / 드래그하여 이동 위치 지정중이 아닐 때)
            if (isChoice && Input.GetMouseButton(0) && !isMove)
            {
                // 레이
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // 레이 발사
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // 레이에 맞은 타겟의 태그가 "Player" 이고 레이에 맞은 트랜스폼이 이전에 클릭한 유닛의 트랜스폼과 같지 않을 경우
                    if (hit.transform.CompareTag("Player") && !hit.transform.Equals(clickUnitInfo.transform))
                    {

                        // 이전에 클릭했던 유닛의 오브젝트 이름에 유닛의 캐릭터 id 명 부여
                        clickUnitInfo.gameObject.name = clickUnitInfo._unitData.char_id;

                        //unitCtrlCanvas.SetActive(false);

                        // 이전에 클릭했던 유닛의 선택상황 false
                        clickUnitInfo._isClick = false;

                        // 이전에 클릭했던 유닛 정보 = null
                        clickUnitInfo = null;

                        // 플레이어 UI의 클릭한 유닛의 정보 = null
                        clickUnitCs.clickUnitInfo = null;

                        // clickUnitInfo 변수에 레이를 맞은 타겟의 PlayerUnitClass 할당
                        clickUnitInfo = hit.transform.GetComponent<PlayerUnitClass>();

                        // 선택한 유닛의 선택상황 true
                        clickUnitInfo._isClick = true;

                        // 선택한 유닛의 오브젝트 이름 ClickUnit로 변경
                        clickUnitInfo.gameObject.name = "ClickUnit";

                        // 유닛 선택 오디오 실행
                        audioPlayer.PlayOneShot(audioSources[0]);

                        // 플레이어 UI의 클릭한 유닛의 정보에 데이터 할당
                        clickUnitCs.clickUnitInfo = hit.transform.GetComponent<PlayerUnitClass>();


                        // 플레이어 조작 창 활성화
                        unitCtrlCanvas.transform.position = transform.position;

                        // 클릭한 유닛의 액션 모드가 홀드모드 일 때 유닛 선택 ui 이미지 중 홀드모드 이미지 비활성화
                        if (clickUnitInfo._enum_Unit_Action_Mode.Equals(eUnit_Action_States.unit_HoldMode))
                        {
                            // 자유모드 버튼 이미지 활성화
                            clickUnitCs.clickUnitFreeBtnImg.sprite = clickUnitCs.freeImgs[0];

                            // 홀드모드 버튼 이미지 비활성화
                            clickUnitCs.clickUnitHoldBtnImg.sprite = clickUnitCs.holdImgs[2];

                            // 홀드모드 버튼 비활성화
                            holdImg.interactable = false;

                            // 자유모드 버튼 활성화
                            freeImg.interactable = true;
                        }

                        //  클릭한 유닛의 액션 모드가 자유모드 일 때 유닛 선택 ui 이미지 중 자유모드 이미지 비활성화
                        else if (clickUnitInfo._enum_Unit_Action_Mode.Equals(eUnit_Action_States.unit_FreeMode))
                        {
                            // 자유모드 버튼 이미지 비활성화
                            clickUnitCs.clickUnitFreeBtnImg.sprite = clickUnitCs.freeImgs[2];

                            // 홀드모드 버튼 이미지 활성화
                            clickUnitCs.clickUnitHoldBtnImg.sprite = clickUnitCs.holdImgs[0];

                            // 홀드모드 버튼 활성화
                            holdImg.interactable = true;

                            // 자유모드 버튼 비활성화
                            freeImg.interactable = false;
                        }

                        // 유닛 Ui 활성화
                        unitCtrlCanvas.SetActive(true);

                        // 유닛 선택 플래그 현재 자기 자신의 위치로 변경
                        flagTr.position = transform.position;

                        // 플래그 오브젝트 활성화
                        flagTr.gameObject.SetActive(true);

                        textMeshProUGUI.text = hit.transform.name + "지정 완료!\n" + "사용자 지정 가능여부 " + isChoice + "\n 유닛이동 가능 여부 " + hit.transform.GetComponent<PlayerUnitClass>()._isClick;

                        print("156 // 오류 1");

                        //isChoice = true;

                    }

                    // 맞은 유닛의 태그가 Player 고 선택한 유닛의 정보가 자유모드 일 때
                    else if (hit.transform.CompareTag("Player") && clickUnitInfo._enum_Unit_Action_Mode.Equals(eUnit_Action_States.unit_FreeMode))
                    {
                        // 해당 유닛을 누르고 있는 동안 시간 증가
                        times += Time.deltaTime;

                        // 시간이 0.6초보다 크거나 같아지면 이동모드로 전환
                        if (times >= 0.6f)
                        {
                            // 유닛 이동모드로 전환
                            isMove = true;

                            // 선택한 유닛의 이동모드 이미지 활성화
                            clickUnitInfo.moveImg.SetActive(true);

                            // 선택한 플래그 크기 커졌다가 줄어드는 애니메이션 실행
                            StartCoroutine(FlagAnim());

                            // 타임 = 0
                            times = 0f;
                        }

                    }

                    // 레이를 발사할 때 UnitUi 레이어마스크를 가진애들만 레이를 맞게 함.
                    else if (!Physics.Raycast(ray, out RaycastHit hit2, 1f, layermask))
                    {
                        print("174// 오류 3");
                        
                        // 유닛 선택 상황 
                        isChoice = false;

                        // 플래그 오브젝트 비활성화
                        flagTr.gameObject.SetActive(false);

                        // 선택한 유닛이 존재하고, 선택한 유닛의 모드전환이 끝난 상태일 때
                        if (clickUnitInfo != null && clickUnitInfo.isChangeState)
                        {
                            clickUnitInfo._isClick = false;
                            clickUnitInfo.gameObject.name = clickUnitInfo._unitData.char_id;
                        }



                        clickUnitInfo = null;


                        unitCtrlCanvas.SetActive(false);


                        clickUnitCs.clickUnitInfo = null;


                        isChoice = false;
                    }

                }
            }

            //유닛 첫 터치 시
            else if (!isMove && Input.GetMouseButtonDown(0) && !isChoice)
            {
                times = 0f;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        clickUnitInfo = hit.transform.GetComponent<PlayerUnitClass>();


                        if (!isChoice && !clickUnitInfo._isClick)
                        {
                            clickUnitInfo.gameObject.name = "ClickUnit";
                            audioPlayer.PlayOneShot(audioSources[0]);
                            clickUnitInfo._isClick = true;
                            //clickUnitInfo = hit.transform.GetComponent<PlayerUnitClass>();

                            //clickUnitInfo._isClick = true;
                            clickUnitCs.clickUnitInfo = hit.transform.GetComponent<PlayerUnitClass>();
                            //isMove = true;
                            _flag_Material = _Materials[0];

                            // 플레이어 조작 창 활성화
                            unitCtrlCanvas.transform.position = transform.position;
                            if (clickUnitInfo._enum_Unit_Action_Mode.Equals(eUnit_Action_States.unit_HoldMode))
                            {
                                clickUnitCs.clickUnitFreeBtnImg.sprite = clickUnitCs.freeImgs[0];
                                clickUnitCs.clickUnitHoldBtnImg.sprite = clickUnitCs.holdImgs[2];
                                holdImg.interactable = false;
                                freeImg.interactable = true;
                            }
                            else if (clickUnitInfo._enum_Unit_Action_Mode.Equals(eUnit_Action_States.unit_FreeMode))
                            {
                                clickUnitCs.clickUnitHoldBtnImg.sprite = clickUnitCs.holdImgs[0];
                                clickUnitCs.clickUnitFreeBtnImg.sprite = clickUnitCs.freeImgs[2];
                                holdImg.interactable = true;
                                freeImg.interactable = false;
                            }

                            unitCtrlCanvas.SetActive(true);
                            //플레이어 선택창 활성화
                            flagTr.position = transform.position;
                            flagTr.gameObject.SetActive(true);

                            textMeshProUGUI.text = hit.transform.name + "지정 완료!\n" + "사용자 지정 가능여부 " + isChoice + "\n 유닛이동 가능 여부 " + hit.transform.GetComponent<PlayerUnitClass>()._isClick;
                            isChoice = true;

                        }
                        //if(isChoice)
                        //{
                        //    times += Time.deltaTime;
                        //    if (times>=0.3f)
                        //    {
                        //        isMove = true;
                        //        StartCoroutine(FlagAnim());
                        //        times = 0f;
                        //    }
                        //}
                    }


                    else if (!hit.transform.tag.Equals("UnitUI"))
                    {
                        print("174// 오류 1");
                        print(hit.transform.name);
                        isChoice = false;
                        flagTr.gameObject.SetActive(false);

                        if (clickUnitInfo != null && clickUnitInfo.isChangeState)
                        {
                            clickUnitInfo._isClick = false;
                            clickUnitInfo.gameObject.name = clickUnitInfo._unitData.char_id;
                        }
                        print(hit.transform);
                        clickUnitInfo = null;
                        unitCtrlCanvas.SetActive(false);

                        clickUnitCs.clickUnitInfo = null;
                        isChoice = false;
                    }

                }





            }
            if (isChoice && !isMove)
            {
                transform.position = clickUnitInfo.transform.position;
                unitCtrlCanvas.transform.position = transform.position + new Vector3(xValue, yValue, zValue);

                flagTr.position = transform.position;
            }
        }


    }

    //private void 

    private IEnumerator FlagAnim()
    {
        audioPlayer.PlayOneShot(audioSources[1]);
        _flag_Material.color = Color.white;

        // 유닛 설정 팝업창 비활성화
        unitCtrlCanvas.SetActive(false);

        float value = 4f;
        flagTr.localScale = new Vector3(value, 0f, value);
        while (value > 1.5f)
        {
            flagTr.localScale = new Vector3(value, 0f, value);
            value -= 0.1f;
            yield return null;

        }

    }



    public void OnDrag(PointerEventData eventData)
    {
        //times += Time.deltaTime;
        //if (times>=0.3f&&!isMove)
        //{
        //    StartCoroutine(FlagAnim());
        //    isChoice = true;

        //}
        if (isChoice&&isMove)
        {
            

            // 이동 위치 활성화
            flagTr.gameObject.SetActive(true);


            //isMove = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                transform.position = new Vector3(hit.point.x, 8.6f, hit.point.z);
                flagTr.position = transform.position;
                textMeshProUGUI.text = $"{hit.point}\n{transform.position}";
            }

            //Vector3 mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //transform.position = new Vector3(mouseInput.x, 0f, mouseInput.y);
            //textMeshProUGUI.text = $"드래그중\n유닛이 이동할 좌표 : {mouseInput}";
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //StartCoroutine(FlagAnim());
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isChoice&& isMove)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                transform.position = new Vector3(hit.point.x, 8.6f, hit.point.z);
                //if (hit.transform.CompareTag("Stairs"))
                //{
                //    transform.position = new Vector3(hit.point.x, 12.96f, hit.point.z);

                //    flagTr.position = new Vector3(hit.point.x, 12.96f, hit.point.z);
                //    clickUnitInfo._movePos = new Vector3(hit.point.x, 12.96f, hit.point.z);
                //    clickUnitInfo.arriveFlag.position = clickUnitInfo._movePos;
                //    clickUnitInfo.arriveFlag.gameObject.SetActive(true);
                //    print("계단");
                //}
                //else
                flagTr.position = transform.position;
                clickUnitInfo._movePos = transform.position;
                clickUnitInfo.arriveFlag.position = clickUnitInfo._movePos;
                clickUnitInfo.arriveFlag.gameObject.SetActive(true);

                //{

                //}
                textMeshProUGUI.text = $"{hit.point}\n{transform.position}";
            }


            _flag_Material.color = Color.blue;

            
            clickUnitInfo._enum_Unit_Action_State = eUnit_Action_States.unit_Move;

            textMeshProUGUI.text = $"드래그끝\n유닛이 이동할 좌표 : {transform.position}";

            clickUnitInfo.arriveFlag.SetParent(emptyParent);

            

            //clickUnitInfo._isClick = false;
            clickUnitInfo = null;
            //GameObject clone = Instantiate(flagTr.gameObject, transform.position,Quaternion.identity);
            //Destroy(clone, 7f);
            flagTr.gameObject.SetActive(false);
            
            isChoice = false;
            isMove = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //StartCoroutine(FlagAnim());

    }
}
