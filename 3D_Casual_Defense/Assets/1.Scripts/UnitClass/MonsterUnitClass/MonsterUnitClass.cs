using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnitDataManager;

public abstract class MonsterUnitClass : UnitInfo
{
    [Header("몬스터 유닛 머태리얼")]
    public Material mosterUnitMtr;

    [SerializeField]
    private bool isChangeState = true; // 상태 변환 체크하는 변수

    public Transform castleTr; // 성 트랜스폼

    [Header("몬스터 대기 시간")]
    [SerializeField]
    private float delayTime;    // 대기 시간

    [SerializeField]
    private float resetWayTime;    // 경로 초기화 시간

    public abstract void InitUnitInfoSetting(CharacterData character_Data);     // 유닛 정보 초기화 시켜주는 함수

    public NavMeshPath path;

    #region # Act_By_Unit() : 유닛 행동 구분지어주는 함수
    public void Act_By_Unit()
    {
        switch (_enum_Unit_Action_Mode) // 유닛 모드에 따라 행동
        {
            case eUnit_Action_States.monster_NormalPhase: // 몬스터 이동 모드일 때 행동
                Act_NormalPhase();
                break;

            case eUnit_Action_States.monster_AngryPhase:
                Act_AngryPhase();
                break;

            case eUnit_Action_States.monster_AttackCastlePhase:
                Act_Atk_CastlePhase();
                break;
        }
    }
    #endregion

    //#region # Act_NormalMode() : 몬스터가 일반 모드일 때 호출되는 함수 , 구현된 행동 : 대기(탐지), 이동(성), 추적, 공격
    //private void Act_NormalPhase()
    //{
    //    switch (_enum_Unit_Action_State)     // 현재 유닛 행동
    //    {
    //        case eUnit_Action_States.unit_Move: // 유닛 이동

    //            if (isChangeState)  // 상태가 변환됐을 때
    //            {
    //                _isSearch = false;


    //                unitTargetSearchCs._targetUnit = null;
    //                print("애니메이션 실행");
    //                float time = 0f;

    //                while (time < 0.3f)
    //                {
    //                    time += Time.deltaTime;
    //                }
    //                isChangeState = false;
    //            }

    //            //_nav.SetDestination(castleTr.position); // 성으로 이동

    //            //unitTargetSearchCs._unitModelTr.LookAt(castleTr.position);
    //            //unitTargetSearchCs._unitModelTr.localRotation = Quaternion.Euler(0f, transform.rotation.y, transform.rotation.z);
    //            //_nav.isStopped = false;

    //            resetWayTime += Time.deltaTime;
    //            while (resetWayTime >= 3f)
    //            {
    //                for (int i = 0; i < Castle.Instance.caslteModels.Length; i++)
    //                {
    //                    float baseDistance = Vector3.Distance(transform.position, castleTr.position);
    //                    float distance = Vector3.Distance(transform.position, Castle.Instance.caslteModels[i].position);

    //                    if (distance < baseDistance)
    //                    {
    //                        castleTr = Castle.Instance.caslteModels[i];
    //                    }

    //                }
    //                path= new NavMeshPath();
    //                _nav.CalculatePath(Castle.Instance.transform.position, path);
    //                _nav.SetPath(path);
    //                if (path.status == NavMeshPathStatus.PathPartial)
    //                {
    //                    Debug.LogError("분노모드로 전환");
    //                    _enum_Unit_Action_Mode = eUnit_Action_States.monster_AngryPhase;
    //                    _enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
    //                }

    //                //_nav.ResetPath();
    //                //_nav.SetDestination(castleTr.position);
    //                resetWayTime = 0f;
    //            }

    //            _anim.SetBool("isMove", true);   // 걷는 모션 애니메이션 실행
    //            if (_nav.velocity.magnitude <= 0.3f)   // 네비 메쉬 에이전트의 이동속도가 0 이하라면
    //            {
    //                _anim.SetBool("isMove", false);
    //                delayTime += Time.deltaTime;    // 대기 시간에 타임.델타타임 더해줌

