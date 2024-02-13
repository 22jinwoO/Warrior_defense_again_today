using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;
using static UnitDataManager;
using UnityEngine.EventSystems;


public class Archer : PlayerUnitClass
{
    //
    [SerializeField]
    private bool isCollision;

    [SerializeField]
    private float maxdistance;

    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    public Player player;
    private void Awake()
    {
        someMtr = new Material[someMeshReners.Length];
        soundPos = GameObject.FindGameObjectWithTag("SoundPos").transform;
        player= GameObject.Find("Player").GetComponent<Player>();

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


        //_unit_CloakingMtr = Instantiate(_unit_CloakingMtr);

        //bodyMeshRener.material = Instantiate(_unit_CloakingMtr);

        Init_Vfx();

        sprCol = GetComponent<SphereCollider>();

        //aaa = GetComponent<Material>();
        navObs = GetComponent<NavMeshObstacle>();
        _anim = GetComponent<Animator>();
        _this_Unit_ArmorCalculateCs = new PaddingArmor();
        _nav = GetComponent<NavMeshAgent>();
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();
        actUnitCs = GetComponent<ActUnit>();
        _isClick = false;
        //InitUnitInfoSetting();  // 유닛 정보 초기화 시켜주는 함수
        //gen_skill._projectile_Prefab = Instantiate(_projectile_Prefab);
        //gen_skill._projectile_Prefab.SetActive(false);

        // 사운드 오디오 소스 할당
        atkSound = GetComponents<AudioSource>()[0];
        hitSound = GetComponents<AudioSource>()[1];


    }

    private void Start()
    {
        gen_skill._projectile_Prefab = Instantiate(_projectile_Prefab);
        gen_skill._projectile_Prefab.GetComponent<Abs_Bullet>().unitInfoCs = this;

        gen_skill._projectile_Prefab.GetComponent<Abs_Bullet>().atkDmg = _unitData._unit_General_Skill_Dmg;
        gen_skill._projectile_Prefab.GetComponent<Abs_Bullet>()._start_Pos = _projectile_startPos;

        gen_skill._projectile_Prefab.GetComponent<Abs_Bullet>()._skill = gen_skill;

        //transform.eulerAngles = Vector3.zero;
        //_nav.SetDestination(Castle.Instance.transform.position);
        //print(_unitData.sightRange);
        //Instantiate(_projectile_Prefab);
    }
    // Update is called once per frame
    void Update()
    {
        if (unitTargetSearchCs._targetUnit != null && unitTargetSearchCs._targetUnit.GetComponent<SphereCollider>().enabled.Equals(false))
        {
            _isSearch = false;
            unitTargetSearchCs._targetUnit = null;
            unitTargetSearchCs._target_Body = null;
            _enum_Unit_Action_Mode = _enum_pUnit_Action_BaseMode;
            _enum_Unit_Action_State = _enum_pUnit_Action_BaseState;
        }

        //if (_isClick && Input.GetMouseButtonDown(1))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out RaycastHit hit))
        //    {
        //        _movePos = hit.point;
        //        _enum_Unit_Action_State = eUnit_Action_States.unit_Move;

        //    }
        //}

        //if (_isClick)
        //{
        //    Touch touch = Input.GetTouch(0);

        //    if (player.isChoice && touch.phase == TouchPhase.Moved && Input.touchCount == 1)
        //    {
        //        player.textMeshProUGUI.text = "드래그중";

        //        Vector3 mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //        if (player.isChoice && touch.phase == TouchPhase.Ended && Input.touchCount == 1)
        //        {
        //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //            if (Physics.Raycast(ray, out RaycastHit hit))
        //            {
        //                mouseInput=new Vector3(mouseInput.x, 0f, mouseInput.z);
        //                _movePos = mouseInput;
        //                _enum_Unit_Action_State = eUnit_Action_States.unit_Move;
        //                player.textMeshProUGUI.text = gameObject.name + "\n사용자 지정 가능여부 " + player.isChoice + "\n 유닛이동 가능 여부 " + _isClick + " 이동 : " + hit.point + "\n월드좌표 : " + mouseInput;
        //                player.isChoice = false;
        //                _isClick = false;
        //            }

        //        }
        //    }


        //    //Vector3 mouseInput = Camera.main.ScreenToWorldPoint(hit.point);

        //    //if (player.isChoice&&touch.phase==TouchPhase.Ended&&Input.touchCount == 1)
        //    //{
        //    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    //    if (Physics.Raycast(ray, out RaycastHit hit))
        //    //    {
        //    //        player.isChoice = false;

        //    //        Vector3 mouseInput = Camera.main.ScreenToWorldPoint(hit.point);

        //    //        _movePos = mouseInput;
        //    //        _enum_Unit_Action_State = eUnit_Action_States.unit_Move;
        //    //        player.textMeshProUGUI.text = gameObject.name + "\n사용자 지정 가능여부 " + player.isChoice + "\n 유닛이동 가능 여부 " + _isClick + " 이동 : " + hit.point + "\n월드좌표 : " + mouseInput;

        //    //    }

        //    //}
        //}

        //Instantiate(_projectile_Prefab);
        Unit_Attack_Skill_CoolTime();   // 유닛 기본 공격, 스킬 공격 쿨타임 돌려주는 함수

    }

