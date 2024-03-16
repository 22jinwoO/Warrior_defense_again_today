using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnitDataManager;

public class CreatePlayerUnit : MonoBehaviour
{
    [SerializeField]
    public AbsPlayerUnitFactory[] playerUnitFactorys;

    [SerializeField]
    private Button clickKnightBtn;

    [SerializeField]
    private Button clickArcherBtn;

    [SerializeField]
    private Button clickUnitFreeBtn;

    [SerializeField]
    private Button clickUnitHoldBtn;


    public PlayerUnitClass clikUnitInfo;

    [SerializeField]
    private UI_PopUpManager popUpMgr;

    [SerializeField]
    private CreateButton[] CreateBtns;

    [SerializeField]
    private List<string> unitIds;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private RectTransform canvasRectTr;



    public PlayerUnitClass[] playerUnits;


    [SerializeField]
    private GameObject summonUnit;

    [SerializeField]
    private GameObject summonPortal;


    [SerializeField]
    private Camera uiCam;

    private Vector2 screenPoint;

    [SerializeField]
    private SerializableDictionary<string, PlayerUnitClass> dicplayerUnits;

    [SerializeField]
    private Player playerCs;


    [SerializeField]
    private AudioSource audioPlayer;


    [SerializeField]
    private AudioClip[] audioSources;



    private void Awake()
    {
        clickUnitFreeBtn.onClick.AddListener(ClickUnitFree);
        clickUnitHoldBtn.onClick.AddListener(ClickUnitHold);

        print(UnitDataManager.Instance._unitInfo_Dictionary.Keys);

        if (TryGetComponent(out AudioSource audioSource))
        {
            audioPlayer = audioSource;
        }




        unitIds = new List<string>(UnitDataManager.Instance._unitInfo_Dictionary.Keys);

        for (int i = 0; i < CreateBtns.Length; i++)
        {
            CreateBtns[i].playerUnitId = unitIds[i];
            CreateBtns[i].btnIndex = i;
        }

        // 궁수 데이터 키, 값 할당
        dicplayerUnits.Add(key: unitIds[0], value: playerUnits[0]);

        // 기사 데이터 키, 값 할당
        dicplayerUnits.Add(key: unitIds[1], value: playerUnits[1]);

    }


    //클래스마다 생산될 유닛을 결정해주는 구상 생산자
    public PlayerUnitClass RedayUnit(string unitId)
    {
        PlayerUnitClass playerUnit = null;

        playerUnit = dicplayerUnits[unitId];

        return playerUnit;
    }

    // 드래그 끝났을 때 호출되도록?
    public PlayerUnitClass CreateUnit()
    {
        print("유닛생산");

        // 생산자 실행
        PlayerUnitClass unit = playerUnitFactorys[0].CreatePlayerUnit();

        //unit.InitUnitInfoSetting();

        //popUpMgr.isUseShop = false;

        //// 유닛 생산 팝업창 닫기
        //StartCoroutine(popUpMgr.UseUnitPopUp());
        return unit;
    }


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
        //
        // 셔먼 게이지 오브젝트 복사 생성
        // summon = Instantiate(summonUnit, Pos+Vector3.up*10f,Quaternion.Euler(50f,0f,0f));
        //summon.transform.position = Pos;

        //summon.transform.position = Pos;

        //summonCs.portal = portal.transform;
        //summon.transform.SetParent(canvas.transform);
        //summon.transform.localPosition = screenPoint - new Vector2(0, 30f);
        //summonCs.myPos = screenPoint - new Vector2(0, 30f);

