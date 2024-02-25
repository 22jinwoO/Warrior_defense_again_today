using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnitDataManager;
using EnumTypes;
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
        soundPos = GameObject.FindGameObjectWithTag("SoundPos").transform;

        print(MonsterKind);
        rgb = GetComponent<Rigidbody>();
        _nav = GetComponent<NavMeshAgent>();
        print(_nav.obstacleAvoidanceType);
        //ObstacleAvoidanceType obstacleAvoidanceType = ;
        //obstacleAvoidanceType.
        //print(_nav.obstacleAvoidanceType.);

        Init_Vfx();

        _this_Unit_ArmorCalculateCs = new ChainArmor();
        _enum_Unit_Action_Mode = eUnit_Action_States.monster_NormalPhase;
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();
        actUnitCs = GetComponent<ActUnit>();
        _anim =GetComponent<Animator>();
        //InitUnitInfoSetting();
        castleTr = Castle.Instance.transform;
        canAct = true;

        sprCol = GetComponent<SphereCollider>();

        for (int i = 0; i < someMeshReners.Length; i++)
        {
            someMeshReners[i].material = Instantiate(mosterUnitMtr);

        }
        bodyMeshRener.material = Instantiate(mosterUnitMtr);
        //_unit_CloakingMtr = Instantiate(_unit_CloakingMtr);
        someMtr = new Material[someMeshReners.Length];
        bodyMtr = bodyMeshRener.material;

        cloaking_someMtr = new Material[someMeshReners.Length];

        for (int i = 0; i < someMeshReners.Length; i++)
        {
            cloaking_someMtr[i] = Instantiate(_unit_CloakingMtr);

        }
        cloaking_bodyMtr = Instantiate(_unit_CloakingMtr);


        // 사운드 오디오 소스 할당
        atkSoundPlayer = GetComponents<AudioSource>()[0];
        hitSoundPlayer = GetComponents<AudioSource>()[1];
        _nav.SetDestination(castleTr.position); // 성으로 이동

        //NavMeshPath path = new NavMeshPath();
        //_nav.CalculatePath(castleTr.position, path);
        //print(_nav.CalculatePath(castleTr.position, path));
        //print(path.corners.Length);
        //_nav.SetPath(path);
        //_nav.SetDestination(castleTr.position); // 성으로 이동
    }

    private void OnEnable()
    {
        bodyMeshRener.material = bodyMtr;
        _nav.isStopped = false;

        for (int j = 0; j < someMeshReners.Length; j++)
        {
            someMeshReners[j].material = someMtr[j];
            //0.157f
        }
        Color cloaking_Mtr_Color = bodyMeshRener.material.color;
        cloaking_Mtr_Color.a = 1f;
        _unit_CloakingMtr.color = cloaking_Mtr_Color;

        for (int j = 0; j < someMeshReners.Length; j++)
        {
            cloaking_someMtr[j].color = cloaking_Mtr_Color;
            //0.157f
        }
        _nav.SetDestination(castleTr.position);

    }


    // Update is called once per frame

    void Update()
    {
        Unit_Attack_Skill_CoolTime();

        // 타겟이 죽었을 때 호출되는 함수
        if (unitTargetSearchCs._targetUnit!=null&& unitTargetSearchCs._targetUnit.GetComponent<SphereCollider>().enabled.Equals(false))
        {
            _isSearch = false;
            unitTargetSearchCs._targetUnit = null;
            unitTargetSearchCs._target_Body = null;
            _enum_Unit_Action_Mode = _enum_mUnit_Action_BaseMode;
            _enum_Unit_Action_State = _enum_mUnit_Action_BaseState;
            _nav.SetDestination(castleTr.position);
        }


    }

    private void FixedUpdate()
    {
        if (canAct&&_nav.isOnNavMesh)
        {
            Act_By_Unit();  // 유닛 행동 구분지어 주는 함수
        }

    }

    //private void U

    // 활성화 시 필요한 초기 데이터 값 부여하는 함수
    private void SetUnitValue()
    {
        canAct = true;
        sprCol.enabled = true;
        _nav.enabled = true;
        _isSearch = false;
        _isDead = false;
        _nav.speed = 1f;
        _nav.acceleration = 8f;

    }

    private void SetStructValue(CharacterData character_Data)
    {
        // 유닛 가속도
        _unitData.moveAcc = 8f;
        // 유닛 이름
        _unitData._unit_Name = character_Data.char_id;

        // 캐릭터 넘버
        _unitData.no = character_Data.no;

        // 캐릭터 id
        _unitData.char_id = character_Data.char_id;

        // 유닛 클래스
        _unitData.unit_class = character_Data.unit_class;

        // 레벨
        _unitData.level = character_Data.level;

        // 체력
        _unitData.maxHp = character_Data.hp;
        _unitData.hp = character_Data.hp;

        // 방어 타입
        _unitData.defenseType = character_Data.defenseType;

        // 이동속도
        _unitData.moveSpeed = character_Data.moveSpeed;
        _unitData.moveSpeed = 3.5f;


        // 시야 범위
        _unitData.sightRange = 24f;
        //_unitData.sightRange = character_Data.sightRange;

        // 공격 범위
        _unitData.attackRange = 2f;
        //_unitData.attackRange = character_Data.attackRange;

        // 크리티컬 확률
        //_unitData.criticRate = character_Data.criticRate;
        _unitData.criticRate = 10;


        // 일반스킬
        _unitData.generalSkill = character_Data.generalSkill;

        // 일반스킬 이름
        _unitData.generalSkillName = character_Data.generalSkillName;

        //일반스킬 할당
        gen_skill = Instantiate(character_Data.unit_Gen_Skill, transform);
        gen_skill.gameObject.name = _unitData.generalSkillName;
        gen_skill._link_Skill = character_Data.unit_Gen_Skill._link_Skill;
        gen_skill.unitInfoCs = this;

        // 특수 스킬 할당
        //gen_skill = character_Data.unit_Spc_Skill;
        //unit_Spc_Skill.unitInfoCs = this;

        //// 특수 스킬 , 자유모드 일 때 사용하는 스킬
        //_unitData.specialSkill1 = character_Data.specialSkill1;

        //// 특수 스킬 1 이름
        //_unitData.specialSkill1Name = character_Data.specialSkill1Name;

        //// 특수 스킬 , 홀드모드 일 때 사용하는 스킬
        //_unitData.specialSkill2 = character_Data.specialSkill2;

        //// 특수 스킬 2 이름
        //_unitData.specialSkill2Name = character_Data.specialSkill2Name;

        // 유닛 타겟 설정 타입
        _unitData.targetSelectType = character_Data.targetSelectType;



        // 유닛 방어구 속성 할당
        _unitData._eUnit_Defense_Property = character_Data.unit_Armor_property;

        // 유닛 방어구 속성에 따른 계산을 위한 스크립트 할당
        _this_Unit_ArmorCalculateCs = character_Data.unit_ArmorCalculateCs;

        // 유닛 타겟 설정 타입 할당
        _unitData._unit_targetSelectType = character_Data.unit_targetSelectType;

        // 유닛 자유 모드
        _enum_Unit_Action_Mode = eUnit_Action_States.monster_NormalPhase;

        // 유닛 기본 대기 상태
        _enum_Unit_Action_State = eUnit_Action_States.unit_Move;

        // 유닛 추격 상태
        _enum_Unit_Attack_State = eUnit_Action_States.unit_Tracking;

        // 근접 공격 유닛 (나중에 없애도 됨)
        _enum_Unit_Attack_Type = eUnit_Action_States.close_Range_Atk;


        //_unitData._unit_maxHealth = 200f;                                                       // 유닛 최대 체력
        //_unitData._unit_maxHealth = 200f;                                                       // 유닛 현재 체력

        //_unitData._eUnit_genSkill_Property = eUnit_Attack_Property_States.slash_Attack;      // 유닛 공격속성
        _unitData._unit_General_Skill_Dmg = 1f;                                                  // 유닛 공격 데미지
        _unitData._unit_Special_Skill_Dmg = 6f;                                            // 유닛 공격 데미지
        //_unitData._eUnit_Defense_Property = eUnit_Defense_Property_States.padding_Armor;    // 유닛 방어속성
        //_unitData._unit_Description = "용사입니다";                                           // 유닛 설명
        //_unitData._unit_Type = "용사";                                                       // 유닛 타입
        _unitData._unit_MoveSpeed = 1f;                                                      // 유닛 이동속도
        //_unitData.sightRange = 8f;                                                     // 유닛 시야
        //_unitData.attackRange = 4f;                                                   // 유닛 공격 범위
        _unitData._unit_Attack_Speed = 3f;                                                   // 유닛 공격 속도
        _unitData._unit_Attack_CoolTime = 5f;                                                // 유닛 기본 공격 쿨타임
        _unitData._unit_Skill_CoolTime = 8f;                                                 // 유닛 스킬 공격 쿨타임

        //_unitData.unit_Id = "hum_warr01";

    }

    #region # InitUnitInfoSetting(): 유닛 정보 셋팅하는 함수
    public override void InitUnitInfoSetting(CharacterData character_Data)
    {
        if (Castle.Instance._castle_Hp.Equals(0))
        {
            OnCastleDown();
        }

        // 성 무너졌을 때 기본 상태로 변환되는 이벤트 함수 연결
        Castle.Instance.OnCastleDown += OnCastleDown;


        // 활성화 시 필요한 초기 데이터 값 부여하는 함수
        SetUnitValue();

        SetStructValue(character_Data);
    }
    #endregion
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

    #region # Act_NormalMode() : 몬스터가 일반 모드일 때 호출되는 함수 , 구현된 행동 : 대기(탐지), 이동(성), 추적, 공격
    private void Act_NormalPhase()
    {
        switch (_enum_Unit_Action_State)     // 현재 유닛 행동
        {
            case eUnit_Action_States.unit_Move: // 유닛 이동

                if (isChangeState)  // 상태가 변환됐을 때
                {
                    _isSearch = false;

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
                //_nav.SetDestination(castleTr.position); // 성으로 이동

                //unitTargetSearchCs._unitModelTr.LookAt(castleTr.position);
                //unitTargetSearchCs._unitModelTr.localRotation = Quaternion.Euler(0f, transform.rotation.y, transform.rotation.z);
                //_nav.isStopped = false;

                _anim.SetBool("isMove", true);   // 걷는 모션 애니메이션 실행
                if (_nav.velocity.magnitude <= 0.3f)   // 네비 메쉬 에이전트의 이동속도가 0 이하라면
                {
                    _anim.SetBool("isMove", false);
                    delayTime += Time.deltaTime;    // 대기 시간에 타임.델타타임 더해줌

                    if (delayTime >= 5f)  // 딜레이타임이 5초 이상 됐을 때
                    {
                        if (_nav.isOnNavMesh)
                        {
                            _nav.isStopped = false;
                        }
                        _enum_Unit_Action_Mode = eUnit_Action_States.monster_AngryPhase;
                        _enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
                        delayTime = 0f;
                    }

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
                float _distance = Vector3.Distance(transform.position, transform.position);

                unitTargetSearchCs.Look_At_The_Castle(next_Action_State : eUnit_Action_States.unit_Attack);
                Debug.LogWarning("몬스터 성 공격");
                //if (unitTargetSearchCs._targetUnit != null)
                //{
                //    actUnitCs.ReadyForAttack(unit_Atk_State: eUnit_Action_States.unit_Tracking);
                //}
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
        if (unitTargetSearchCs._targetUnit != null && unitTargetSearchCs._targetUnit.Equals(other))
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
    //    Gizmos.DrawWireSphere(transform.position, _unitData.sightRange);

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, _unitData.attackRange);
    //}
}
