using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateButton : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("플레이어 스크립트")]
    [SerializeField]
    private Player player;

    [Header("버튼 인덱스 값")]
    public int btnIndex;

    [Header("유닛 팩토리")]
    [SerializeField]
    private CreatePlayerUnit unitFactory;

    [Header("유닛 ID")]
    public string playerUnitId;

    [Header("유닛 팩토리")]
    public Transform spawnPoint;

    [Header("플레이어 유닛 스폰포인트 스크립트")]
    [SerializeField]
    private PlayerUnitSpawnPoint spawnPointCs;

    [Header("버튼이미지")]
    [SerializeField]
    private Image btnImage;

    [Header("슬롯 버튼 이미지")]
    [SerializeField]
    private Sprite[] actSprites;


    private void Awake()
    {
        spawnPointCs= spawnPoint.GetComponent<PlayerUnitSpawnPoint>();
        btnImage=GetComponent<Button>().image;
    }

    // 드래그로 유닛을 스폰시킬 생성 위치 지정
    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            player.isMove = true;

            spawnPoint.position = new Vector3(hit.point.x, 8.5f, hit.point.z);
        }

    }

    // 드래그 시작 시 생성 위치 오브젝트 활성화 및 버튼 클릭 이미지 변경
    public void OnBeginDrag(PointerEventData eventData)
    {
        spawnPointCs.SetBase();
        btnImage.sprite = actSprites[1];
        spawnPoint.gameObject.SetActive(true);
    }

    // 드래그 끝났을 때 버튼 이미지 전환, 생성 위치에 소환진 오브젝트 생성
    public void OnEndDrag(PointerEventData eventData)
    {
        btnImage.sprite = actSprites[0];

        if (spawnPointCs.canCreate)
        {
            unitFactory.SpawnSummon(spawnPoint.position, btnIndex);
        }
        player.isMove = false;

        spawnPoint.gameObject.SetActive(false);
    }
}