        //, screenPoint, Quaternion.identity,canvas.transform
    }


    // 드래그 끝났을 때 호출되도록?
    //public GameObject CreateUnit()
    //{
    //    // 소환진 활성화

    //    // 대기시간 다 지나면

    //    // 반환된 유닛 활성화

    //    // 기사 유닛 생산자
    //    //playerUnitFactorys[0].knightClass = AbsPlayerUnitFactory.KnightClass.Knight;

    //    // 생산자 실행
    //    //GameObject unit = CreatePlayerUnit();

    //    //print(UnitDataManager.Instance._unitInfo_Dictionary[unit._unitData.unit_Id]);
    //    //Vector3 setPos = new Vector3(Input.mousePosition.x, 0f, Input.mousePosition.z);
    //    //unit.transform.position = setPos;
    //    GameObject unit = Instantiate(RedayUnit("sd").gameObject);
    //    unit.gameObject.name = "기사";

    //    popUpMgr.isUseShop = false;

    //    // 유닛 생산 팝업창 닫기
    //    StartCoroutine(popUpMgr.UseUnitPopUp());
    //    return unit;
    //}



    // 드래그 끝났을 때 호출되도록?
    public PlayerUnitClass CreateKnight()
    {
        print("기사생산");

        // 소환진 활성화

        // 대기시간 다 지나면

        // 반환된 유닛 활성화


        // 기사 유닛 생산자
        playerUnitFactorys[0].knightClass = AbsPlayerUnitFactory.KnightClass.Knight;

        // 생산자 실행
        PlayerUnitClass knight = playerUnitFactorys[0].CreatePlayerUnit();

        //print(UnitDataManager.Instance._unitInfo_Dictionary[unit._unitData.unit_Id]);
        //Vector3 setPos = new Vector3(Input.mousePosition.x, 0f, Input.mousePosition.z);
        //unit.transform.position = setPos;

        knight.gameObject.name = "기사"+ knight._unitData.char_id;

        popUpMgr.isUseShop = false;
        
        // 유닛 생산 팝업창 닫기
        StartCoroutine(popUpMgr.UseUnitPopUp());
        return knight;
    }

    public PlayerUnitClass CreateArcher()
    {
        print("궁수생산");
        // 궁수 유닛 생산자
        playerUnitFactorys[1].archerClass = AbsPlayerUnitFactory.ArcherClass.Archer;

        // 생산자 실행
        PlayerUnitClass Archor = playerUnitFactorys[1].CreatePlayerUnit();
        //Archor.transform.position = Vector3.zero;
        Archor.gameObject.name = "궁수";
        popUpMgr.isUseShop = false;
        StartCoroutine(popUpMgr.UseUnitPopUp());
        return Archor;

    }

    private void ClickUnitFree()
    {
        if (clikUnitInfo==null)
        {
            return;
        }
        audioPlayer.PlayOneShot(audioSources[1]);
        playerCs.unitCtrlCanvas.SetActive(false);
        print("클릭한 유닛 자유모드");
        clikUnitInfo.changeTime = 0f;

        clikUnitInfo._enum_Unit_Action_Mode = eUnit_Action_States.unit_FreeMode;
        clikUnitInfo._enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
        //likUnitInfo.transform.position = initPos;
        clikUnitInfo._isSearch = false;
        clikUnitInfo._enum_Unit_Attack_State=eUnit_Action_States.unit_Tracking;
        //clikUnitInfo.transform.position = initPos;

        playerCs.isChoice = false;
        playerCs.isMove = false;
        playerCs.flagTr.gameObject.SetActive(false);


        clikUnitInfo._isClick = false;
        clikUnitInfo = null;
    }

    private void ClickUnitHold()
    {

        if (clikUnitInfo == null)
        {
            return;
        }

        audioPlayer.PlayOneShot(audioSources[0]);

        playerCs.unitCtrlCanvas.SetActive(false);
        clikUnitInfo.changeTime = 0f;

        print("클릭한 유닛 홀드모드");
        clikUnitInfo._enum_Unit_Action_Mode = eUnit_Action_States.unit_HoldMode;
        clikUnitInfo._enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
        clikUnitInfo._isSearch = false;
        clikUnitInfo._enum_Unit_Attack_State = eUnit_Action_States.unit_Boundary;
        //initPos = clikUnitInfo.transform.position;
        playerCs.isChoice = false;
        playerCs.isMove = false;
        playerCs.flagTr.gameObject.SetActive(false);
        clikUnitInfo._isClick = false;
        clikUnitInfo = null;
    }

}
