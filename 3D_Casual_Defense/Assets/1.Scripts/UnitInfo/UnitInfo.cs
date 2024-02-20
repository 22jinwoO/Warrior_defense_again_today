//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
//      | 유닛 정보를 하나의 클래스로 만들어서 한번에 관리|
//       ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Runtime.InteropServices.WindowsRuntime;


// 함수이름은 명사말고 동사 먼저, enum은 변수이름 앞에 소문자 e 작성, 변수는 카멜표기법으로 소문자 이후 단어 첫글자 대문자
[Serializable]
public struct unit_Data    // 유닛 데이터 가져오는 구조체
{
    [Header("유닛 이름")]
    public string _unit_Name;                           // 유닛 이름
    //public float _unit_maxHealth;                       // 유닛 최대 체력
    //public float _unit_currentHealth;                   // 유닛 현재 체력 유닛 최대체력 넣어주기
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

    //public int sightRange;   // 시야 범위
    //public int attackRange;   // 공격 범위

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

public abstract class UnitInfo : MonoBehaviour
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

    [Header("피격 사운드 - 0 : 베기 공격 / 1 : 관통 공격 / 2: 분쇄 공격")]
    [SerializeField]
    private AudioClip[] hit_Sfxs;

    [Header("기절 상태이상 발생 이펙트")]
    public GameObject _status_Effect_Stun_Start;   // 스킬 공격 가능 불가능 확인하는 변수

    [Header("출혈 상태이상 발생 이펙트")]
    public GameObject _status_Effect_Bleeding_Start;   // 스킬 공격 가능 불가능 확인하는 변수

    [Header("기절 상태이상 이펙트")]
    public GameObject _status_Effect_Stun;   // 스킬 공격 가능 불가능 확인하는 변수

    [Header("중독 상태이상 이펙트")]
    public GameObject _status_Effect_Poison;   // 스킬 공격 가능 불가능 확인하는 변수

    [Header("출혈 상태이상 이펙트")]
    public GameObject _status_Effect_Bleeding;   // 스킬 공격 가능 불가능 확인하는 변수

    [Header("이속감소 상태이상 이펙트")]
    public GameObject _status_Effect_Slow;   // 스킬 공격 가능 불가능 확인하는 변수


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

    [Header("공격 사운드")]
    [SerializeField]
    public AudioSource atkSound;

    [Header("피격 사운드")]
    [SerializeField]
    public AudioSource hitSound;

    [SerializeField]
    public Transform soundPos;

    // 사운드 *************************

    // 사운드 =========================

    // 히트 시 출력되는 텍스트 *************************

    // 히트 시 출력되는 텍스트 =========================

    #region # Unit_Attack_Skill_CoolTime() : 유닛 기본공격, 스킬공격 쿨타임 돌려주는 함수
    public void Unit_Attack_Skill_CoolTime()
    {
        // 기본 공격이 가능한지 확인
        _can_genSkill_Attack = _unitData._unit_Attack_CoolTime >= _unitData._unit_Attack_Speed ? true : false;

        // 스킬 공격이 가능한지 확인
        _can_SpcSkill_Attack = _unitData._unit_Current_Skill_CoolTime >= _unitData._unit_Skill_CoolTime ? true : false;

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


    public IEnumerator TargetDead()
    {
        yield return null;

        yield return null;
        _isTargetDead = true;

        _isSearch = false;

        yield return null;
        unitTargetSearchCs._targetUnit = null;
        unitTargetSearchCs._target_Body = null;

        Debug.LogWarning(_isSearch);

        gen_skill.unitTargetSearchCs._targetUnit = null;
        gen_skill.unitTargetSearchCs._target_Body = null;

        Debug.LogWarning(gameObject.name + ": 타겟 비우기");
        Debug.LogWarning(_isSearch);
        Debug.LogWarning(actUnitCs.unitTargetSearchCs._targetUnit);
        Debug.LogWarning(actUnitCs.unitTargetSearchCs._target_Body);
        yield return null;

        _isSearch = false;
        _isTargetDead = false;

        if (gameObject.tag.Equals("Player"))
        {
            _enum_Unit_Action_Mode = _enum_pUnit_Action_BaseMode;
            _enum_Unit_Action_State = _enum_pUnit_Action_BaseState;
        }
        else
        {
            _enum_Unit_Action_Mode = _enum_mUnit_Action_BaseMode;
            _enum_Unit_Action_State = _enum_mUnit_Action_BaseState;
        }
        Debug.LogWarning(_isSearch);


    }

    // 이펙트 오브젝트 풀링
    public void Init_Vfx()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject vfx = Instantiate(_hit_Effects[0], transform);
            _hit_Effect_SlashAtk_Vfxs.Add(vfx);
            vfx.SetActive(false);
        }

        for (int i = 0; i < 4; i++)
        {
            GameObject vfx = Instantiate(_hit_Effects[1], transform);
            _hit_Effect_PierceAtk_Vfxs.Add(vfx);
            vfx.SetActive(false);
        }

        for (int i = 0; i < 4; i++)
        {
            GameObject vfx = Instantiate(_hit_Effects[2], transform);
            _hit_Effect_CrushAtk_Vfxs.Add(vfx);
            vfx.SetActive(false);
        }

        GameObject vfx1 = Instantiate(_status_Effect_Stun_Start,transform);   // 스킬 공격 가능 불가능 확인하는 변수
        vfx1.SetActive(false);
        _status_Effect_Stun_Start = vfx1;

        GameObject vfx2 = Instantiate(_status_Effect_Bleeding_Start, transform);   // 스킬 공격 가능 불가능 확인하는 변수
        vfx2.SetActive(false);
        _status_Effect_Bleeding_Start = vfx2;

