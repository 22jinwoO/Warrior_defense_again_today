using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using EnumTypes;
using System;
using Unity.VisualScripting;

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

    private void Awake()
    {
        //audioPlayer.PlayOneShot();
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
            // 유닛 클릭 지속 시 이동상태로 전환시켜주는 구문
            if (isChoice && Input.GetMouseButton(0) && !isMove)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        times += Time.deltaTime;
                        if (times >= 0.6f)
                        {
                            isMove = true;
                            StartCoroutine(FlagAnim());
                            times = 0f;
                        }

                    }
                }
            }

            if (!isMove && Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        if (!isChoice)
                        {
                            audioPlayer.PlayOneShot(audioSources[0]);
                            clickUnitInfo = hit.transform.GetComponent<PlayerUnitClass>();

                            clickUnitInfo._isClick = true;
                            clickUnitCs.clikUnitInfo = hit.transform.GetComponent<PlayerUnitClass>();
                            //isMove = true;
                            _flag_Material = _Materials[0];

                            // 플레이어 조작 창 활성화
                            unitCtrlCanvas.transform.position = transform.position;
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
                        print("다른거 맞음");
                        isChoice = false;
                        flagTr.gameObject.SetActive(false);

                        if (clickUnitInfo != null)
                        {
                            clickUnitInfo._isClick = false;
                        }

                        clickUnitInfo = null;
                        unitCtrlCanvas.SetActive(false);

                        clickUnitCs.clikUnitInfo = null;
                        isChoice = false;
                    }
                }
            }

        }

        if (isChoice&&!isMove)
        {
            transform.position = clickUnitInfo.transform.position;
            unitCtrlCanvas.transform.position = transform.position+new Vector3(xValue,yValue,zValue);

            flagTr.position = transform.position;
        }

    }

    private IEnumerator FlagAnim()
    {
        audioPlayer.PlayOneShot(audioSources[1]);
        _flag_Material.color = Color.white;

        // 유닛 설정 팝업창 비활성화
        unitCtrlCanvas.SetActive(false);

        float value = 4f;
        flagTr.localScale = new Vector3(value, 0f, value);
        while (value>1.5f)
        {
            flagTr.localScale = new Vector3(value, 0f, value);
            value -= 0.1f;
            yield return null;

        }

    }

    public float times = 0f;
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

            

            clickUnitInfo._isClick = false;
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
