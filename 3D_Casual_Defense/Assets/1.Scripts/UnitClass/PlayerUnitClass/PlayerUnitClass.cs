using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public abstract class PlayerUnitClass : UnitInfo
{
    [Header("플레이어 유닛 홀드 상태 시 활성화되는 네모박스 오브젝트")]
    public GameObject holdOb;

    [Header("플레이어 유닛 홀드 상태 시 필요한 네비메쉬 옵스태클")]
    public NavMeshObstacle navObs;

    [Header("유닛의 타겟 선정 타입마다 필요한 함수들을 구현해놓은 UnitTargetSearch 스크립트")]
    [SerializeField]
    protected UnitTargetSearch unitTargetSearchCs;

    [Header("유닛의 행동들을 구현해놓은 ActUnit 스크립트")]
    [SerializeField]
    protected ActUnit actUnitCs;

    public abstract void InitUnitInfoSetting();     // 유닛 정보 초기화 시켜주는 함수

    #region # Act_By_Unit() : 유닛 행동 구분지어주는 함수, IActByUnit 인터페이스 함수 정의

    public void Act_By_Unit()  // 유닛 행동 구분지어주는 함수
    {
        switch (_enum_Unit_Action_Mode) // 유닛 모드에 따라 행동
        {
            case eUnit_Action_States.unit_FreeMode: // 유닛 자유 모드일 때 행동 구분
                Act_FreeMode(); // 자유모드일 때 호출되는 함수
                break;

            case eUnit_Action_States.unit_HoldMode:         
                Act_HoldMode(); // 홀드모드일 때 호출되는 함수
                break;
        }
    }

    #endregion  IActByUnit 함수 

    #region # Act_FreeMode() : 플레이어 유닛이 자유모드일 때 호출되는 함수, 구현된 행동 : 대기(탐지), 이동, 추적, 공격
    private void Act_FreeMode()
    {
        holdOb.SetActive(false);
        navObs.enabled = false;
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
                _isSearch = false;
                _isClick = false;
                unitTargetSearchCs._targetUnit = null;
                _nav.isStopped = false;
                _anim.SetBool("isMove", true);   // 걷는 모션 애니메이션 실행
                                                 //unitTargetSearchCs._unitModelTr.LookAt(_movePos);
                actUnitCs.MoveUnit(_movePos);
                break;

            case eUnit_Action_States.unit_Tracking: // 유닛이 몬스터 추적
                actUnitCs.TrackingTarget(next_ActionState: eUnit_Action_States.unit_Tracking);
                break;

            case eUnit_Action_States.unit_Attack:   // 유닛이 몬스터 공격
                actUnitCs.ReadyForAttack(unit_Atk_State: eUnit_Action_States.unit_Tracking);
                break;

        }
    }
    #endregion

    #region # Act_HoldMode() : 플레이어 유닛이 홀드모드일 때 호출되는 함수, 구현된 행동 : 대기(탐지), 공격, 홀드(제자리 경계)
    private void Act_HoldMode()
    {
        holdOb.SetActive(true);
        //holdOb.transform.localRotation = Quaternion.Euler(Vector3.zero);
        navObs.enabled = true;
        _nav.enabled = false;

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
                actUnitCs.ReadyForAttack(unit_Atk_State: eUnit_Action_States.unit_Boundary);
                break;

            case eUnit_Action_States.unit_Boundary: // 유닛 홀드(제자리 경계)
                actUnitCs.Boundary();
                break;
        }
    }
    #endregion
}

