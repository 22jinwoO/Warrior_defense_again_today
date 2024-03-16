using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateButton_2 : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private CreatePlayerUnit _unit;

    [SerializeField]
    private PlayerUnitClass playerUnit;

    [SerializeField]
    private PlayerUnitSpawnPoint spawnPointCs;

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
        playerUnit = _unit.CreateArcher();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        player.isMove = false;

        playerUnit = null;

    }
}