    //                if (delayTime >= 5f)  // 딜레이타임이 5초 이상 됐을 때
    //                {
    //                    if (_nav.isOnNavMesh)
    //                    {
    //                        _nav.isStopped = false;
    //                    }
    //                    _enum_Unit_Action_Mode = eUnit_Action_States.monster_AngryPhase;
    //                    _enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
    //                    delayTime = 0f;
    //                }

    //            }

    //            //
    //            if (Vector3.Distance(transform.position, castleTr.position) <= _unitData.attackRange + Castle.Instance.halfColiderValue)
    //            {
    //                _anim.SetBool("isMove", false);

    //                _enum_Unit_Action_Mode = eUnit_Action_States.monster_AttackCastlePhase;
    //                _enum_Unit_Action_State = eUnit_Action_States.unit_Attack;
    //            }

    //            break;
    //    }
    //}
    //#endregion


    #region # Act_NormalMode() : 몬스터가 일반 모드일 때 호출되는 함수 , 구현된 행동 : 대기(탐지), 이동(성), 추적, 공격
    private void Act_NormalPhase()
    {
        switch (_enum_Unit_Action_State)     // 현재 유닛 행동
        {
            case eUnit_Action_States.unit_Move: // 유닛 이동

                if (isChangeState)  // 상태가 변환됐을 때
                {
                    _isSearch = false;


                    unitTargetSearchCs._targetUnit = null;
                    print("애니메이션 실행");
                    float time = 0f;

                    while (time < 0.3f)
                    {
                        time += Time.deltaTime;
                    }
                    isChangeState = false;
                }

                //_nav.SetDestination(castleTr.position); // 성으로 이동

                //unitTargetSearchCs._unitModelTr.LookAt(castleTr.position);
                //unitTargetSearchCs._unitModelTr.localRotation = Quaternion.Euler(0f, transform.rotation.y, transform.rotation.z);
                //_nav.isStopped = false;

                resetWayTime += Time.deltaTime;
                while (resetWayTime >= 3f)
                {
                    for (int i = 0; i < Castle.Instance.caslteModels.Length; i++)
                    {
                        float baseDistance = Vector3.Distance(transform.position, castleTr.position);
                        float distance = Vector3.Distance(transform.position, Castle.Instance.caslteModels[i].position);

                        if (distance < baseDistance)
                        {
                            castleTr = Castle.Instance.caslteModels[i];
                        }

                    }

                    Debug.LogError("1"+_nav.pathEndPosition);
                    //_nav.ResetPath();

                    _nav.SetDestination(castleTr.position);

                    if (_nav.pathEndPosition.magnitude< castleTr.position.magnitude-3f)//62/64
                    {
                        Debug.LogError("2" + _nav.pathEndPosition);

                        Debug.LogError("sad" + _nav.pathEndPosition.magnitude);
                        Debug.LogError("분fa" + castleTr.position);

                        Debug.LogError("분fa" + castleTr.position.magnitude);

                        Debug.LogError("분노몸드 전환" + _nav.pathStatus);

                        _enum_Unit_Action_Mode = eUnit_Action_States.monster_AngryPhase;
                        _enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
                        delayTime = 0f;

                    }
                    Debug.LogWarning("이동전환"+_nav.pathStatus);

                    resetWayTime = 0f;
                }

                _anim.SetBool("isMove", true);   // 걷는 모션 애니메이션 실행
                //if (_nav.velocity.magnitude <= 0.3f)   // 네비 메쉬 에이전트의 이동속도가 0 이하라면
                //{
                //    _anim.SetBool("isMove", false);
                //    delayTime += Time.deltaTime;    // 대기 시간에 타임.델타타임 더해줌

                //    if (delayTime >= 5f)  // 딜레이타임이 5초 이상 됐을 때
                //    {
                //        if (_nav.isOnNavMesh)
                //        {
                //            _nav.isStopped = false;
                //        }
                //        _enum_Unit_Action_Mode = eUnit_Action_States.monster_AngryPhase;
                //        _enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
                //        delayTime = 0f;
                //    }

                //}

                //
                if (Vector3.Distance(transform.position, castleTr.position) <= _unitData.attackRange + Castle.Instance.halfColiderValue)
                {
                    _anim.SetBool("isMove", false);

                    _enum_Unit_Action_Mode = eUnit_Action_States.monster_AttackCastlePhase;
                    _enum_Unit_Action_State = eUnit_Action_States.unit_Attack;
                    resetWayTime = 0f;
                }

                break;
        }
    }
    #endregion

