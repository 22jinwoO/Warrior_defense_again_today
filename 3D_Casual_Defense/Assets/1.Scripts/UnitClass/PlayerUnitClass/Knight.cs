using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnitDataManager;

public class Knight : PlayerUnitClass
{
    [SerializeField]
    private bool isCollision;

    //Abs_StatusEffect asd = new Abs_StatusEffect();

    private void Awake()
    {
        soundPos = GameObject.FindGameObjectWithTag("SoundPos").transform;
        //chageModeVfx = Instantiate(chageModeVfx);

        navObs = GetComponent<NavMeshObstacle>();
        _anim = GetComponent<Animator>();
        _this_Unit_ArmorCalculateCs = new PaddingArmor();
        _nav = GetComponent<NavMeshAgent>();
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();
        actUnitCs=GetComponent<ActUnit>();
        _isClick = false;
        sprCol=GetComponent<SphereCollider>();

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

        //_unit_CloakingMtr = Instantiate(_unit_CloakingMtr);

        cloaking_someMtr = new Material[someMeshReners.Length];

        for (int i = 0; i < someMeshReners.Length; i++)
        {
            cloaking_someMtr[i] = Instantiate(_unit_CloakingMtr);

        }
        cloaking_bodyMtr = Instantiate(_unit_CloakingMtr);

        //_unit_CloakingMtr = Instantiate(_unit_CloakingMtr);

        //Rigidbody asd = GetComponent<Rigidbody>();
        //InitUnitInfoSetting();  // 유닛 정보 초기화 시켜주는 함수
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
    private void Start()
    {
        _nav.enabled=true;
        print(_nav+gameObject.name);
        //asd.unit_Data = _unitData;
        //StartCoroutine(asd.Apply_Status_Effect(this, "", 2, 5));

        //CDF(_unitData);
    }

    //public IEnumerator Get_Posion(int thisUnit, string linkId, int statusValue, int duration)
    //{
    //    yield return new WaitForSeconds(1f);
    //    //isPoison = false;

    //    int times = 0;

    //    while (times < duration)
    //    {
    //        //if (isPoison)
    //        //{
    //        //    yield return null;

    //        //    break;
    //        //}
    //        _unitData.hp -= statusValue;
    //        //print(thisUnit);
    //        //unit_Data.hp = thisUnit;
    //        //print("유닛hp : "+unit_Data.hp);

    //        times++;
    //        yield return new WaitForSeconds(1f);
    //    }
    //}
    public void Update()
    {
        Unit_Attack_Skill_CoolTime();
        CheckChangeMode();
        if (unitTargetSearchCs._targetUnit != null && unitTargetSearchCs._targetUnit.GetComponent<SphereCollider>().enabled.Equals(false))
        {
            _isSearch = false;
            unitTargetSearchCs._targetUnit = null;
            unitTargetSearchCs._target_Body = null;
            _enum_Unit_Action_Mode = _enum_pUnit_Action_BaseMode;
            _enum_Unit_Action_State = _enum_pUnit_Action_BaseState;
        }
        //_unitData = asd.unit_Data;

        //transform.eulerAngles = Vector3.zero;
        //print(_nav.velocity.magnitude);
        //print(_nav.desiredVelocity.magnitude);
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    asd.isStatusApply = true;
        //    StartCoroutine(asd.Apply_Status_Effect(this, "", 2, 5));

        //    //_nav.velocity = new Vector3(_nav.velocity.x / 2, _nav.velocity.y / 2, _nav.velocity.z / 2);
        //}
        if (_can_SpcSkill_Attack&&_enum_Unit_Action_Mode.Equals(eUnit_Action_States.unit_FreeMode))
        {
            _unitData.attackRange = _unitData.sightRange;
        }
        else
            _unitData.attackRange = 2f;
        //_nav.velocity.magnitude /= 2f;
        //if (_nav.velocity!=Vector3.zero)
        //{
        //            _nav.velocity = new Vector3(_nav.velocity.x/2, _nav.velocity.y/2, _nav.velocity.z/2);

        //}

        //if (_isClick&&Input.GetMouseButtonDown(1))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out RaycastHit hit))
        //    {
        //        _movePos = hit.point;
        //        _enum_Unit_Action_State = eUnit_Action_States.unit_Move;
        //    }
        //}
        //Unit_Attack_Skill_CoolTime();   // 유닛 기본 공격, 스킬 공격 쿨타임 돌려주는 함수

        if (Input.GetKeyDown(KeyCode.B))
        {
            Collider[] colls = Physics.OverlapSphere(transform.position, 10f, unitTargetSearchCs._layerMask);
            print("B눌림");
            for (int i = 0; i < colls.Length; i++)
            {
                UnitInfo unitInfo = colls[i].GetComponent<UnitInfo>();
                print(colls[i].name);
                unitInfo.unitTargetSearchCs._targetUnit = transform;
                unitInfo.unitTargetSearchCs._target_Body = body_Tr;
                //unitInfo.unitTargetSearchCs._target_Body = this.transform;

                unitInfo._enum_Unit_Action_Mode = eUnit_Action_States.monster_AngryPhase;
                unitInfo._enum_Unit_Action_State = eUnit_Action_States.unit_Attack;
                //unitInfo.unitTargetSearchCs._target_Body = 

            }
        }
    }

    private void FixedUpdate()
    {
        if (canAct)
        {
            Act_By_Unit();  // 유닛 행동 함수

        }
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

        canAct = true;
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
        _unitData.sightRange = 10f;
        //_unitData.sightRange = character_Data.sightRange;

        // 공격 범위
        _unitData.attackRange = 2f;
        //_unitData.attackRange = character_Data.attackRange;

        // 크리티컬 확률
        //_unitData.criticRate = character_Data.criticRate;
        _unitData.criticRate = character_Data.criticRate;


        // 일반스킬
        _unitData.generalSkill = character_Data.generalSkill;

        // 일반스킬 이름
        _unitData.generalSkillName = character_Data.generalSkillName;


        // 일반스킬 이름
        _unitData.generalSkillName = character_Data.generalSkillName;
        gen_skill = Instantiate(character_Data.unit_Gen_Skill, transform);
        gen_skill.gameObject.name = _unitData.generalSkillName;
        gen_skill._link_Skill = character_Data.unit_Gen_Skill._link_Skill;
        gen_skill.unitInfoCs = this;

        // 특수 스킬 , 자유모드 일 때 사용하는 스킬
        _unitData.specialSkill1 = character_Data.specialSkill1;

        _unitData.specialSkill1Name = character_Data.specialSkill1Name;
        spe_skill_1 = Instantiate(character_Data.unit_Spc_Skill, transform);
        spe_skill_1.gameObject.name = _unitData.specialSkill1Name;
        spe_skill_1._link_Skill = character_Data.unit_Spc_Skill._link_Skill;
        spe_skill_1.unitInfoCs = this;


        // 특수 스킬 , 홀드모드 일 때 사용하는 스킬
        _unitData.specialSkill2 = character_Data.specialSkill2;

        _unitData.specialSkill2Name = character_Data.specialSkill2Name;
        spe_skill_2 = Instantiate(character_Data.unit_Spc_Skill2, transform);
        spe_skill_2.gameObject.name = _unitData.specialSkill2Name;
        spe_skill_2._link_Skill = character_Data.unit_Spc_Skill2._link_Skill;
        print(spe_skill_2._link_Skill);
        spe_skill_2.unitInfoCs = this;


        // 유닛 타겟 설정 타입
        _unitData.targetSelectType = character_Data.targetSelectType;

        //// 일반스킬 할당
        //gen_skill = character_Data.unit_Gen_Skill;
        //print(gen_skill.unitInfoCs);

        //gen_skill.unitInfoCs = this;

        //// 특수 스킬 할당
        //spe_skill_1 = character_Data.unit_Spc_Skill;
        //spe_skill_1.unitInfoCs = this;

        //// 특수 스킬 할당
        //spe_skill_2 = character_Data.unit_Spc_Skill2;
        //spe_skill_2.unitInfoCs = this;

        gen_skill.unitTargetSearchCs = this.unitTargetSearchCs;
        print(unitTargetSearchCs);
        print(gen_skill.unitTargetSearchCs);
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
        _unitData._unit_Attack_CoolTime = 3f;                                                // 유닛 기본 공격 쿨타임
        _unitData._unit_Skill_CoolTime = 5f;                                                 // 유닛 스킬 공격 쿨타임

        //_unitData.unit_Id = "hum_warr01";
    }
    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (unitTargetSearchCs._targetUnit != null && unitTargetSearchCs._targetUnit.Equals(other))
        {
            _nav.isStopped = true;
            print("트리거 콜라이더 충돌");
        }

        // 돌격 스킬 사용중일 때
        //if (spe_skill_1.isRush.Equals(true)&& unitTargetSearchCs._targetUnit.Equals(other))
        //{
        //    _nav.isStopped = true;
        //    unitTargetSearchCs._targetUnit.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(spe_skill_1._skill_AtkType, unitTargetSearchCs._targetUnit, spe_skill_1._base_Value);
        //    _unitData._unit_Attack_CoolTime = 0f;
        //    _unitData._unit_Current_Skill_CoolTime = 0f;
        //}
    }


    private void OnTriggerExit(Collider other)
    {
        if (unitTargetSearchCs._targetUnit != null && unitTargetSearchCs._targetUnit.Equals(other))
        {
            _nav.isStopped = false;
            print("트리거 콜라이더 나감");
        }

    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, _unitData.sightRange);

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, _unitData.attackRange);
    //}
}
