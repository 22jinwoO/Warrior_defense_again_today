using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField]
    private CreatePlayerUnit clickUnitCs;

    [SerializeField]
    private UnitInfo clickUnitInfo;

    [SerializeField]
    public TextMeshProUGUI textMeshProUGUI;

    public bool isChoice;
    // Update is called once per frame
    void Update()
    {
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
        if (!isChoice&&Input.touchCount == 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.tag == "Player")
                {
                    hit.transform.GetComponent<UnitInfo>()._isClick = true;
                    clickUnitCs.clikUnitInfo = hit.transform.GetComponent<UnitInfo>();
                    isChoice = true;
                    textMeshProUGUI.text = hit.transform.name + "지정 완료!\n" + "사용자 지정 가능여부 " + isChoice + "\n 유닛이동 가능 여부 " + hit.transform.GetComponent<UnitInfo>()._isClick;

                }
            }
        }
    }
}
