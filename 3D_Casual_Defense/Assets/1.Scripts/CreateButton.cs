using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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


    [SerializeField]
    private Image btnImage;

    [SerializeField]
    private Sprite[] actSprites;


    private void Awake()
    {
        spawnPointCs= spawnPoint.GetComponent<PlayerUnitSpawnPoint>();
        btnImage=GetComponent<Button>().image;
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
        btnImage.sprite = actSprites[1];
        spawnPoint.gameObject.SetActive(true);

        //unitFactory.playerUnit = unitFactory.RedayUnit(playerUnitId);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        btnImage.sprite = actSprites[0];

        if (spawnPointCs.canCreate)
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
