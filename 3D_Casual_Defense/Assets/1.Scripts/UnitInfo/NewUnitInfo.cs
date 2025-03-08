using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using static UnitDataManager;

// 함수이름은 명사말고 동사 먼저, enum은 변수이름 앞에 소문자 e 작성, 변수는 카멜표기법으로 소문자 이후 단어 첫글자 대문자
[Serializable]
public struct new_Unit_Data    // 유닛 데이터 가져오는 구조체
{
    [Header("유닛 이름")]
    public string _unit_Name;                           // 유닛 이름

    public int _unit_Level;                             // 유닛 레벨

    public int no; // 캐릭터 넘버
    public string char_id;   // 캐릭터 id
    public string unit_class; // 유닛 클래스
    public int level;    // 레벨
    public float maxHp;   // 최대 체력
    public float hp;   // 현재 체력
    public string defenseType;   // 방어 타입
    public float moveSpeed;   // 이동속도
    public float moveAcc;   // 이동 가속도

    public int criticRate;    // 크리티컬 확률

    public string generalSkill;   // 일반스킬
    public string generalSkillName;   // 일반스킬 이름

    public string specialSkill1;   // 특수 스킬 , 자유모드 일 때 사용하는 스킬
    public string specialSkill1Name;   // 특수 스킬 1 이름

    public string specialSkill2;   // 특수 스킬 , 홀드모드 일 때 사용하는 스킬
    public string specialSkill2Name;   // 특수 스킬 2 이름
    public string targetSelectType;   // 유닛 설정 타입

    public eUnit_Attack_Property_States _eUnit_genSkill_Property;    // 유닛 일반 스킬속성
    public float _unit_General_Skill_Dmg;                   // 유닛 일반 스킬 데미지
    public float _unit_Special_Skill_Dmg;                   // 유닛 특수 스킬 데미지
    public eUnit_Defense_Property_States _eUnit_Defense_Property; // 유닛 방어속성
    public string _unit_Description;                        // 유닛 설명
    public string _unit_Type;                               // 유닛 타입
    public float _unit_MoveSpeed;                           // 유닛 이동속도
    public float sightRange;                          // 유닛 시야
    public float attackRange;                        // 유닛 공격 범위
    public float _unit_Attack_Speed;                        // 유닛 공격 속도
    public float _unit_Attack_CoolTime;                     // 유닛 기본 공격 쿨타임
    public float _unit_Current_Skill_CoolTime;              // 유닛 현재 스킬 공격 쿨타임
    public float _unit_Skill_CoolTime;                      // 유닛 스킬 공격 쿨타임
    public int _unit_CriticalRate;                          // 유닛 크리티컬 확률
    public eUnit_targetSelectType _unit_targetSelectType;   // 타겟 선정 타입
    public string unit_Id;                                  // 유닛 Id
}


public class NewUnitInfo : MonoBehaviour
{
    [Header("유닛 데이터 구조체 변수")]
    public new_Unit_Data _unitData; // 유닛 데이터 구조체 변수

    [Header("유닛 행동 모드 상태 변수")]
    public eUnit_Action_States _enum_Unit_Action_Mode;   // 유닛 모드 상태 변수

    [Header("유닛 행동 상태 변수")]
    public eUnit_Action_States _enum_Unit_Action_State;   // 유닛 행동 상태 변수

    // 발사체 프리팹 *************************
    [Header("일반스킬 사용할 때의 발사체 게임 오브젝트 프리팹")]
    public Abs_Bullet _projectile_Bullet;   // 스킬 공격 가능 불가능 확인하는 변수

    [Header("내비메쉬 에이전트")]
    public NavMeshAgent _nav;    // 내비메쉬

    [Header("애니메이터")]
    public Animator _anim;       // 애니메이터


    [Header("유닛 방어구 속성")]
    public ArmorCalculate _this_Unit_ArmorCalculateCs;

    [Header("몸통 트랜스폼")]
    public Transform body_Tr;



