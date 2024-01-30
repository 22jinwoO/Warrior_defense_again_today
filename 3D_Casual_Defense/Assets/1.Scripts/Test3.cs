using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Test3
{
    [Header("유닛 데이터 구조체 변수")]
    public unit_Data _unitData; // 유닛 데이터 구조체 변수

    [Header("유닛 행동 모드 상태 변수")]
    public eUnit_Action_States _enum_Unit_Action_Mode;   // 유닛 모드 상태 변수

    [Header("유닛 행동 상태 변수")]
    public eUnit_Action_States _enum_Unit_Action_State;   // 유닛 행동 상태 변수

    [Header("플레이어 유닛 기본 모드 상태 변수")]
    public eUnit_Action_States _enum_pUnit_Action_BaseMode = eUnit_Action_States.unit_FreeMode;   // 플레이어 유닛 기본 모드 상태 변수

    [Header("플레이어 유닛 기본 행동 상태 변수")]
    public eUnit_Action_States _enum_pUnit_Action_BaseState = eUnit_Action_States.unit_Idle;   // 플레이어 유닛 기본 행동 상태 변수

    [Header("몬스터 유닛 기본 모드 상태 변수")]
    public eUnit_Action_States _enum_mUnit_Action_BaseMode = eUnit_Action_States.monster_NormalPhase;   // 유닛 모드 상태 변수

    [Header("몬스터 유닛 기본 모드 상태 변수")]
    public eUnit_Action_States _enum_mUnit_Action_BaseState = eUnit_Action_States.unit_Move;   // 유닛 행동 상태 변수


    [Header("플레이어가 지정해준 현재 유닛 공격 상태 변수")]
    public eUnit_Action_States _enum_Unit_Attack_State;   // 플레이어가 지정해준 현재 유닛 공격 상태 변수

    [Header("유닛의 공격 상태 (근거리, 원거리)")]
    public eUnit_Action_States _enum_Unit_Attack_Type;   // 플레이어가 지정해준 현재 유닛 공격 상태 변수

    [Header("유닛의 타겟 선정 타입")]
    public eUnit_targetSelectType _eUnit_Target_Search_Type;   // 유닛의 타겟 선정 타입

    [Header("유닛 방어구 속성")]
    public ArmorCalculate _this_Unit_ArmorCalculateCs;

    [Header("유닛의 타겟 선정 타입마다 필요한 함수들을 구현해놓은 UnitTargetSearch 스크립트")]
    [SerializeField]
    public UnitTargetSearch unitTargetSearchCs;

    [Header("유닛의 행동들을 구현해놓은 ActUnit 스크립트")]
    [SerializeField]
    public ActUnit actUnitCs;

    [Header("유닛 바디 트랜스폼")]
    public Transform body_Tr;    // 유닛 바디 트랜스폼

    [Header("내비메쉬 에이전트")]
    public NavMeshAgent _nav;    // 내비메쉬

    [Header("애니메이터")]
    public Animator _anim;       // 애니메이터

    [Header("유닛을 클릭했는지 확인하는 변수")]
    public bool _isClick;       // 유닛을 클릭했는지 확인하는 변수

    [Header("유닛이 죽었는지 확인하는 변수")]
    public bool _isDead;       // 유닛이 죽었는지 확인하는 변수

    [Header("타겟이 죽었는지 확인하는 변수")]
    public bool _isTargetDead;       // 타겟이 죽었는지 확인하는 변수

    [Header("적을 탐지했는지 확인하는 변수")]
    public bool _isSearch = false;   // 적을 탐지했는지 확인하는 변수

    [Header("유닛이 도착할 위치를 의미하는 벡터변수")]
    public Vector3 _movePos;

    [Header("기본 스킬 가능 불가능 확인하는 변수")]
    public bool _can_genSkill_Attack;    // 기본 공격 가능 불가능 확인하는 변수

    [Header("특수 스킬 공격 가능 불가능 확인하는 변수")]
    public bool _can_SpcSkill_Attack;   // 스킬 공격 가능 불가능 확인하는 변수


    // 게임 이펙트 부분*********************************

    [Header("피격시 생성되는 이펙트- 0 : 베기 공격 / 1 : 관통 공격 / 2: 분쇄 공격")]
    public GameObject[] _hit_Effects = new GameObject[3];   // 스킬 공격 가능 불가능 확인하는 변수

    [Header("베기 공격 피격시 생성되는 이펙트 오브젝트 풀링")]
    public List<GameObject> _hit_Effect_SlashAtk_Vfxs;   // 스킬 공격 가능 불가능 확인하는 변수

    [Header("관통 공격 피격 시 생성되는 이펙트 오브젝트 풀링")]
    public List<GameObject> _hit_Effect_PierceAtk_Vfxs;   // 스킬 공격 가능 불가능 확인하는 변수

    [Header("분쇄 공격 피격 시 생성되는 이펙트 오브젝트 풀링")]
    public List<GameObject> _hit_Effect_CrushAtk_Vfxs;   // 스킬 공격 가능 불가능 확인하는 변수

    // 게임 이펙트 부분=====================================

    // 발사체 프리팹 *************************
    [Header("일반스킬 사용할 때의 발사체 게임 오브젝트 프리팹")]
    public GameObject _projectile_Prefab;   // 스킬 공격 가능 불가능 확인하는 변수

    public Abs_Bullet _projectile_Bullet;   // 스킬 공격 가능 불가능 확인하는 변수

    // 발사체 프리팹 =========================
    [Header("일반스킬 사용할 때의 발사체 게임 오브젝트 프리팹 생성 위치")]
    public Transform _projectile_startPos;   // 스킬 공격 가능 불가능 확인하는 변수

    // 스킬 1,2 ===============================

    [Header("일반 스킬")]
    public Abs_Skill gen_skill;

    [Header("특수 스킬 - 1")]
    public Abs_Skill spe_skill_1;

    [Header("특수 스킬 - 2")]
    public Abs_Skill spe_skill_2;

    [Header("유닛 행동 가능 불가능 체크하는 변수")]
    public bool canAct;

    [Header("일반 상태 유닛 머태리얼")]
    public Material _unit_NomralMtr;

    [Header("유닛 투명화 머태리얼")]
    public Material _unit_CloakingMtr;

    [Header("피격 머태리얼")]
    public Material _damaged_Mtr;

    [Header("바디 이외의 메쉬렌더러 변수")]
    public MeshRenderer[] someMeshReners;

    [Header("바디 메쉬 렌더러 변수")]
    public SkinnedMeshRenderer bodyMeshRener;

    public Material[] someMtr;
    public Material bodyMtr;

    public Material[] cloaking_someMtr;
    public Material cloaking_bodyMtr;

    [Header("스피어 콜라이더")]
    public SphereCollider sprCol;


    public AudioSource atkSound;

}
