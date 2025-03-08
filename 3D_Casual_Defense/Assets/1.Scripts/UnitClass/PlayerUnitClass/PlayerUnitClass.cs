using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using static UnitDataManager;
using Unity.VisualScripting;

public abstract class PlayerUnitClass : UnitInfo
{
    [Header("플레이어 유닛 홀드 상태 시 활성화되는 네모박스 프리팹")]
    public GameObject holdObPref;

    [Header("플레이어 유닛 홀드 상태 시 활성화되는 네모박스 게임 오브젝트")]
    public GameObject holdOb=null;

    [Header("플레이어 유닛 홀드 상태 시 필요한 네비메쉬 옵스태클")]
    public NavMeshObstacle navObs;

    [Header("유닛을 클릭했는지 확인하는 변수")]
    public bool _isClick;       // 유닛을 클릭했는지 확인하는 변수

    [SerializeField]
    public float changeTime = 0f;
    
    public Transform arriveFlag;

    [Header("모드 전환 시 활성화 되는 이펙트")]
    public GameObject chageModeVfx;

    [Header("이동 모드 전환 시 활성화 되는 이미지")]
    public GameObject moveImg;

    [Header("홀드 모드 전환 시 활성화 되는 이미지")]
    public GameObject tranceImg;

    public bool _isHoldeMode;

    public bool _isFreeeMode;

    [SerializeField]
    private GameObject circleObj;

    [SerializeField]
    private float rotSpeed=1200f;

    public SpriteRenderer holdObj;

    public AbsPlayerUnitFactory unitFactory;

    public abstract void InitUnitInfoSetting(CharacterData character_Data);     // 유닛 정보 초기화 시켜주는 함수

    public abstract void SetUnitValue();    // 활성화 시 필요한 초기 데이터 값 부여하는 함수

    public abstract void SetStructValue(CharacterData character_Data);  // 해당 유닛의 Json 파일 데이터들을 가져와 할당해주는 함수


    public void CheckChangeMode()
    {
        if (!isChangeState && changeTime < 3f)
        {
            holdObj.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
            circleObj.SetActive(true);
            changeTime += Time.deltaTime;
            if(_isHoldeMode&& holdObj.color.a<1f)
            {
                holdObj.gameObject.SetActive(true);
                holdObj.color += new Color(0, 0, 0, 0.005f);
                print("홀드출력");


            }
            if (_isFreeeMode && holdObj.color.a > 0f)
            {
                holdObj.color -= new Color(0, 0, 0, 0.005f);
                print(holdObj.color);
                print("출력");
            }
            circleObj.transform.Rotate(new Vector3(0, 0, rotSpeed * Time.deltaTime));
            print(_enum_Unit_Action_Mode);
        }
        else if (!isChangeState && changeTime >= 3f)
        {
            circleObj.SetActive(false);
            _isClick = false;
            canAct = true;
            isChangeState = true;
            changeTime = 0f;
            if (_isFreeeMode)
            {
                holdObj.transform.SetParent(circleObj.transform.parent);
                holdObj.gameObject.SetActive(false);
                if (_nav.isOnNavMesh)
                    _nav.isStopped = false;
                _isFreeeMode = false;
            }
            else if (_isHoldeMode)
            {
                holdObj.transform.SetParent(GameObject.FindGameObjectWithTag("HoldPrefabs").transform);
            }
            holdObj.transform.rotation = Quaternion.Euler(-90f,0f,0f);

            tranceImg.SetActive(false);
            _isHoldeMode = false;
            print("모드전환");

        }
    }
    #region # Act_By_Unit() : 유닛 행동 구분지어주는 함수, IActByUnit 인터페이스 함수 정의