    // 스킬 1,2 ===============================
    [Header("일반 스킬")]
    public AbsNewSKill gen_skill;

    [Header("특수 스킬 - 1")]
    public AbsNewSKill spe_skill_1;

    [Header("특수 스킬 - 2")]
    public AbsNewSKill spe_skill_2;

    // 발사체 프리팹 =========================
    [Header("일반스킬 사용할 때의 발사체 게임 오브젝트 프리팹 생성 위치")]
    public Transform _projectile_startPos;   // 스킬 공격 가능 불가능 확인하는 변수

    // ===================================================

    [Header("적을 탐지했는지 확인하는 변수")]
    public bool _isSearch = false;   // 적을 탐지했는지 확인하는 변수

    [Header("유닛 행동 가능 불가능 체크하는 변수")]
    public bool canAct;

    [Header("기본 스킬 가능 불가능 확인하는 변수")]
    public bool _can_genSkill_Attack;    // 기본 공격 가능 불가능 확인하는 변수

    [Header("특수 스킬 공격 가능 불가능 확인하는 변수")]
    public bool _can_SpcSkill_Attack;   // 스킬 공격 가능 불가능 확인하는 변수

    private void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _anim=GetComponent<Animator>();
        _projectile_Bullet = GetComponent<Abs_Bullet>();

        // 유닛 방어구 속성 데미지 계산을 위한 스크립트 할당
        //_this_Unit_ArmorCalculateCs = UnitDataManager.Instance._armorCs_Dictionary[_unitData._eUnit_Defense_Property];

