using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Archor : PlayerUnitClass
{
    //
    [SerializeField]
    private bool isCollision;

    private void Awake()
    {
        navObs = GetComponent<NavMeshObstacle>();
        _anim = GetComponent<Animator>();
        _this_Unit_Armor_Property = new GambesonArmor();
        _nav = GetComponent<NavMeshAgent>();
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();
        actUnitCs = GetComponent<ActUnit>();
        _isClick = false;
        InitUnitInfoSetting();  // 유닛 정보 초기화 시켜주는 함수

        // 발사체 값 넣어주기
        _projectile_Prefab.GetComponent<Abs_Bullet>().atkDmg = _unitData._unit_General_Skill_Dmg;
        _projectile_Prefab.GetComponent<Abs_Bullet>().unitInfoCs = GetComponent<UnitInfo>();
    }

    private void Start()
    {
        //transform.eulerAngles = Vector3.zero;
        //_nav.SetDestination(Castle.Instance.transform.position);

        //Instantiate(_projectile_Prefab);
    }
    // Update is called once per frame
    void Update()
    {
        if (_isClick && Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _movePos = hit.point;
                _enum_Unit_Action_State = eUnit_Action_States.unit_Move;
            }
        }

        //Instantiate(_projectile_Prefab);
        Unit_Attack_Skill_CoolTime();   // 유닛 기본 공격, 스킬 공격 쿨타임 돌려주는 함수

    }

    private void FixedUpdate()
    {
        Act_By_Unit();
    }
    #region # InitUnitInfoSetting(): 유닛 정보 셋팅하는 함수
    public override void InitUnitInfoSetting()
    {
        _unitData._unit_Name = "궁수";                                                        // 유닛 이름
        _unitData._unit_maxHealth = 200f;                                                       // 유닛 체력
        _unitData._eUnit_genSkill_Property = eUnit_Attack_Property_States.slash_Attack;      // 유닛 공격속성
        _unitData._unit_General_Skill_Dmg = 1f;                                                  // 유닛 공격 데미지
        _unitData._unit_Special_Skill_Dmg = 6f;                                            // 유닛 공격 데미지
        _unitData._eUnit_Defense_Property = eUnit_Defense_Property_States.padding_Armor;    // 유닛 방어속성
        _unitData._unit_Description = "궁수입니다";                                           // 유닛 설명
        _unitData._unit_Type = "궁수";                                                       // 유닛 타입
        _unitData._unit_MoveSpeed = 1f;                                                      // 유닛 이동속도
        _unitData._unit_SightRange = 40f;                                                     // 유닛 시야
        _unitData._unit_Attack_Range = 35f;                                                   // 유닛 공격 범위
        _unitData._unit_Attack_Speed = 3f;                                                   // 유닛 공격 속도
        _unitData._unit_Attack_CoolTime = 5f;                                                // 유닛 기본 공격 쿨타임
        _unitData._unit_Skill_CoolTime = 8f;                                                 // 유닛 스킬 공격 쿨타임

        _enum_Unit_Action_Mode = eUnit_Action_States.unit_FreeMode;
        _enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
        _enum_Unit_Attack_State = eUnit_Action_States.unit_Tracking;
        _enum_Unit_Attack_Type = eUnit_Action_States.long_Range_Atk;
        _eUnit_Target_Search_Type = eUnit_targetSelectType.fixed_Target;
        
    }
    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _unitData._unit_SightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _unitData._unit_Attack_Range);
    }
}