    public void Act_By_Unit()  // 유닛 행동 구분지어주는 함수
    {
        switch (_enum_Unit_Action_Mode) // 유닛 모드에 따라 행동
        {
            case eUnit_Action_States.unit_FreeMode: // 유닛 자유 모드일 때 행동 구분

                _enum_pUnit_Action_BaseMode = eUnit_Action_States.unit_FreeMode;

                Act_FreeMode(); // 자유모드일 때 호출되는 함수
                break;

            case eUnit_Action_States.unit_HoldMode:

                _enum_pUnit_Action_BaseMode = eUnit_Action_States.unit_HoldMode;

                Act_HoldMode(); // 홀드모드일 때 호출되는 함수

                break;
        }
    }
    #endregion  IActByUnit 함수 

    #region # Act_FreeMode() : 플레이어 유닛이 자유모드일 때 호출되는 함수, 구현된 행동 : 대기(탐지), 이동, 추적, 공격
    private void Act_FreeMode()
    {
        navObs.enabled = false;
        if (_nav.enabled==false)
            _nav.enabled = true;

        switch (_enum_Unit_Action_State)     // 현재 유닛 행동
        {
            case eUnit_Action_States.unit_Idle: // 유닛 대기 상태(탐지 상태)

                _anim.SetBool("isMove", false);

                if (!_isSearch)  // 적 탐지 못했을 때만 실행
                {
                    actUnitCs.SearchTarget(target_Search_Type: _eUnit_Target_Search_Type);
                }
                break;

            case eUnit_Action_States.unit_Move: // 유닛 이동

                // 탐지한 타겟 비워주기
                _isSearch = false;
                unitTargetSearchCs._targetUnit = null;
                
                //

                _nav.isStopped = false;

                _anim.SetBool("isMove", true);   // 걷는 모션 애니메이션 실행

                
                actUnitCs.MoveUnit(_movePos);   // 목표지점으로 이동하는 함수 호출

                float distance = Vector3.Distance(transform.position, _movePos);

                if(distance <= 2f)
                {
                    moveImg.SetActive(false);
                    _isClick = false;
                    arriveFlag.transform.SetParent(transform);
                    arriveFlag.gameObject.SetActive(false);
                }
                break;

            case eUnit_Action_States.unit_Tracking: // 유닛이 몬스터 추적
                if (unitTargetSearchCs._targetUnit != null)
                {
                    actUnitCs.TrackingTarget(next_ActionState: eUnit_Action_States.unit_Tracking);
                }
                break;

            case eUnit_Action_States.unit_Attack:   // 유닛이 몬스터 공격
                if (unitTargetSearchCs._targetUnit!=null)
                {
                    actUnitCs.Attack_Unit(eUnit_Action_States.unit_Tracking);
                }
                break;

        }
    }
    #endregion
    public IEnumerator Change_PlayerState(eUnit_Action_States next_Action_State)
    {
        _enum_Unit_Action_State = next_Action_State;

        yield return new WaitForSeconds(0.5f);
    }
    #region # Act_HoldMode() : 플레이어 유닛이 홀드모드일 때 호출되는 함수, 구현된 행동 : 대기(탐지), 공격, 홀드(제자리 경계)
    private void Act_HoldMode()
    {
        
        _nav.enabled = false;
        navObs.enabled = true;

        switch (_enum_Unit_Action_State)
        {
            case eUnit_Action_States.unit_Idle: // 유닛 대기 상태
                _anim.SetBool("isMove", false);
                if (!_isSearch)  // 적 탐지 못했을 때만 실행
                {
                    actUnitCs.SearchTarget(target_Search_Type : _eUnit_Target_Search_Type);
                }
                break;

            case eUnit_Action_States.unit_Attack:   // 유닛이 몬스터 공격
                if (unitTargetSearchCs._targetUnit != null)
                {
                    actUnitCs.Attack_Unit(eUnit_Action_States.unit_Boundary);
                }
                break;

            case eUnit_Action_States.unit_Boundary: // 유닛 홀드(제자리 경계)
                if (unitTargetSearchCs._targetUnit != null)
                {
                    actUnitCs.Boundary();
                }
                break;
        }
    }
    #endregion
}