        gen_skill = GetComponents<AbsNewSKill>()[0];
        spe_skill_1 = GetComponents<AbsNewSKill>()[1];
        spe_skill_2 = GetComponents<AbsNewSKill>()[1];
    }

    private void Start()
    {
        _nav.enabled = true;

        // 유닛 초기값 할당하는 함수 호출
        SetStructValue(UnitDataManager.Instance._unitInfo_Dictionary["hum_arch01"]);
    }


    void Update()
    {
        Unit_Attack_Skill_CoolTime();
    }

    #region # Unit_Attack_Skill_CoolTime() : 유닛 기본공격, 스킬공격 쿨타임 돌려주는 함수
    public void Unit_Attack_Skill_CoolTime()
    {
        // 기본 공격이 가능한지 확인
        _can_genSkill_Attack = _unitData._unit_Attack_CoolTime >= _unitData._unit_Attack_Speed ? true : false;

        // 스킬 공격이 가능한지 확인
        _can_SpcSkill_Attack = _unitData._unit_Current_Skill_CoolTime >= _unitData._unit_Skill_CoolTime ? true : false;


        //현재 기본 공격 쿨타임이 유닛의 기본 공격속도 보다 낮다면 쿨타임 돌려주기
        if (_unitData._unit_Attack_Speed >= _unitData._unit_Attack_CoolTime)
        {
            _unitData._unit_Attack_CoolTime += Time.deltaTime;
        }

        //현재 스킬 공격 쿨타임이 유닛의 스킬 공격 쿨타임 보다 낮다면 쿨타임 돌려주기
        if (_unitData._unit_Skill_CoolTime >= _unitData._unit_Current_Skill_CoolTime)
        {
            _unitData._unit_Current_Skill_CoolTime += Time.deltaTime;
        }

    }
    #endregion

    #region # SetStructValue() 함수 : 유닛 데이터 Json 파싱 후 값 할당
    public void SetStructValue(CharacterData character_Data)
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
        _unitData.sightRange = 10f;

        // 공격 범위
        _unitData.attackRange = 8f;

        // 크리티컬 확률
        _unitData.criticRate = character_Data.criticRate;

        // 일반스킬
        _unitData.generalSkill = character_Data.generalSkill;

        // 일반스킬 이름
        _unitData.generalSkillName = character_Data.generalSkillName;

        if (gen_skill == null)
        {
            gen_skill = Instantiate(character_Data.new_unit_Gen_Skill, transform);
            gen_skill.gameObject.name = _unitData.generalSkillName;
            gen_skill._link_Skill = character_Data.unit_Gen_Skill._link_Skill;
            //gen_skill.unitInfoCs = this;

        }

        // 특수 스킬1 , 자유모드 일 때 사용하는 스킬
        _unitData.specialSkill1 = character_Data.specialSkill1;

        _unitData.specialSkill1Name = character_Data.specialSkill1Name;

        if (spe_skill_1 == null)
        {
            spe_skill_1 = Instantiate(character_Data.new_unit_Spc_Skill, transform);
            spe_skill_1.gameObject.name = _unitData.specialSkill1Name;
            spe_skill_1._link_Skill = character_Data.unit_Spc_Skill._link_Skill;
            //spe_skill_1.unitInfoCs = this;

        }


        // 특수 스킬 2 , 홀드모드 일 때 사용하는 스킬
        _unitData.specialSkill2 = character_Data.specialSkill2;

        _unitData.specialSkill2Name = character_Data.specialSkill2Name;

        if (spe_skill_2 == null)
        {
            spe_skill_2 = Instantiate(character_Data.new_unit_Spc_Skill2, transform);
            spe_skill_2.gameObject.name = _unitData.specialSkill2Name;
            spe_skill_2._link_Skill = character_Data.unit_Spc_Skill2._link_Skill;
            //spe_skill_2.unitInfoCs = this;

        }


        // 발사체 셋팅 준비하는 함수
        ReadyProjectile();

        // 기본스킬, 특수스킬 1, 2 의 unitTargetSearchCs 값 할당
        //gen_skill.unitTargetSearchCs = unitTargetSearchCs;
        //spe_skill_1.unitTargetSearchCs = unitTargetSearchCs;
        //spe_skill_2.unitTargetSearchCs = unitTargetSearchCs;


        // 유닛 방어구 속성 할당
        _unitData._eUnit_Defense_Property = UnitDataManager.Instance._armor_Dictionary[_unitData.defenseType];

        // 유닛 방어구 속성 데미지 계산을 위한 스크립트 할당
        _this_Unit_ArmorCalculateCs = UnitDataManager.Instance._armorCs_Dictionary[_unitData._eUnit_Defense_Property];

        // 유닛 타겟 설정 타입 할당
        print(character_Data.targetSelectType);
        _unitData._unit_targetSelectType = UnitDataManager.Instance._targetSelect_Dictionary[character_Data.targetSelectType];
        //_eUnit_Target_Search_Type = UnitDataManager.Instance._targetSelect_Dictionary[character_Data.targetSelectType];


        // 유닛 자유 모드
        _enum_Unit_Action_Mode = eUnit_Action_States.unit_FreeMode;

        // 유닛 기본 대기 상태
        _enum_Unit_Action_State = eUnit_Action_States.unit_Idle;

        // 유닛 추격 상태
        //_enum_Unit_Attack_State = eUnit_Action_States.unit_Tracking;

        // 근접 공격 유닛 (나중에 없애도 됨)
        //_enum_Unit_Attack_Type = eUnit_Action_States.long_Range_Atk;

        _unitData._eUnit_genSkill_Property = eUnit_Attack_Property_States.slash_Attack;      // 유닛 공격속성
        _unitData._unit_General_Skill_Dmg = 3f;                                                  // 유닛 공격 데미지
        _unitData._unit_Special_Skill_Dmg = 6f;                                            // 유닛 공격 데미지
        _unitData._unit_MoveSpeed = 1f;                                                      // 유닛 이동속도
        _unitData._unit_Attack_Speed = 3f;                                                   // 유닛 공격 속도
        _unitData._unit_Attack_CoolTime = 3f;                                                // 유닛 기본 공격 쿨타임
        _unitData._unit_Skill_CoolTime = 5f;                                                 // 유닛 스킬 공격 쿨타임

        //holdObj.gameObject.SetActive(false);

    }
    #endregion



    #region # SetStructValue() 함수 : 활성화 시 필요한 초기 데이터 값 부여하는 함수
    // 활성화 시 필요한 초기 데이터 값 부여하는 함수
    public void SetUnitValue() // 이벤트로 만들기
    {
        canAct = true;
        //sprCol.enabled = true;
        _isSearch = false;
        //_isDead = false;

        _nav.speed = 3.5f;
        _nav.acceleration = 8f;

    }
    #endregion


    #region # ReadyProjectile() : 발사체 셋팅 준비하는 함수
    private void ReadyProjectile()
    {
        if (_projectile_Bullet==null)
            return;

        _projectile_Bullet.newUnitInfoCs = this;

        //일반 스킬 발사체 준비
        gen_skill._projectile_Prefab = Instantiate(_projectile_Bullet.gameObject);
        //gen_skill._projectile_Prefab.GetComponent<Abs_Bullet>().unitInfoCs = this;

        gen_skill._projectile_Prefab.GetComponent<Abs_Bullet>().atkDmg = _unitData._unit_General_Skill_Dmg;
        gen_skill._projectile_Prefab.GetComponent<Abs_Bullet>()._start_Pos = _projectile_startPos;

        gen_skill._projectile_Prefab.GetComponent<Abs_Bullet>()._newSkill = gen_skill;

        //자유모드 - 특수 스킬 1 발사체 준비
        spe_skill_1._projectile_Prefab = Instantiate(_projectile_Bullet.gameObject);
        //spe_skill_1._projectile_Prefab.GetComponent<Abs_Bullet>().unitInfoCs = this;

        spe_skill_1._projectile_Prefab.GetComponent<Abs_Bullet>().atkDmg = _unitData._unit_Special_Skill_Dmg;
        spe_skill_1._projectile_Prefab.GetComponent<Abs_Bullet>()._start_Pos = _projectile_startPos;

        spe_skill_1._projectile_Prefab.GetComponent<Abs_Bullet>()._newSkill = spe_skill_1;


        //홀드모드 - 특수 스킬 2 발사체 준비
        spe_skill_2._projectile_Prefab = Instantiate(_projectile_Bullet.gameObject);
        //spe_skill_2._projectile_Prefab.GetComponent<Abs_Bullet>().unitInfoCs = this;

        spe_skill_2._projectile_Prefab.GetComponent<Abs_Bullet>().atkDmg = _unitData._unit_Special_Skill_Dmg;
        spe_skill_2._projectile_Prefab.GetComponent<Abs_Bullet>()._start_Pos = _projectile_startPos;

        spe_skill_2._projectile_Prefab.GetComponent<Abs_Bullet>()._newSkill = spe_skill_2;
    }
    #endregion


    public void StopUnitAct()
    {

        _isSearch = false;
        canAct = false;
        //unitTargetSearchCs._targetUnit = null;
        //unitTargetSearchCs._target_Body = null;
        //_enum_Unit_Action_Mode = _enum_mUnit_Action_BaseMode;
        //_enum_Unit_Action_State = _enum_mUnit_Action_BaseState;
        this.enabled = false;
        //actUnitCs.enabled = false;
        _nav.enabled = false;
        _anim.enabled = false;
        //unitTargetSearchCs.enabled = false;
    }


    #region # InitUnitInfoSetting(): 유닛 정보 셋팅하는 함수
    public void InitUnitInfoSetting(CharacterData character_Data)
    {
        if (Castle.Instance._castle_Hp.Equals(0))
        {
            StopUnitAct();
        }
        // 성 무너졌을 때 기본 상태로 변환되는 이벤트 함수 연결
        Castle.Instance.OnCastleDown += StopUnitAct;

        // 활성화 시 필요한 초기 데이터 값 부여하는 함수
        //SetUnitValue();

        // 유닛 데이터 Json 파싱 후 값 할당
        SetStructValue(character_Data);
    }
    #endregion
}