    //IEnumerator Get_DamagedBody()
    //{
    //    print("V키 눌림");
    //    //Material asdf = GetComponentInChildren<Material>();

    //    //Material asdfd= aaa;
    //    for (int i = 0; i < someMaterials.Length; i++)
    //    {
    //        someMaterials[i].material.color = Color.gray;

    //    }
    //    bodyMaterials.material.color = Color.gray;

    //    // aaa = asdf;
    //    yield return new WaitForSeconds(0.1f);
    //    for (int i = 0; i < someMaterials.Length; i++)
    //    {
    //        someMaterials[i].material.color = Color.white;
    //        someMaterials[i].material.color = Color.white;

    //    }
    //    bodyMaterials.material.color = Color.white;

    //}

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
        canAct = true;
        Debug.LogWarning(character_Data.unit_Gen_Skill._link_Skill.link_name);
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
        gen_skill = Instantiate(character_Data.unit_Gen_Skill,transform);
        gen_skill.gameObject.name = _unitData.generalSkillName;
        gen_skill._link_Skill= character_Data.unit_Gen_Skill._link_Skill;
        gen_skill.unitInfoCs = this;
        Debug.LogWarning(character_Data.unit_Gen_Skill._link_Skill.link_name);
        Debug.LogWarning(gen_skill._link_Skill.link_name);
        print(gen_skill.unitInfoCs);


        // 특수 스킬 , 자유모드 일 때 사용하는 스킬
        _unitData.specialSkill1 = character_Data.specialSkill1;

        // 특수 스킬 1 이름
        _unitData.specialSkill1Name = character_Data.specialSkill1Name;

        // 특수 스킬 , 홀드모드 일 때 사용하는 스킬
        _unitData.specialSkill2 = character_Data.specialSkill2;

        // 특수 스킬 2 이름
        _unitData.specialSkill2Name = character_Data.specialSkill2Name;

        // 유닛 타겟 설정 타입
        _unitData.targetSelectType = character_Data.targetSelectType;

        //// 일반스킬 할당
        //gen_skill = character_Data.unit_Gen_Skill;

        // 유닛 방어구 속성 할당
        _unitData._eUnit_Defense_Property = UnitDataManager.Instance._armor_Dictionary[_unitData.defenseType];

        // 유닛 방어구 속성 데미지 계산을 위한 스크립트 할당
        _this_Unit_ArmorCalculateCs = UnitDataManager.Instance._armorCs_Dictionary[_unitData._eUnit_Defense_Property];

        // 유닛 타겟 설정 타입 할당
        _unitData._unit_targetSelectType = UnitDataManager.Instance._targetSelect_Dictionary[_unitData.targetSelectType];
        _eUnit_Target_Search_Type = UnitDataManager.Instance._targetSelect_Dictionary[_unitData.targetSelectType];


        // 유닛 자유 모드
        _enum_Unit_Action_Mode = eUnit_Action_States.unit_FreeMode;

        // 유닛 기본 대기 상태
        _enum_Unit_Action_State = eUnit_Action_States.unit_Idle;

        // 유닛 추격 상태
        _enum_Unit_Attack_State = eUnit_Action_States.unit_Tracking;

        // 근접 공격 유닛 (나중에 없애도 됨)
        _enum_Unit_Attack_Type = eUnit_Action_States.long_Range_Atk;

        //_unitData._unit_Name = "궁수";                                                        // 유닛 이름
        //_unitData._unit_maxHealth = 200f;                                                       // 유닛 체력
        _unitData._eUnit_genSkill_Property = eUnit_Attack_Property_States.slash_Attack;      // 유닛 공격속성
        _unitData._unit_General_Skill_Dmg = 1f;                                                  // 유닛 공격 데미지
        _unitData._unit_Special_Skill_Dmg = 6f;                                            // 유닛 공격 데미지
        //_unitData._eUnit_Defense_Property = eUnit_Defense_Property_States.padding_Armor;    // 유닛 방어속성
        //_unitData._unit_Description = "궁수입니다";                                           // 유닛 설명
        //_unitData._unit_Type = "궁수";                                                       // 유닛 타입
        _unitData._unit_MoveSpeed = 1f;                                                      // 유닛 이동속도
        //_unitData.sightRange = _unitData.sightRange;                                                     // 유닛 시야
        //_unitData.attackRange = _unitData.attackRange;                                                   // 유닛 공격 범위
        _unitData._unit_Attack_Speed = 3f;                                                   // 유닛 공격 속도
        _unitData._unit_Attack_CoolTime = 5f;                                                // 유닛 기본 공격 쿨타임
        _unitData._unit_Skill_CoolTime = 8f;                                                 // 유닛 스킬 공격 쿨타임

        //_enum_Unit_Action_Mode = eUnit_Action_States.unit_FreeMode;
        //_enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
        //_enum_Unit_Attack_State = eUnit_Action_States.unit_Tracking;
        //_enum_Unit_Attack_Type = eUnit_Action_States.long_Range_Atk;
        //_eUnit_Target_Search_Type = eUnit_targetSelectType.fixed_Target;

    }
    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _unitData.sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _unitData.attackRange);
    }


}
