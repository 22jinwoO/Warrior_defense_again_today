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

    [SerializeField]
    private float delayTime;    // 대기 시간


    [SerializeField]
    Rigidbody rgb;

    


    [SerializeField]
    private float timer;
    //[SerializeField]
    //private NavMeshPath path;

    // Start is called before the first frame update
    void Awake()
    {
        rgb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        _this_Unit_Armor_Property = new MailArmor();
        _enum_Unit_Action_Mode = eUnit_Action_States.monster_MoveMode;
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();
        anim =GetComponent<Animator>();
        InitUnitInfoSetting();
        castleTr = Castle.Instance.transform;
        //NavMeshPath path = new NavMeshPath();
        //nav.CalculatePath(castleTr.position, path);
        //print(nav.CalculatePath(castleTr.position, path));
        //print(nav.SetPath(path));
        //NavMeshPath path = new NavMeshPath();
        //nav.CalculatePath(castleTr.position, path);
        //nav.SetPath(path);
        //nav.SetDestination(castleTr.position); // 성으로 이동
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        //while (timer<=30f)
        //{
        //    print("더해주기");
        //    timer += Time.deltaTime;
        //}
        //if (timer>30f)
        //{
        //    //nms.BuildNavMesh();
        //    timer = 0f;
        //}
        //print(timer);
        //nav.SetDestination(castleTr.position); // 성으로 이동
        //path = new NavMeshPath();
        //print(nav.CalculatePath(castleTr.position,path));
        Unit_Attack_Skill_CoolTime();
        //nav.SetDestination(castleTr.position); // 성으로 이동
        hpText.text="몬스터 체력 : "+ Mathf.CeilToInt(_unitData._unit_Health).ToString();
        if (nav.isStopped == true)
        {
            print("멈춤");
        }
        Act_By_Unit();
        //if (isMove)
        //{
        //    //nav.SetDestination(pointTr.position);
        //    nav.SetDestination(castleTr.position); // 성으로 이동
        //}
    }



    public override void InitUnitInfoSetting()
    {

        _unitData._unit_Name = "오크";            // 유닛 이름
        _unitData._unit_Health = 200f;             // 유닛 체력
        _unitData._eUnit_Attack_Property = eUnit_Attack_Property_States.slash_Attack;    // 유닛 공격속성
        _unitData._unit_Attack_Damage = 1f;    // 유닛 공격 데미지
        _unitData._unit_Skill_Attack_Damage = 1f;    // 유닛 스킬 공격 데미지
        _unitData._eUnit_Defense_Property = eUnit_Defense_Property_States.mail_Armor;   // 유닛 방어속성
        _unitData._unit_Description = "오크입니다";     // 유닛 설명
        _unitData._unit_Type = "근거리";            // 유닛 타입
        _unitData._unit_MoveSpeed = 1f;        // 유닛 이동속도
        _unitData._unit_Outlook = 20f;          // 유닛 시야
        _unitData._unit_Attack_Range = 3.5f;     // 유닛 공격 범위
        _unitData._unit_Attack_Speed = 3f;        // 유닛 공격 속도
        _unitData._unit_Attack_CoolTime = 3f;     // 유닛 기본 공격 쿨타임
        _unitData._unit_Skill_CoolTime = 8f;     // 유닛 스킬 공격 쿨타임
        _unitData._unit_Attack_CoolTime = 0f;     // 유닛 기본 공격 쿨타임
    }
    
    public void Act_By_Unit()
    {
        switch (_enum_Unit_Action_Mode) // 유닛 모드에 따라 행동
        {
            case eUnit_Action_States.monster_MoveMode: // 몬스터 이동 모드일 때 행동

                switch (_enum_Unit_Action_Type)     // 현재 유닛 행동
                {

                    case eUnit_Action_States.unit_Move: // 유닛 이동
                        if (isChangeState)  // 상태가 변환됐을 때
                        {


                            isSearch = false;
                            isClick = false;
                            unitTargetSearchCs._targetUnit = null;
                            print("애니메이션 실행");
                            float time = 0f;

                            while (time<0.3f)
                            {
                                time += Time.deltaTime;
                            }
                            isChangeState = false;
                        }

                        //if (nav.velocity.magnitude.Equals(0f))
                        //{
                        //    nav.ResetPath();
                        //    nav.SetDestination(castleTr.position); // 성으로 이동
                        //}

                        anim.SetBool("isMove", true);   // 걷는 모션 애니메이션 실행
                        if (nav.velocity.magnitude <= 0.3f)   // 네비 메쉬 에이전트의 이동속도가 0 이하라면
                        {
                            anim.SetBool("isMove", false);
                            delayTime += Time.deltaTime;    // 대기 시간에 타임.델타타임 더해줌

                            if (delayTime >= 20f)  // 딜레이타임이 1초 이상 됐을 때
                            {
                                nav.isStopped = false;
                                _enum_Unit_Action_Type = eUnit_Action_States.unit_Idle;
                                _enum_Unit_Action_Mode = eUnit_Action_States.monster_AttackMode;    // 몬스터 공격상태로 변환

                                delayTime = 0f;
                            }

                        }
                        nav.SetDestination(castleTr.position); // 성으로 이동
                        break;
                }
                break;

            case eUnit_Action_States.monster_AttackMode:

                switch (_enum_Unit_Action_Type)
                {
                    case eUnit_Action_States.unit_Idle: // 유닛 대기 상태
                        nav.isStopped = true;
                        if (!isSearch)  // 적 탐지 않았을 때만 실행
                        {

                            unitTargetSearchCs.Search_For_Near_Enemy();
                        }

                        break;


                    case eUnit_Action_States.unit_Tracking: // 유닛이 몬스터 추적

                        nav.isStopped = false;

                        float distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);
                        //print("거리 : " + distance);

                        unitTargetSearchCs.Look_At_The_Target();

                        if (distance >= _unitData._unit_Attack_Range && distance <= _unitData._unit_Outlook)   // 공격 범위에 안들어오고 유닛 시야범위보다 작다면
                        {
                            print("추격중");
                            anim.SetBool("isMove", true);
                            nav.SetDestination(unitTargetSearchCs._targetUnit.position);
                        }

                        // 공격 범위에 적이 들어왔을 때
                        else if (distance <= _unitData._unit_Attack_Range)
                        {
                            print("공격 타입으로 변환");
                            anim.SetBool("isMove", false);
                            nav.SetDestination(transform.position);
                            _enum_Unit_Action_Type = eUnit_Action_States.unit_Attack;
                        }

                        // 시야밖으로 적이 사라졌을 때
                        else if (distance > _unitData._unit_Outlook)
                        {
                            nav.SetDestination(transform.position);
                            anim.SetBool("isMove", false);

                            isSearch = false;
                            unitTargetSearchCs._targetUnit = null;
                            nav.isStopped = false;
                            _enum_Unit_Action_Type = eUnit_Action_States.unit_Idle;
                        }
                        break;

                    case eUnit_Action_States.unit_Attack:   // 유닛이 몬스터 공격
                        distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);

                        // 거리가 공격범위보다 크면 유닛 추적
                        if (distance > _unitData._unit_Attack_Range)
                        {
                            _enum_Unit_Action_Type = eUnit_Action_States.unit_Tracking;
                        }

                        //공격모션을 실행하고
                        unitTargetSearchCs.Look_At_The_Target();
                        break;

                    default:
                        break;
                }
                break;
        }
    }


    #region # Unit_Attack_Skill_CoolTime() : 유닛 기본공격, 스킬공격 쿨타임 돌려주는 함수
    public void Unit_Attack_Skill_CoolTime()
    {
        // 기본 공격이 가능한지 확인
        _can_Base_Attack = _unitData._unit_Attack_CoolTime >= _unitData._unit_Attack_Speed ? true : false;

        // 스킬 공격이 가능한지 확인
        _can_Skill_Attack = _unitData._unit_Current_Skill_CoolTime >= _unitData._unit_Skill_CoolTime ? true : false;

        //현재 스킬 공격 쿨타임이 유닛의 스킬 공격 쿨타임 보다 낮다면 쿨타임 돌려주기
        if (_unitData._unit_Skill_CoolTime >= _unitData._unit_Current_Skill_CoolTime)
        {
            _unitData._unit_Current_Skill_CoolTime += Time.deltaTime;
        }

        //현재 기본 공격 쿨타임이 유닛의 기본 공격속도 보다 낮다면 쿨타임 돌려주기
        if (_unitData._unit_Attack_Speed >= _unitData._unit_Attack_CoolTime)
        {
            _unitData._unit_Attack_CoolTime += Time.deltaTime;
        }
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (unitTargetSearchCs._targetUnit!=null&&unitTargetSearchCs._targetUnit.Equals(other))
        {
            nav.isStopped = true;
            //float time = 0f;
            //while (time<0.3f)
            //{
            //    time += Time.deltaTime;
            //}
        }
        print("트리거 콜라이더 충돌");
        //rgb.angularVelocity = Vector3.zero;
        //print(rgb.angularVelocity);
    }


    private void OnTriggerExit(Collider other)
    {
        if (unitTargetSearchCs._targetUnit != null && unitTargetSearchCs._targetUnit.Equals(other))
        {
            nav.isStopped = false;

        }
        print("트리거 콜라이더 나감");

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _unitData._unit_Outlook);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _unitData._unit_Attack_Range);
    }
}
