using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitDataManager;
using UnityEngine.AI;
using UnityEditor;

public class HammerKnight : PlayerUnitClass
{
    [SerializeField]
    private bool isCollision;

    private void Awake()
    {
        soundPos = GameObject.FindGameObjectWithTag("SoundPos").transform;
        Debug.LogWarning(transform.position);
        navObs = GetComponent<NavMeshObstacle>();
        _anim = GetComponent<Animator>();
        _this_Unit_ArmorCalculateCs = new PaddingArmor();
        _nav = GetComponent<NavMeshAgent>();
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();
        actUnitCs = GetComponent<ActUnit>();
        _isClick = false;
        sprCol = GetComponent<SphereCollider>();
        //chageModeVfx = Instantiate(chageModeVfx);

        atkSoundPlayer = GetComponents<AudioSource>()[0];
        hitSoundPlayer = GetComponents<AudioSource>()[1];


        Init_Vfx();


        someMtr = new Material[someMeshReners.Length];

        for (int i = 0; i < someMeshReners.Length; i++)
        {
            someMeshReners[i].material = Instantiate(_unit_NomralMtr);

        }
        bodyMeshRener.material = Instantiate(_unit_NomralMtr);

        for (int i = 0; i < someMeshReners.Length; i++)
        {
            someMtr[i] = someMeshReners[i].material;

        }
        bodyMtr = bodyMeshRener.material;

        cloaking_someMtr = new Material[someMeshReners.Length];

        for (int i = 0; i < someMeshReners.Length; i++)
        {
            cloaking_someMtr[i] = Instantiate(_unit_CloakingMtr);

        }
        cloaking_bodyMtr = Instantiate(_unit_CloakingMtr);
        //InitUnitInfoSetting(UnitDataManager.Instance._unitInfo_Dictionary["hum_warr02"]);
    }

    private void Start()
    {
        _nav.enabled = true;
    }
    private void OnEnable()
    {
        bodyMeshRener.material = bodyMtr;

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
    }

    public void Update()
    {
        Unit_Attack_Skill_CoolTime();

        if (unitTargetSearchCs._targetUnit != null && unitTargetSearchCs._targetUnit.GetComponent<SphereCollider>().enabled.Equals(false))
        {
            _isSearch = false;
            unitTargetSearchCs._targetUnit = null;
            unitTargetSearchCs._target_Body = null;
            _enum_Unit_Action_Mode = _enum_pUnit_Action_BaseMode;
            _enum_Unit_Action_State = _enum_pUnit_Action_BaseState;
        }

        if (_can_SpcSkill_Attack)
        {
            _unitData.attackRange = _unitData.sightRange;
        }
        else
            _unitData.attackRange = 2f;

        CheckChangeMode();
    }

    private void FixedUpdate()
    {
        if (canAct)
        {
            Act_By_Unit();  // 유닛 행동 함수

        }
    }

    #region # SetStructValue() 함수 : 활성화 시 필요한 초기 데이터 값 부여하는 함수
    // 활성화 시 필요한 초기 데이터 값 부여하는 함수
    public override void SetUnitValue()
    {
        canAct = true;
        sprCol.enabled = true;
        _isSearch = false;
        _isDead = false;

        _nav.speed = 3.5f;
        _nav.acceleration = 8f;

    }
    #endregion

