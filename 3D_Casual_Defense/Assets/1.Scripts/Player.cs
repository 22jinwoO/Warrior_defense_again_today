using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private CreatePlayerUnit clickUnitCs;

    [SerializeField]
    private UnitInfo clickUnitInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.tag=="Player")
                {
                    print("유닛 지정 완료!");
                    hit.transform.GetComponent<UnitInfo>()._isClick = true;
                    clickUnitCs.clikUnitInfo= hit.transform.GetComponent<UnitInfo>();
                }
                //_movePos = hit.point;
                //_enum_Unit_Action_State = eUnit_Action_States.unit_Move;
            }
        }
    }
}
