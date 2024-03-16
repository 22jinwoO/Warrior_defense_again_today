using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateButton : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private Player player;

    public int btnIndex;

    [SerializeField]
    private CreatePlayerUnit unitFactory;

    public string playerUnitId;

    public Transform spawnPoint;

    [SerializeField]
    private PlayerUnitSpawnPoint spawnPointCs;

    private void Awake()
    {
        spawnPointCs= spawnPoint.GetComponent<PlayerUnitSpawnPoint>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            player.isMove = true;

            spawnPoint.position = new Vector3(hit.point.x, 8.5f, hit.point.z);
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        spawnPointCs.SetBase();

        spawnPoint.gameObject.SetActive(true);

        //unitFactory.playerUnit = unitFactory.RedayUnit(playerUnitId);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(spawnPointCs.canCreate)
        {
            unitFactory.SpawnSummon(spawnPoint.position, btnIndex);


        }
        //playerUnitId = null;
        //spawnPoint = null;
        player.isMove = false;

        spawnPoint.gameObject.SetActive(false);

        //eventData.worldPosition;

    }
}
