using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreatePlayerUnit : MonoBehaviour
{
    [Header("플레이어 유닛 팩토리들")]
    [SerializeField]
    public AbsPlayerUnitFactory[] playerUnitFactorys;

    [Header("기사버튼")]
    [SerializeField]
    private Button clickKnightBtn;

    [Header("궁수버튼")]
    [SerializeField]
    private Button clickArcherBtn;

    [Header("유닛자유모드")]
    [SerializeField]
    private Button clickUnitFreeBtn;

    [Header("자유모드 이미지")]
    public Image clickUnitFreeBtnImg;

    [Header("유닛홀드모드")]
    [SerializeField]
    private Button clickUnitHoldBtn;

    [Header("홀드모드 이미지")]
    public Image clickUnitHoldBtnImg;

    [Header("클리한 플레이어 유닛")]
    public PlayerUnitClass clickUnitInfo;

    [Header("UI 팝업매니저 스크립트")]
    [SerializeField]
    private UI_PopUpManager popUpMgr;

    [Header("유닛생산하는 버튼들")]
    [SerializeField]
    private CreateButton[] CreateBtns;

    [Header("유닛 아이디 문자형 변수")]
    [SerializeField]
    private List<string> unitIds;

    [Header("캔버스 변수")]
    [SerializeField]
    private Canvas canvas;

    [Header("렉트트랜스폼 변수")]
    [SerializeField]
    private RectTransform canvasRectTr;

    [Header("스폰이펙트 게임오브젝트")]
    [SerializeField]
    private GameObject summonPortal;

    [Header("uiCam 카메라")]
    [SerializeField]
    private Camera uiCam;

    [Header("screenPoint 변수")]
    [SerializeField]
    private Vector2 screenPoint;

    [Header("딕셔너리 (키 : 플레이어 아이디 / 값 : 플레이어 유닛 클래스)")]
    [SerializeField]
    private SerializableDictionary<string, PlayerUnitClass> dicplayerUnits;

    [Header("플레이어 스크립트")]
    [SerializeField]
    private Player playerCs;

    [Header("오디오 소스")]
    [SerializeField]
    private AudioSource audioPlayer;

    [Header("오디오 클립")]
    [SerializeField]
    private AudioClip[] audioSources;

    [Header("홀드 이미지 버튼들")]
    public Sprite[] holdImgs;

    [Header("자유 이미지 버튼들")]
    public Sprite[] freeImgs;
    

    private void Awake()
    {
        // 자유모드 / 홀드모드
        clickUnitFreeBtn.onClick.AddListener(()=> StartCoroutine(ClickUnitFree()));
        clickUnitHoldBtn.onClick.AddListener(() => StartCoroutine(ClickUnitHold()));

        // 자유모드 이미지 / 홀드모드 이미지
        clickUnitFreeBtnImg = clickUnitFreeBtn.GetComponent<Image>();
        clickUnitHoldBtnImg = clickUnitHoldBtn.GetComponent<Image>();

        // AudioSource 로 변수 가져오기
        if (TryGetComponent(out AudioSource audioSource))
        {
            audioPlayer = audioSource;
        }

        // _unitInfo_Dictionary.Keys 키 값들 할당
        unitIds = new List<string>(UnitDataManager.Instance._unitInfo_Dictionary.Keys);

        // CreateBtns[i] 의 플레이어 유닛 ID, 버튼 인덱스 할당
        for (int i = 0; i < CreateBtns.Length; i++)
        {
            CreateBtns[i].playerUnitId = unitIds[i];
            CreateBtns[i].btnIndex = i;
        }
    }

    #region # SpawnSummon() : 소환 이펙트 유닛 생산지점에 생성하는 함수
    // SpawnSummon 소환 이펙트 유닛 생산지점에 생성하는 함수
    public void SpawnSummon(Vector3 Pos,int btnIndex)
    {
        audioPlayer.PlayOneShot(audioSources[2]);


        // 포탈 스폰위치에 복사 후 생성
        GameObject portal =Instantiate(summonPortal, Pos+new Vector3(0f,0.5f,0f),Quaternion.Euler(-90f,0f,0f));
        SummonUnit summonCs = portal.GetComponent<SummonUnit>();
        summonCs.unitFactory = this;
        summonCs.btnIndex = btnIndex;
        summonCs.unitTr = Pos;
        StartCoroutine(summonCs.CreateUnit());

        // 터치한 위치 반환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTr, Input.mousePosition, uiCam, out screenPoint);
    }
    #endregion

    #region # ClickUnitFree() : 클릭한 유닛 자유모드로 변환하는 함수
    private IEnumerator ClickUnitFree()
    {
        if (clickUnitInfo==null ||!clickUnitInfo._isClick)
        {
            yield return null;
        }

        // 이미지 전환
        clickUnitFreeBtnImg.sprite = freeImgs[1];

        yield return new WaitForSecondsRealtime(0.05f);

        // 이미지 전환
        clickUnitFreeBtnImg.sprite = freeImgs[0];

        // 사운드 실행
        audioPlayer.PlayOneShot(audioSources[1]);

        // 클릭한 유닛 컨트롤 UI 비활성화
        playerCs.unitCtrlCanvas.SetActive(false);

        print("클릭한 유닛 자유모드");

        // 클릭한 유닛 행동 불가능
        clickUnitInfo.canAct =false;

        // 클릭한 유닛의 모드 전환 이미지 활성화
        clickUnitInfo.tranceImg.SetActive(true);

        clickUnitInfo._isFreeeMode = true;

        // 모드 전환 변수 false로 전환
        clickUnitInfo.isChangeState = false;

        // 클릭한 유닛의 모드 전환시간 0으로 할당
        clickUnitInfo.changeTime = 0f;

        // 클릭한 유닛의 _isClick 변수 true로 전환
        clickUnitInfo._isClick = true;

        // 클릭한 유닛의 모드 자유모드로 전환
        clickUnitInfo._enum_Unit_Action_Mode = eUnit_Action_States.unit_FreeMode;

        // 클릭한 유닛의 상태 Idle 상태로 전환
        clickUnitInfo._enum_Unit_Action_State = eUnit_Action_States.unit_Idle;

        // 클릭한 유닛의 탐지 변수 false로 변환
        clickUnitInfo._isSearch = false;

        clickUnitInfo.gameObject.name = clickUnitInfo._unitData.char_id;

        // 클릭한 유닛의 기본 공격 상태 추적 상태로 변환
        clickUnitInfo._enum_Unit_Attack_State=eUnit_Action_States.unit_Tracking;

        playerCs.isChoice = false;
        playerCs.isMove = false;
        playerCs.flagTr.gameObject.SetActive(false);

        // 클릭한 유닛 변수 null로 변환
        clickUnitInfo = null;
    }
    #endregion


    #region # ClickUnitHold() : 클릭한 유닛 홀드모드로 변환하는 함수
    private IEnumerator ClickUnitHold()
    {
        if (clickUnitInfo == null||!clickUnitInfo._isClick)
        {
            yield return null;
        }

        // 클릭한 유닛 자기 위치로 이동
        clickUnitInfo._nav.SetDestination(clickUnitInfo.transform.position);

        // 이미지 전환
        clickUnitHoldBtnImg.sprite = holdImgs[1];

        yield return new WaitForSecondsRealtime(0.05f);

        // 이미지 전환 
        clickUnitHoldBtnImg.sprite = holdImgs[0];

        // 사운드 실행
        audioPlayer.PlayOneShot(audioSources[0]);

        // 클릭한 유닛 컨트롤 UI 비활성화
        playerCs.unitCtrlCanvas.SetActive(false);

        clickUnitInfo._nav.SetDestination(clickUnitInfo.transform.position);


        // 클릭한 유닛 행동 불가능
        clickUnitInfo.canAct = false;

        // 클릭한 유닛의 _isClick 변수 true로 전환
        clickUnitInfo._isClick = true;

        // 클릭한 유닛의 모드 전환 이미지 활성화
        clickUnitInfo.tranceImg.SetActive(true);

        clickUnitInfo._isHoldeMode = true;

        // 모드 전환 변수 false로 전환
        clickUnitInfo.isChangeState = false;

        // 클릭한 유닛의 모드 전환시간 0으로 할당
        clickUnitInfo.changeTime = 0f;

        print("클릭한 유닛 홀드모드");

        // 클릭한 유닛의 모드 홀드모드로 전환
        clickUnitInfo._enum_Unit_Action_Mode = eUnit_Action_States.unit_HoldMode;

        // 클릭한 유닛의 상태 Idle 상태로 전환
        clickUnitInfo._enum_Unit_Action_State = eUnit_Action_States.unit_Idle;

        // 클릭한 유닛의 탐지 변수 false로 변환
        clickUnitInfo._isSearch = false;

        // 클릭한 유닛의 모드 탐지모드로 전환
        clickUnitInfo._enum_Unit_Attack_State = eUnit_Action_States.unit_Boundary;


        clickUnitInfo.gameObject.name = clickUnitInfo._unitData.char_id;

        // 
        playerCs.isChoice = false;

        // 
        playerCs.isMove = false;

        // 
        playerCs.flagTr.gameObject.SetActive(false);

        // 
        clickUnitInfo = null;
    }
    #endregion

}
