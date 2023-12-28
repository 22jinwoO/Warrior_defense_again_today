using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Orc : MonsterUnitClass, IActByUnit
{
    [SerializeField]
    Text hpText;

    [SerializeField]
    bool isMove=false;

    [SerializeField]
    Transform pointTr;

    [SerializeField]
    private bool isChangeState=true; // 상태 변환 체크하는 변수

    [SerializeField]
    private Transform castleTr; // 성 트랜스폼

    [Header("몬스터 대기 시간")]
    [SerializeField]
    private float delayTime;    // 대기 시간


    [SerializeField]
    Rigidbody rgb;

    


    [SerializeField]
    private float timer;

    public Orc MonsterKind  // 몬스터 종류 반환
    {
        get { return this; }
    }
    //[SerializeField]
    //private NavMeshPath path;

    // Start is called before the first frame update
    void Awake()
    {

        print(MonsterKind);
        rgb = GetComponent<Rigidbody>();
        _nav = GetComponent<NavMeshAgent>();
        print(_nav.obstacleAvoidanceType);
        //ObstacleAvoidanceType obstacleAvoidanceType = ;
        //obstacleAvoidanceType.
        //print(_nav.obstacleAvoidanceType.);

        _this_Unit_Armor_Property = new MailArmor();
        _enum_Unit_Action_Mode = eUnit_Action_States.monster_NormalMode;
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();
        actUnitCs = GetComponent<ActUnit>();
        _anim =GetComponent<Animator>();
        InitUnitInfoSetting();
        castleTr = Castle.Instance.transform;

        //NavMeshPath path = new NavMeshPath();
        //_nav.CalculatePath(castleTr.position, path);
        //print(_nav.CalculatePath(castleTr.position, path));
        //print(path.corners.Length);
        //_nav.SetPath(path);
        //_nav.SetDestination(castleTr.position); // 성으로 이동
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.identity;

        Unit_Attack_Skill_CoolTime();

        //if (Input.GetMouseButtonDown(1))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out RaycastHit hit))
        //    {
        //        _nav.SetDestination(hit.point);
        //        //_movePos = hit.point;
        //        //_enum_Unit_Action_State = eUnit_Action_States.unit_Move;
        //    }
        //}
        //hpText.text="몬스터 체력 : "+ Mathf.CeilToInt(_unitData._unit_maxHealth).ToString();
    }

    private void FixedUpdate()
    {
        Act_By_Unit();  // 유닛 행동 구분지어 주는 함수
    }

    public override void InitUnitInfoSetting()
    {

        _unitData._unit_Name = "오크";            // 유닛 이름
        _unitData._unit_maxHealth = 200f;             // 유닛 체력
        _unitData._eUnit_genSkill_Property = eUnit_Attack_Property_States.slash_Attack;    // 유닛 공격속성
        _unitData._unit_General_Skill_Dmg = 1f;    // 유닛 공격 데미지
        _unitData._unit_Special_Skill_Dmg = 1f;    // 유닛 스킬 공격 데미지
        _unitData._eUnit_Defense_Property = eUnit_Defense_Property_States.chain_Armor;   // 유닛 방어속성
        _unitData._unit_Description = "오크입니다";     // 유닛 설명
        _unitData._unit_Type = "근거리";            // 유닛 타입
        _unitData._unit_MoveSpeed = 1f;        // 유닛 이동속도
        _unitData._unit_SightRange = 20f;          // 유닛 시야
        _unitData._unit_Attack_Range = 3.5f;     // 유닛 공격 범위
        _unitData._unit_Attack_Speed = 3f;        // 유닛 공격 속도
        _unitData._unit_Attack_CoolTime = 3f;     // 유닛 기본 공격 쿨타임
        _unitData._unit_Skill_CoolTime = 8f;     // 유닛 스킬 공격 쿨타임
        _unitData._unit_Attack_CoolTime = 0f;     // 유닛 기본 공격 쿨타임
    }

    #region # Act_By_Unit() : 유닛 행동 구분지어주는 함수
    public void Act_By_Unit()
    {
        switch (_enum_Unit_Action_Mode) // 유닛 모드에 따라 행동
        {
            case eUnit_Action_States.monster_NormalMode: // 몬스터 이동 모드일 때 행동
                Act_NormalMode();
                break;

            case eUnit_Action_States.monster_AngryMode:
                switch (_enum_Unit_Action_State)
                {
                    default:
                        break;
                }
                break;
        }
    }
    #endregion

    #region # Act_NormalMode() : 몬스터가 일반 모드일 때 호출되는 함수 , 구현된 행동 : 대기(탐지), 이동(성), 추적, 공격
    private void Act_NormalMode()
    {
        switch (_enum_Unit_Action_State)     // 현재 유닛 행동
        {
            case eUnit_Action_States.unit_Idle: // 유닛 대기 상태
                _nav.isStopped = true;
                if (!_isSearch)  // 적 탐지 않았을 때만 실행
                {
                    actUnitCs.SearchTarget(target_Search_Type: _eUnit_Target_Search_Type);
                }
                break;

            case eUnit_Action_States.unit_Move: // 유닛 이동

                if (isChangeState)  // 상태가 변환됐을 때
                {
                    _isSearch = false;
                    _nav.SetDestination(castleTr.position); // 성으로 이동

                    //StartCoroutine(Test());
                    unitTargetSearchCs._targetUnit = null;
                    print("애니메이션 실행");
                    float time = 0f;

                    while (time < 0.3f)
                    {
                        time += Time.deltaTime;
                    }
                    isChangeState = false;
                }

                unitTargetSearchCs._unitModelTr.LookAt(castleTr.position);
                unitTargetSearchCs._unitModelTr.localRotation = Quaternion.Euler(0f, transform.rotation.y, transform.rotation.z);
                _nav.isStopped = false;

                _anim.SetBool("isMove", true);   // 걷는 모션 애니메이션 실행
                if (_nav.velocity.magnitude <= 0.3f)   // 네비 메쉬 에이전트의 이동속도가 0 이하라면
                {
                    _anim.SetBool("isMove", false);
                    delayTime += Time.deltaTime;    // 대기 시간에 타임.델타타임 더해줌

                    if (delayTime >= 5f)  // 딜레이타임이 1초 이상 됐을 때
                    {
                        _nav.isStopped = false;
                        _enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
                        delayTime = 0f;
                    }

                }
                else
                {
                    delayTime = 0f;
                }
                break;

            case eUnit_Action_States.unit_Tracking: // 유닛이 몬스터 추적
                actUnitCs.TrackingTarget(next_ActionState: eUnit_Action_States.unit_Move);

                break;

            case eUnit_Action_States.unit_Attack:   // 유닛이 몬스터 공격
                if (unitTargetSearchCs._targetUnit != null)
                {
                    actUnitCs.ReadyForAttack(unit_Atk_State: eUnit_Action_States.unit_Move);
                }
                break;
        }
    }
    #endregion

    IEnumerator Test()
    {
        print("오크 길변환");
        _nav.ResetPath();
        yield return new WaitForSeconds(5f);
        Test();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (unitTargetSearchCs._targetUnit!=null&&unitTargetSearchCs._targetUnit.Equals(other))
        {
            _nav.isStopped = true;

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (unitTargetSearchCs._targetUnit != null && unitTargetSearchCs._targetUnit.Equals(other))
        {
            _nav.isStopped = false;

        }
        //print("트리거 콜라이더 나감");

    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, _unitData._unit_SightRange);

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, _unitData._unit_Attack_Range);
    //}
}
