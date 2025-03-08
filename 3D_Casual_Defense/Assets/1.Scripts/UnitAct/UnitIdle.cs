using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdle : MonoBehaviour, IUnitActState
{
    [SerializeField]
    private NewUnitInfo unitInfoCs;

    [SerializeField]
    private UnitBaseController contorllerCs;

    [SerializeField]
    private AbsSearchTarget searchTargetCs;


    public void Enter()
    {
        unitInfoCs = GetComponent<NewUnitInfo>();
        contorllerCs = GetComponent<UnitBaseController>();
        searchTargetCs = GetComponent<AbsSearchTarget>();
        searchTargetCs._actStateCs = this;
    }

    public void DoAction()
    {
        searchTargetCs.SearchTarget();
    }


    public void Exit()
    {
        Destroy(this);
    }

    private void OnDestroy()
    {
        Debug.Log($"{this} 파괴2");
        contorllerCs.unitActState = null;
        contorllerCs.actionState = eUnit_Action_States.unit_Tracking;
    }
}