    #region # SetStructValue() 함수 : 유닛 데이터 Json 파싱 후 값 할당
    public override void SetStructValue(CharacterData character_Data)
    {
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

        // 시야 범위
        _unitData.sightRange = character_Data.sightRange;

        // 공격 범위
        _unitData.attackRange = character_Data.attackRange;

        // 크리티컬 확률
        _unitData.criticRate = character_Data.criticRate;


        // 일반스킬
        _unitData.generalSkill = character_Data.generalSkill;

        // 일반스킬 이름
        _unitData.generalSkillName = character_Data.generalSkillName;


        // 일반스킬 이름
        _unitData.generalSkillName = character_Data.generalSkillName;
        if(gen_skill==null)
        {
            gen_skill = Instantiate(character_Data.unit_Gen_Skill, transform);
            gen_skill.gameObject.name = _unitData.generalSkillName;
            gen_skill._link_Skill = character_Data.unit_Gen_Skill._link_Skill;
            gen_skill.unitInfoCs = this;

        }

        // 특수 스킬 , 자유모드 일 때 사용하는 스킬
        _unitData.specialSkill1 = character_Data.specialSkill1;

        _unitData.specialSkill1Name = character_Data.specialSkill1Name;

        if (spe_skill_1 == null)
        {
            spe_skill_1 = Instantiate(character_Data.unit_Spc_Skill, transform);
            spe_skill_1.gameObject.name = _unitData.specialSkill1Name;
            spe_skill_1._link_Skill = character_Data.unit_Spc_Skill._link_Skill;
            spe_skill_1.unitInfoCs = this;
        }



        // 특수 스킬 , 홀드모드 일 때 사용하는 스킬
        _unitData.specialSkill2 = character_Data.specialSkill2;

        _unitData.specialSkill2Name = character_Data.specialSkill2Name;

        if (spe_skill_2 == null)
        {
            spe_skill_2 = Instantiate(character_Data.unit_Spc_Skill2, transform);
            spe_skill_2.gameObject.name = _unitData.specialSkill2Name;
            spe_skill_2._link_Skill = character_Data.unit_Spc_Skill2._link_Skill;
            spe_skill_2.unitInfoCs = this;

        }

        // 유닛 타겟 설정 타입
        _unitData.targetSelectType = character_Data.targetSelectType;

        gen_skill.unitTargetSearchCs = this.unitTargetSearchCs;

        spe_skill_1.unitTargetSearchCs = this.unitTargetSearchCs;
        spe_skill_2.unitTargetSearchCs = this.unitTargetSearchCs;

        // 유닛 방어구 속성 할당
        _unitData._eUnit_Defense_Property = character_Data.unit_Armor_property;

        // 유닛 방어구 속성에 따른 계산을 위한 스크립트 할당
        _this_Unit_ArmorCalculateCs = character_Data.unit_ArmorCalculateCs;

        // 유닛 타겟 설정 타입 할당
        _unitData._unit_targetSelectType = character_Data.unit_targetSelectType;

        // 유닛 자유 모드
        _enum_Unit_Action_Mode = eUnit_Action_States.unit_FreeMode;

        // 유닛 기본 대기 상태
        _enum_Unit_Action_State = eUnit_Action_States.unit_Idle;

        // 유닛 추격 상태
        _enum_Unit_Attack_State = eUnit_Action_States.unit_Tracking;

        // 근접 공격 유닛 (나중에 없애도 됨)
        _enum_Unit_Attack_Type = eUnit_Action_States.close_Range_Atk;


        _unitData._unit_General_Skill_Dmg = 1f;                                                  // 유닛 공격 데미지
        _unitData._unit_Special_Skill_Dmg = 6f;                                            // 유닛 공격 데미지

        _unitData._unit_MoveSpeed = 1f;                                                      // 유닛 이동속도

        _unitData._unit_Attack_Speed = 3f;                                                   // 유닛 공격 속도
        _unitData._unit_Attack_CoolTime = 3f;                                                // 유닛 기본 공격 쿨타임
        _unitData._unit_Skill_CoolTime = 8f;                                                 // 유닛 스킬 공격 쿨타임

        holdObj.gameObject.SetActive(false);

    }
    #endregion

    #region # InitUnitInfoSetting(): 유닛 정보 셋팅하는 함수
    public override void InitUnitInfoSetting(CharacterData character_Data)
    {
        if (Castle.Instance._castle_Hp.Equals(0))
        {
            StopUnitAct();
        }

        // 성 무너졌을 때 기본 상태로 변환되는 이벤트 함수 연결
        Castle.Instance.OnCastleDown += StopUnitAct;

        // 활성화 시 필요한 초기 데이터 값 부여하는 함수
        SetUnitValue();

        // 유닛 데이터 Json 파싱 후 값 할당
        SetStructValue(character_Data);
    }
    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (unitTargetSearchCs._targetUnit != null && unitTargetSearchCs._targetUnit.Equals(other))
        {
            _nav.isStopped = true;
            print("트리거 콜라이더 충돌");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (unitTargetSearchCs._targetUnit != null && unitTargetSearchCs._targetUnit.Equals(other))
        {
            _nav.isStopped = false;
            print("트리거 콜라이더 나감");
        }

    }


}
