using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class CreateButton_1 : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private Player player;


    [SerializeField]
    private CreatePlayerUnit _unit;

    [SerializeField]
    private PlayerUnitClass playerUnit;


    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            player.isMove = true;

            playerUnit.transform.position = new Vector3(hit.point.x, 0f, hit.point.z);
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        playerUnit = _unit.CreateKnight();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        playerUnit = null;
        player.isMove = false;


    }
}
