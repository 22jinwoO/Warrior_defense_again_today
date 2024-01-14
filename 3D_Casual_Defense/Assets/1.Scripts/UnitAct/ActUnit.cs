/*
    ===================================================
   ㅣ           유닛의 행동을 구현한 스크립트            ㅣ
    ===================================================
 
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
//
public class ActUnit : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent nav;
    
    [SerializeField]
    private UnitInfo unitInfoCs;

    //[SerializeField]
    //private unit_Data _unitData;


    [SerializeField]
    private Animator anim;

    [SerializeField]
    private UnitTargetSearch unitTargetSearchCs;

    
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        //print(_unitData._unit_Attack_CoolTime);
        anim = GetComponent<Animator>();
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();

        unitInfoCs = GetComponent<UnitInfo>();

    }
    private void Start()
    {

        //unitInfoCs._unitData = unitInfoCs._unitData;
        print(unitInfoCs._unitData.criticRate);
        print(unitInfoCs.gameObject.name);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(Get_DamagedBody());
        }
    }
    #region # Attack_Unit() : 유닛이 공격할 때 호출되는 함수
    public void Attack_Unit(eUnit_Action_States next_Action_State = eUnit_Action_States.Default)   // 매개변수로 공격 후 다음에 어떤상태로 변환할지 넣어주기
    {
        anim.ResetTrigger("isAttack");

        if (nav.enabled)    // 네비메쉬 에이전트가 활성화 되어 있다면
        {
            nav.isStopped = true;
        }
        if (unitInfoCs._can_genSkill_Attack)
        {
            if (unitInfoCs._can_SpcSkill_Attack&&gameObject.name=="기사")    // 스킬은 근거리 원거리 나눌 필요 x, UseSkill 함수 호출 플레이어 유닛이라면 
            {
                Debug.Log("스킬공격!!!");
                if (unitTargetSearchCs._targetUnit == null)
                {
                    return;
                }
                //anim.SetTrigger("isAttack");
                unitInfoCs.spe_skill_2.Attack_Skill();

                unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;

                // 몬스터 유닛일 땐 공격 후 딜레이를 주어 다음 상태로 변환
                if (gameObject.CompareTag("Monster")&&unitInfoCs._enum_Unit_Action_Mode.Equals(eUnit_Action_States.monster_NormalMode))
                {
                    StartCoroutine(Change_MonsterState(next_Action_State));
                }

                // 플레이어 유닛은 바로 다음상 상태로 변환
                else
                    unitInfoCs._enum_Unit_Action_State = next_Action_State;

                return; // 함수 탈출

            }

            else
            {
                Debug.Log("기본공격!!!");
                print("88오브젝트이름 " + unitInfoCs.gameObject.name);

                print("88크리티컬 확률 " + unitInfoCs._unitData.criticRate);

                if (unitTargetSearchCs._targetUnit == null)
                {
                    return;
                }
                anim.SetTrigger("isAttack");
                if (gameObject.tag.Equals("Monster"))
                {
                    StartCoroutine(unitTargetSearchCs._targetUnit.GetComponent<ActUnit>().Get_DamagedBody());
                }
                
                unitInfoCs._unitData._unit_Attack_CoolTime = 0f;


                // 몬스터 유닛일 땐 공격 후 딜레이를 주어 다음 상태로 변환
                if (gameObject.CompareTag("Monster") && unitInfoCs._enum_Unit_Action_Mode.Equals(eUnit_Action_States.monster_NormalMode))
                {
                    StartCoroutine(Change_MonsterState(next_Action_State));
                }
                // 플레이어 유닛은 바로 다음상 상태로 변환
                else
                    unitInfoCs._enum_Unit_Action_State = next_Action_State;

            }
            //anim.ResetTrigger("isAttack");

        }
        if (nav.enabled)    // 네비메쉬 에이전트가 활성화 되어 있다면
        {
            //print(nav.gameObject.name);
            nav.isStopped = false;  // 이동 가능 상태로 변환
        }
    }
    #endregion

    public void Anim_LongRangAtk()
    {
        //anim.SetTrigger("isAttack");

        unitInfoCs._projectile_Prefab.GetComponent<Abs_Bullet>()._target_Unit = unitTargetSearchCs._targetUnit;
        unitInfoCs._projectile_Prefab.GetComponent<Abs_Bullet>()._target_BodyTr = unitTargetSearchCs._target_Body;
        unitInfoCs._projectile_Prefab.GetComponent<Abs_Bullet>()._start_Pos = unitInfoCs._projectile_startPos;

        Instantiate(unitInfoCs._projectile_Prefab);
        unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
    }

    public void Anim_LongRang_Skill_Atk()
    {
        //anim.SetTrigger("isAttack");

        unitInfoCs._projectile_Prefab.GetComponent<Abs_Bullet>()._target_Unit = unitTargetSearchCs._targetUnit;
        unitInfoCs._projectile_Prefab.GetComponent<Abs_Bullet>()._target_BodyTr = unitTargetSearchCs._target_Body;
        unitInfoCs._projectile_Prefab.GetComponent<Abs_Bullet>()._start_Pos = unitInfoCs._projectile_startPos;

        Instantiate(unitInfoCs._projectile_Prefab);
        print("공격 실행");
        unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;
        unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
    }

    #region # Change_MonsterState(eUnit_Action_States next_Action_State) : 몬스터가 공격 후 다음 상태로 변환 시 딜레이를 주기 호출되는 함수
    IEnumerator Change_MonsterState(eUnit_Action_States next_Action_State)
    {
        unitInfoCs._isSearch = false;
        unitTargetSearchCs._targetUnit = null;
        yield return new WaitForSeconds(1.5f);

        nav.isStopped = true;

        unitInfoCs._enum_Unit_Action_State = next_Action_State;
    }
    #endregion

    #region # AnimEvent_Normal_Atk() : 일반 스킬 애니메이션 동작 시 호출되는 애니메이션 이벤트 함수
    public void AnimEvent_Normal_Atk()  // 일반 스킬 사용 시 호출되는 애니메이션
    {
        //if (!unitInfoCs._enum_Unit_Attack_Type.Equals(eUnit_Action_States.close_Range_Atk))
        //    return;
        print("애니메이션 호출 함수");

        // 여기서부터 10에서 0으로 바뀜,,
        print("165오브젝트이름 " + unitInfoCs.gameObject.name);
        print("165크리티컬 확률 " + unitInfoCs._unitData.criticRate);

        unitInfoCs.gen_skill.unitInfoCs = unitInfoCs;   // 나중에 유닛 awake 문에서 한번만 실행하도록 변경하기
        unitInfoCs.gen_skill.unitTargetSearchCs = unitTargetSearchCs;

        // 일반 스킬 사용
        unitInfoCs.gen_skill.Attack_Skill();


        ////unitTargetSearchCs._targetUnit.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(,unitTargetSearchCs._targetUnit, unitInfoCs._unitData._unit_General_Skill_Dmg);
        //unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
        //unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;


        // 스킬 공격
        //unitTargetSearchCs._targetUnit.GetComponent<ActUnit>().BeAttacked_By_OtherUnit(unitTargetSearchCs._targetUnit, unitInfoCs._unitData._unit_Special_Skill_Dmg);
        //unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;
        //unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
    }
    #endregion

    #region # float CheckCritical(Abs_Skill skill, float atkDmg) : 크리티컬 확률 체크하는 함수
    private float CheckCritical(UnitInfo attacker, Abs_Skill skill, float atkDmg)
    {
        // int 범위를 구할때는 최대값이 제외된다. (크리티컬 확률에 해당하는지 체크)
        int randNum = Random.Range(1, 101);
        print("피격자"+unitInfoCs.gameObject.name);

        print("피격자 크리티컬 확률 " + unitInfoCs._unitData.criticRate);

        print("공격자" + attacker.gameObject.name);

        print("공격자 크리티컬 확률 " + attacker._unitData.criticRate);
        //unitInfoCs._unitData.criticRate = 50;

        print("크리티컬 랜덤 숫자 " + randNum);

        // 공격자의 공격이 크리티컬 확률에 해당한다면
        if (randNum <= attacker._unitData.criticRate)
        {
            // 링크 스킬이 있다면 링크 스킬 적용
            if (skill._link_Skill != null)
            {
                skill._link_Skill.isStatusApply = true;

                UnitInfo targetUnitInfo = unitTargetSearchCs.GetComponent<UnitInfo>();

                string link_Id = skill._link_Id;
                int link_value = skill._link_Skill.linkValue_ps;
                print(link_value);
                int duration_s = skill._link_Skill.duration_s;
                print(duration_s);

                StartCoroutine(skill._link_Skill.Apply_Status_Effect(targetUnitInfo, link_Id, link_value, duration_s));
                
                // 링크스킬 적용 후 기본 공격 값 반환
            }
            //링크 스킬이 없을 경우의 크리티컬이 적용됐다면
            else
            {
                // 공격 반환 값 = 기본스킬 데미지 * 크리티컬 배율
                atkDmg = skill._base_Value * skill._critical_Dmg;
            }
            print("크리티컬 공격!!");
            //

        }
        print("공격 데미지 "+ atkDmg);
        return atkDmg;

    }
    #endregion

    #region # BeAttacked_By_OtherUnit(Transform other,float attack_Dmg) : 다른 유닛으로부터의 공격으로 피해를 입을 때 호출되는 함수
    // 기본공격 일 때와 스킬 공격 일 때 를 나눠야 함...
    public void BeAttacked_By_OtherUnit(Abs_Skill skill, eUnit_Attack_Property_States myAtkType, UnitInfo attacker, Transform other)
    {
        // 공격 받을 때 호출되는 함수라 몬스터의 크리티컬확률이 출력되서 몬스터 크리티컬 확률이 0인거임,,
        print("215크리티컬 확률 " + unitInfoCs._unitData.criticRate);

        // 피격자의 유닛 데이터 가져오기
        unit_Data attackerUnitData = other.GetComponent<UnitInfo>()._unitData;

        float attack_Dmg = skill._base_Value;

        // 피해 입을 때 몸에 피격상태 나타내주는 함수 실행
        StartCoroutine(Get_DamagedBody());

        // 데미지 계산하는 함수 실행
        unitInfoCs._unitData.hp -= unitInfoCs._this_Unit_ArmorCalculateCs
            .CalculateDamaged(attackType: myAtkType, ArmorType: attackerUnitData, attack_Dmg: CheckCritical(attacker, skill, attack_Dmg));

        //Abs_StatusEffect asd = new PoisonStatus();
        //StartCoroutine(asd.Get_Posion(other.GetComponent<UnitInfo>(), "", 2, 5));

        //print("충돌했음");
        //print(other.gameObject.name);
        //print(other.GetComponent<UnitInfo>()._unitData);
        //print(attackerUnitData._eUnit_Defense_Property);
        //print(unitInfoCs._unitData._unit_maxHealth);
        //print(attackerUnitData.);
        //print(unitInfoCs._this_Unit_ArmorCalculateCs);
        //print(attack_Dmg);
        //print(unitInfoCs._unitData._unit_maxHealth);

    }
    #endregion

    IEnumerator Get_DamagedBody()
    {
        print("피격당한 유닛 이름 : "+ gameObject.name);
        //Material asdf = GetComponentInChildren<Material>();
        //Material asdfd= aaa;
        for (int i = 0; i < unitInfoCs.someMeshReners.Length; i++)
        {
            unitInfoCs.someMeshReners[i].material.color = new Color(1f, 0.238555f, 0f);
            //0.157f
        }
        unitInfoCs.bodyMeshRener.material.color = new Color(1f, 0.238555f, 0f);

        // aaa = asdf;
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < unitInfoCs.someMeshReners.Length; i++)
        {
            unitInfoCs.someMeshReners[i].material.color = Color.white;
            unitInfoCs.someMeshReners[i].material.color = Color.white;

        }
        unitInfoCs.bodyMeshRener.material.color = Color.white;

    }

    #region # SearchTarget(매개변수 : 유닛 탐지 타입) : 유닛이 Idle 상태일 때 타겟 탐지 시 호출되는 함수
    public void SearchTarget(eUnit_targetSelectType target_Search_Type = eUnit_targetSelectType.Default)  // 유닛 탐지
    {
        switch (target_Search_Type)
        {
            case eUnit_targetSelectType.fixed_Target:
                unitTargetSearchCs.Search_For_Fixed_Target();
                break;

            case eUnit_targetSelectType.nearest_Target:
                unitTargetSearchCs.Search_For_Nearest_Target();
                break;

            case eUnit_targetSelectType.low_Health_Target:
                unitTargetSearchCs.Search_For_Lowhealth_Target();
                break;
        }
        
    }
    #endregion

    #region # MoveUnit(Vector3 arrivePos) : 유닛 상태가 Move일 때 호출되는 함수 - 이동 시 호출
    public void MoveUnit(Vector3 arrivePos) // 유닛 이동
    {
        nav.SetDestination(arrivePos);
        float distance = Vector3.Distance(transform.position, arrivePos);
        if (distance <= 1.2f)
        {
            anim.SetBool("isMove", false);

            unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
        }
    }
    #endregion

    #region # TrackingTarget(eUnit_Action_States next_ActionState = eUnit_Action_States.Default) : 유닛이 타겟을 추적하는 상태일 때 호출되는 함수
    public void TrackingTarget(eUnit_Action_States next_ActionState = eUnit_Action_States.Default)    // 타겟 추적 상태 시 호출하는 함수
    {
        nav.isStopped = false;

        float distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);
        //print("거리 : " + distance);

        //print(distance);
        unitTargetSearchCs.Look_At_The_Target(next_ActionState);

        if (distance >= unitInfoCs._unitData.attackRange && distance <= unitInfoCs._unitData.sightRange)   // 유닛 시야범위보다 작다면
        {
            print(240);
            anim.SetBool("isMove", true);
            nav.SetDestination(unitTargetSearchCs._targetUnit.position);
        }

        // 공격 범위에 적이 들어왔을 때
        else if (distance <= unitInfoCs._unitData.attackRange)
        {
            print("공격 타입으로 변환");
            nav.SetDestination(transform.position);
            anim.SetBool("isMove", false);
            unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Attack;
        }

        // 시야밖으로 적이 사라졌을 때
        else if (distance > unitInfoCs._unitData.sightRange)
        {
            nav.SetDestination(transform.position);
            anim.SetBool("isMove", false);

            unitInfoCs._isSearch = false;
            unitTargetSearchCs._targetUnit = null;
            unitTargetSearchCs._target_Body = null;
            nav.isStopped = false;
            unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
        }
    }

    #endregion

    #region # ReadyForAttack(eUnit_Action_States unit_Atk_State = eUnit_Action_States.Default) : 유닛 상태가 Attack 상태일 때 호출, 현재 공격 지정 상태를 매개변수로 함
    public void ReadyForAttack(eUnit_Action_States unit_Atk_State = eUnit_Action_States.Default)  // Attack 상태일 때 호출 ,기본으로 추적상태를 매개변수로 함
    {
        float distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);

        // 거리가 공격범위보다 크면 유닛 추적
        if (distance > unitInfoCs._unitData.attackRange)
        {
            unitInfoCs._enum_Unit_Action_State = unitInfoCs._enum_Unit_Attack_State;
        }

        //공격모션을 실행하고
        unitTargetSearchCs.Look_At_The_Target(unit_Atk_State);
    }
    #endregion

    #region # Boundary() : 플레이어 유닛이 홀드상태일 때 호출되는 함수
    public void Boundary()  // 유닛 홀드 상태일 때
    {
        print("호출");
        float distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);
        unitTargetSearchCs.Look_At_The_Target(next_Action_State : eUnit_Action_States.unit_Boundary);
        if (distance <= unitInfoCs._unitData.attackRange)
        {
            print("공격 타입으로 변환");
            unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Attack;
        }
        // 시야 범위 밖으로 적이 사라졌을 때
        else if (distance > unitInfoCs._unitData.sightRange)
        {
            unitInfoCs._isSearch = false;
            unitTargetSearchCs._targetUnit = null;
            unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
        }
    }
    #endregion

}