        GameObject vfx3 = Instantiate(_status_Effect_Stun, transform);   // 스킬 공격 가능 불가능 확인하는 변수
        vfx3.SetActive(false);
        _status_Effect_Stun = vfx3;


        GameObject vfx4 = Instantiate(_status_Effect_Poison, transform);   // 스킬 공격 가능 불가능 확인하는 변수
        vfx4.SetActive(false);
        _status_Effect_Poison = vfx4;

        GameObject vfx5 = Instantiate(_status_Effect_Bleeding, transform);   // 스킬 공격 가능 불가능 확인하는 변수
        vfx5.SetActive(false);
        _status_Effect_Bleeding = vfx5;

        // 슬로우 이펙트 미구현
        //GameObject vfx6 = Instantiate(_status_Effect_Slow, transform);   // 스킬 공격 가능 불가능 확인하는 변수
        //vfx6.SetActive(false);
        //_status_Effect_Slow = vfx6;
    }


    public IEnumerator Damaged_Vfx_On(Abs_Skill atkSkill)
    {
        GameObject vfx = null;  // 피격 이펙트
        bool canUse = false;    // 이펙트 오브젝트 풀링 값 사용 가능한지 판단하는 bool 자료형
        AudioClip hitSfx = null;    // 이펙트 오브젝트 풀링 값 사용 가능한지 판단하는 bool 자료형


        switch (atkSkill._skill_AtkType)
        {
            case eUnit_Attack_Property_States.Default:
                break;

            //베기 공격 시 피격 이펙트
            case eUnit_Attack_Property_States.slash_Attack:

                //GameObject vfx1 = null;
                //vfx1 = Instantiate(_hit_Effect_SlashAtk, (transform.position + Vector3.up * 0.5f) + -sfxPos * 0.7f, Quaternion.identity);
                hitSfx = hit_Sfxs[0];
                for (int i = 0; i < _hit_Effect_SlashAtk_Vfxs.Count; i++)
                {
                    if (!_hit_Effect_SlashAtk_Vfxs[i].activeSelf)
                    {
                        vfx = _hit_Effect_SlashAtk_Vfxs[i];
                        canUse = true;
                        break;
                    }
                    yield return null;
                }


                if (!canUse)
                {
                    vfx = Instantiate(_hit_Effects[0], transform);
                    _hit_Effect_SlashAtk_Vfxs.Add(vfx);
                    vfx.SetActive(false);
                }
                //vfx1.transform.SetParent(transform);
                break;

            //관통 공격 시 피격 이펙트
            case eUnit_Attack_Property_States.piercing_Attack:
                hitSfx = hit_Sfxs[1];

                for (int i = 0; i < _hit_Effect_PierceAtk_Vfxs.Count; i++)
                {
                    if (!_hit_Effect_PierceAtk_Vfxs[i].activeSelf)
                    {
                        vfx = _hit_Effect_PierceAtk_Vfxs[i];
                        canUse = true;

                        break;
                    }
                    yield return null;
                }
                if (!canUse)
                {
                    vfx = Instantiate(_hit_Effects[1], transform);
                    _hit_Effect_PierceAtk_Vfxs.Add(vfx);
                    vfx.SetActive(false);
                }
                break;

            //분쇄 공격 시 피격 이펙트
            case eUnit_Attack_Property_States.crushing_attack:
                hitSfx = hit_Sfxs[2];

                for (int i = 0; i < _hit_Effect_CrushAtk_Vfxs.Count; i++)
                {
                    if (!_hit_Effect_CrushAtk_Vfxs[i].activeSelf)
                    {
                        vfx = _hit_Effect_CrushAtk_Vfxs[i];
                        canUse = true;

                        break;
                    }
                    yield return null;
                }

                if (!canUse)
                {
                    vfx = Instantiate(_hit_Effects[2], transform);
                    _hit_Effect_CrushAtk_Vfxs.Add(vfx);
                    vfx.SetActive(false);
                }
                break;

            default:
                break;
        }

        //히트 사운드 피치 조절
        hitSound.pitch = UnityEngine.Random.Range(0.7f, 1.4f);

        // 히트 사운드 볼륨 조절
        hitSound.volume = SoundManager.Instance.VolumeCheck(transform);

        yield return null;

        hitSound.PlayOneShot(hitSfx);


        Vector3 direction = atkSkill.unitInfoCs.transform.position - transform.position;
        print(transform.gameObject);
        Quaternion vfxRot;
        vfxRot = Quaternion.LookRotation(direction.normalized);

        //vfx.transform.rotation = vfxRot;
        vfx.transform.rotation = Quaternion.Euler(0, vfxRot.eulerAngles.y, 0);


        //vfx.transform.rotation = Quaternion.Euler(0, vfxRot.eulerAngles.y, 0);
        yield return new WaitForSecondsRealtime(0.1f);
        //vfx.transform.position = (transform.position + Vector3.up * 0.5f) + sfxPos * 0.7f;
        vfx.transform.position = transform.position + Vector3.up * 0.5f + vfx.transform.forward * 0.7f;

        //vfx.transform.position = new Vector3(vfx.transform.position.x, vfx.transform.position.y, Mathf.Abs(vfx.transform.position.z));
        Debug.LogWarning("362" + vfx.transform.position);
        //vfx.transform.position = new Vector3(vfx.transform.position.x, vfx.transform.position.y, -vfx.transform.position.z);
        Debug.LogWarning("365" + vfx.transform.position);

        //vfx.transform.forward = ;

        vfx.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        vfx.SetActive(false);
    }
}