    #region # Act_NormalMode() : 몬스터가 일반 모드일 때 호출되는 함수 , 구현된 행동 : 대기(탐지), 이동(성), 추적, 공격
    private void Act_AngryPhase()
    {
        switch (_enum_Unit_Action_State)     // 현재 유닛 행동
        {
            case eUnit_Action_States.unit_Idle: // 유닛 대기 상태
                _nav.isStopped = true;
                _anim.SetBool("isMove", false);
                if (!_isSearch)  // 적 탐지 않았을 때만 실행
                {
                    actUnitCs.SearchTarget(target_Search_Type: _eUnit_Target_Search_Type);
                }
                break;

            case eUnit_Action_States.unit_Tracking: // 유닛이 몬스터 추적
                actUnitCs.TrackingTarget(next_ActionState: eUnit_Action_States.unit_Move);
                break;

            case eUnit_Action_States.unit_Attack:   // 유닛이 몬스터 공격
                if (unitTargetSearchCs._targetUnit != null)
                {
                    actUnitCs.Attack_Unit(eUnit_Action_States.unit_Tracking);
                }
                break;
        }
    }
    #endregion

    #region # Act_CastleMode() : 몬스터가 일반 모드일 때 호출되는 함수 , 구현된 행동 : 대기(탐지), 이동(성), 추적, 공격
    private void Act_Atk_CastlePhase()
    {
        _nav.isStopped = true;
        switch (_enum_Unit_Action_State)     // 현재 유닛 행동
        {
            case eUnit_Action_States.unit_Attack:   // 유닛이 몬스터 공격
                _nav.isStopped = true;

                float _distance = Vector3.Distance(transform.position, castleTr.position);

                if (_distance > _unitData.attackRange + Castle.Instance.halfColiderValue)
                {
                    for (int i = 0; i < Castle.Instance.caslteModels.Length; i++)
                    {
                        float baseDistance = Vector3.Distance(transform.position, castleTr.position);
                        float distance = Vector3.Distance(transform.position, Castle.Instance.caslteModels[i].position);

                        if (distance < baseDistance)
                        {
                            castleTr = Castle.Instance.caslteModels[i];
                        }

                    }

                    _anim.SetBool("isMove", true);
                    _nav.isStopped = false;
                    _enum_Unit_Action_Mode = eUnit_Action_States.monster_NormalPhase;
                    _enum_Unit_Action_State = eUnit_Action_States.unit_Move;
                    _nav.SetDestination(castleTr.position);
                }

                unitTargetSearchCs.Look_At_The_Castle(next_Action_State: eUnit_Action_States.unit_Attack);
                Debug.LogWarning("몬스터 성 공격");
                //if (unitTargetSearchCs._targetUnit != null)
                //{
                //    actUnitCs.ReadyForAttack(unit_Atk_State: eUnit_Action_States.unit_Tracking);
                //}
                break;
        }
    }

    #endregion
    public IEnumerator Test()
    {
        print("오크 길변환");
        _nav.ResetPath();
        yield return null;
        _nav.SetDestination(castleTr.position);
        yield return new WaitForSeconds(5f);
        Test();
    }


}
