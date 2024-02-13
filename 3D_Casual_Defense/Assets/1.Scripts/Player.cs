using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class Player : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler
{
    [SerializeField]
    private CreatePlayerUnit clickUnitCs;

    [SerializeField]
    private UnitInfo clickUnitInfo;

    [SerializeField]
    private Transform flagTr;

    [SerializeField]
    public TextMeshProUGUI textMeshProUGUI;

    public bool isChoice;
    public bool isMove;
    // Update is called once per frame
    void Update()
    {
        //float distance = Camera.main.WorldToScreenPoint(transform.position).z;

        //Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        //Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

        ////objPos.z = 0;
        //objPos.x = 0;
        //transform.position = objPos;
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out RaycastHit hit))
        //    {
        //        if (hit.transform.tag=="Player")
        //        {
        //            textMeshProUGUI.text = hit.transform.name+"지정 완료!";
        //            print("유닛 지정 완료!");
        //            hit.transform.GetComponent<UnitInfo>()._isClick = true;
        //            clickUnitCs.clikUnitInfo= hit.transform.GetComponent<UnitInfo>();
        //        }
        //    }
        //}
        if (!isChoice&&Input.touchCount==1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.tag == "Player")
                {
                    clickUnitInfo = hit.transform.GetComponent<UnitInfo>();
                    transform.position = clickUnitInfo.transform.position;
                    hit.transform.GetComponent<UnitInfo>()._isClick = true;
                    clickUnitCs.clikUnitInfo = hit.transform.GetComponent<UnitInfo>();
                    //isMove = true;
                    isChoice = true;
                    flagTr.position = transform.position;
                    flagTr.gameObject.SetActive(true);

                    textMeshProUGUI.text = hit.transform.name + "지정 완료!\n" + "사용자 지정 가능여부 " + isChoice + "\n 유닛이동 가능 여부 " + hit.transform.GetComponent<UnitInfo>()._isClick;

                }
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isChoice)
        {
            isMove = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                transform.position = new Vector3(hit.point.x, 0f, hit.point.z);
                flagTr.position = transform.position;
                textMeshProUGUI.text = $"{hit.point}\n{transform.position}";
            }

            //Vector3 mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //transform.position = new Vector3(mouseInput.x, 0f, mouseInput.y);
            //textMeshProUGUI.text = $"드래그중\n유닛이 이동할 좌표 : {mouseInput}";
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isChoice&& isMove)
        {
            clickUnitInfo._movePos = transform.position;
            clickUnitInfo._enum_Unit_Action_State = eUnit_Action_States.unit_Move;

            textMeshProUGUI.text = $"드래그끝\n유닛이 이동할 좌표 : {transform.position}";

            clickUnitInfo._isClick = false;
            clickUnitInfo = null;
            flagTr.gameObject.SetActive(false);
            
            isChoice = false;
            isMove = false;
        }
    }

}
