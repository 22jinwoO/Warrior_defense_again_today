using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitDataManager;
using UnityEngine.AI;

public class OrcShaman : MonsterUnitClass
{
    // Start is called before the first frame update
    void Awake()
    {
        soundPos = GameObject.FindGameObjectWithTag("SoundPos").transform;
        _nav = GetComponent<NavMeshAgent>();
        print(_nav.obstacleAvoidanceType);

        Init_Vfx();
        _this_Unit_ArmorCalculateCs = new PlateArmor();

        _enum_Unit_Action_Mode = eUnit_Action_States.monster_NormalPhase;
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();
        actUnitCs = GetComponent<ActUnit>();
        _anim = GetComponent<Animator>();
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

        //StartCoroutine(Test());
        //InitUnitInfoSetting(UnitDataManager.Instance._unitInfo_Dictionary["orc_sham01"]);
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
        if (unitTargetSearchCs._targetUnit != null && unitTargetSearchCs._targetUnit.GetComponent<SphereCollider>().enabled.Equals(false))
        {
            _isSearch = false;
            unitTargetSearchCs._targetUnit = null;
            unitTargetSearchCs._target_Body = null;
            _enum_Unit_Action_Mode = _enum_mUnit_Action_BaseMode;
            _enum_Unit_Action_State = _enum_mUnit_Action_BaseState;

            _nav.SetDestination(castleTr.position);
            print("모드 전환");
        }


    }

    private void FixedUpdate()
    {
        if (canAct && _nav.isOnNavMesh)
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
        _nav.speed = 3.5f;
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

        //gen_skill._projectile_Prefab.SetActive(false);


        // 유닛 타겟 설정 타입
        _unitData.targetSelectType = character_Data.targetSelectType;



        // 유닛 방어구 속성 할당
        _unitData._eUnit_Defense_Property = character_Data.unit_Armor_property;

        // 유닛 방어구 속성에 따른 계산을 위한 스크립트 할당
        _this_Unit_ArmorCalculateCs = character_Data.unit_ArmorCalculateCs;

        // 유닛 타겟 설정 타입 할당
        _unitData._unit_targetSelectType = UnitDataManager.Instance._targetSelect_Dictionary[character_Data.targetSelectType];
        _eUnit_Target_Search_Type = UnitDataManager.Instance._targetSelect_Dictionary[character_Data.targetSelectType];

        // 유닛 자유 모드
        _enum_Unit_Action_Mode = eUnit_Action_States.monster_NormalPhase;

        // 유닛 기본 대기 상태
        _enum_Unit_Action_State = eUnit_Action_States.unit_Move;

        // 유닛 추격 상태
        _enum_Unit_Attack_State = eUnit_Action_States.unit_Tracking;

        // 근접 공격 유닛 (나중에 없애도 됨)
        _enum_Unit_Attack_Type = eUnit_Action_States.close_Range_Atk;


        _unitData._unit_General_Skill_Dmg = 1f;                                                  // 유닛 공격 데미지
        _unitData._unit_Special_Skill_Dmg = 6f;                                            // 유닛 공격 데미지
        _unitData._unit_MoveSpeed = 3.5f;                                                      // 유닛 이동속도
        _unitData._unit_Attack_Speed = 5f;                                                   // 유닛 공격 속도
        _unitData._unit_Attack_CoolTime = 5f;                                                // 유닛 기본 공격 쿨타임
        _unitData._unit_Skill_CoolTime = 8f;                                                 // 유닛 스킬 공격 쿨타임
    }

    #region # InitUnitInfoSetting(): 유닛 정보 셋팅하는 함수
    public override void InitUnitInfoSetting(CharacterData character_Data)
    {
        if (Castle._castle_Hp.Equals(0))
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
}
