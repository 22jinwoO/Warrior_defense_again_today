/*
    ===================================================
   ㅣ           유닛의 행동을 구현한 스크립트            ㅣ
    ===================================================
 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

    public UnitTargetSearch unitTargetSearchCs;

    private Stage1_TextManager txtManager;

    private MonsterUnitClass monsterClass;

    private PlayerUnitClass playerClass;
    
    private void Awake()
    {
        unitInfoCs = GetComponent<UnitInfo>();

        nav = GetComponent<NavMeshAgent>();
        //print(_unitData._unit_Attack_CoolTime);
        anim = GetComponent<Animator>();
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();

        txtManager = Stage1_TextManager.Instance;


        if(TryGetComponent(out MonsterUnitClass monster))
        {
            monsterClass = monster;
        }


        if (TryGetComponent(out PlayerUnitClass player))
        {
            playerClass = player;
        }
    }
    public void Update()
    {
        if (unitInfoCs._isDead && unitInfoCs._unitData.hp <= 0)
            Debug.LogWarning("유닛이 안죽는 버그발생");
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(Get_DamagedBody());
        }
    }
    #region # Attack_Unit() : 유닛이 공격할 때 호출되는 함수
    public void Attack_Unit(eUnit_Action_States next_Action_State = eUnit_Action_States.Default)   // 매개변수로 공격 후 다음에 어떤상태로 변환할지 넣어주기
    {
        anim.SetBool("isMove", false);

        anim.ResetTrigger("isAttack");

        //if (nav.enabled)    // 네비메쉬 에이전트가 활성화 되어 있다면
        //{
        //    nav.isStopped = true;
        //}
        // 특수 스킬로 공격
        if (unitInfoCs._can_SpcSkill_Attack && !gameObject.CompareTag("Monster"))
        {
            if (nav.enabled)
            {
                nav.avoidancePriority = 48;
                nav.isStopped = true;  // 이동 가능 상태로 변환
            }
            anim.ResetTrigger("isSkillAtk");
            Vector3 _target_Direction = unitTargetSearchCs._targetUnit.position - unitInfoCs.transform.position;

            Quaternion rot2 = Quaternion.LookRotation(_target_Direction.normalized);

            unitInfoCs.transform.rotation = rot2;

            unitInfoCs.transform.rotation = Quaternion.Euler(0, rot2.eulerAngles.y, 0);


            Debug.Log("스킬공격!!!");
            if (unitTargetSearchCs._targetUnit == null)
            {
                return;
            }
            anim.SetTrigger("isSkillAtk");

            //if (unitInfoCs._enum_Unit_Action_Mode.Equals(eUnit_Action_States.unit_FreeMode))
            //    unitInfoCs.spe_skill_1.Attack_Skill();

            //else
            //    unitInfoCs.spe_skill_2.Attack_Skill();

            //unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
            unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;

            // 몬스터 유닛일 땐 공격 후 딜레이를 주어 다음 상태로 변환
            if (gameObject.CompareTag("Monster"))
            {
                StartCoroutine(Change_MonsterState(next_Action_State));
            }

            // 플레이어 유닛은 바로 다음상 상태로 변환
            //else
            //{
            //    if (nav.enabled&& gameObject.CompareTag("Monster"))
            //    {
            //        nav.avoidancePriority = 50;
            //        nav.isStopped = false;  // 이동 가능 상태로 변환
            //    }
            //    else
            //    {
            //        nav.avoidancePriority = 49;
            //        float asdf = 0f;
            //        while (asdf < 0.5f)
            //        {
            //            asdf += Time.deltaTime;
            //        }
            //        unitInfoCs._enum_Unit_Action_State = next_Action_State;
            //    }


            //}
            nav.avoidancePriority = 49;
            return; // 함수 탈출



            //anim.ResetTrigger("isAttack");

        }

        else if (unitInfoCs._can_genSkill_Attack)
        {
            anim.ResetTrigger("isAttack");

            Debug.Log("기본공격!!!");
            print("88오브젝트이름 " + unitInfoCs.gameObject.name);

            print("88크리티컬 확률 " + unitInfoCs._unitData.criticRate);

            if (unitTargetSearchCs._targetUnit == null)
            {
                return;
            }

            Vector3 _target_Direction = unitTargetSearchCs._targetUnit.position - unitInfoCs.transform.position;

            Quaternion rot2 = Quaternion.LookRotation(_target_Direction.normalized);

            unitInfoCs.transform.rotation = rot2;

            unitInfoCs.transform.rotation = Quaternion.Euler(0, rot2.eulerAngles.y, 0);


            anim.SetTrigger("isAttack");

            unitInfoCs._unitData._unit_Attack_CoolTime = 0f;

            {
                float asdf = 0f;
                while (asdf<0.5f)
                {
                    asdf += Time.deltaTime;
                }
                unitInfoCs._enum_Unit_Action_State = next_Action_State;

            }
            //StartCoroutine(unitInfoCs.GetComponent<PlayerUnitClass>().Change_PlayerState(next_Action_State));
            //unitInfoCs._enum_Unit_Action_State = next_Action_State;

        }

        //if (nav.enabled && gameObject.CompareTag("Monster"))
        //{
        //    nav.avoidancePriority = 50;
        //    nav.isStopped = false;  // 이동 가능 상태로 변환
        //}

        //else if (nav.enabled && gameObject.CompareTag("Player"))    // 네비메쉬 에이전트가 활성화 되어 있다면
        //{
        //    nav.avoidancePriority = 49;

        //    //print(nav.gameObject.name);
        //    nav.isStopped = false;  // 이동 가능 상태로 변환
        //}
        if(nav.enabled)
            nav.isStopped = false;  // 이동 가능 상태로 변환
        Debug.LogWarning(gameObject.name);
        nav.avoidancePriority = 49;
    }
    #endregion


    #region # Change_MonsterState(eUnit_Action_States next_Action_State) : 몬스터가 공격 후 다음 상태로 변환 시 딜레이를 주기 호출되는 함수
    IEnumerator Change_MonsterState(eUnit_Action_States next_Action_State)
    {

        yield return new WaitForSeconds(1.5f);

        nav.isStopped = false;

        unitInfoCs._enum_Unit_Action_State = next_Action_State;
    }
    #endregion

    #region # AnimEvent_Normal_Atk() : 일반 스킬 애니메이션 동작 시 호출되는 애니메이션 이벤트 함수
    public void AnimEvent_Normal_Atk()  // 일반 스킬 사용 시 호출되는 애니메이션
    {

        if (nav.enabled)    // 네비메쉬 에이전트가 활성화 되어 있다면
        {
            nav.isStopped = true;
        }

        unitInfoCs.gen_skill.unitTargetSearchCs = unitTargetSearchCs;

        if (unitInfoCs.atkSoundPlayer!=null)
        {
            // 사운드 오디오 소스 할당
            unitInfoCs.atkSoundPlayer.pitch = Random.Range(0.7f, 1.4f);
            //unitInfoCs.atkSoundPlayer.volume = Random.Range(0.2f, 0.4f);
            
            // 거리에 따른 볼륨 크기 조절
            unitInfoCs.atkSoundPlayer.volume = SoundManager.Instance.VolumeCheck(transform);

            // 오디오 클립 변수 생성하고 어웨이크문에서 오디오 클립 할당하도록 수정하기
            unitInfoCs.atkSoundPlayer.PlayOneShot(unitInfoCs.use_Sfxs[0]);

        }
        // 일반 스킬 사용
        unitInfoCs.gen_skill.Attack_Skill();
    }
    #endregion

    #region # AnimEvent_SKill_Atk() : 특수 스킬 애니메이션 동작 시 호출되는 애니메이션 이벤트 함수
    public void AnimEvent_SKill_Atk()  // 일반 스킬 사용 시 호출되는 애니메이션
    {
        if (nav.enabled)    // 네비메쉬 에이전트가 활성화 되어 있다면
        {
            nav.isStopped = true;
        }

        if (unitInfoCs.atkSoundPlayer != null)
        {

            // 사운드 오디오 소스 할당
            unitInfoCs.atkSoundPlayer.pitch = Random.Range(0.7f, 1.4f);
            //unitInfoCs.atkSoundPlayer.volume = Random.Range(0.2f, 0.4f);

            // 거리에 따른 볼륨 크기 조절
            unitInfoCs.atkSoundPlayer.volume = SoundManager.Instance.VolumeCheck(transform);

            // 오디오 클립 변수 생성하고 어웨이크문에서 오디오 클립 할당하도록 수정하기
            unitInfoCs.atkSoundPlayer.PlayOneShot(unitInfoCs.use_Sfxs[0]);

        }
        // 특수 스킬 사용
        if (unitInfoCs._enum_Unit_Action_Mode.Equals(eUnit_Action_States.unit_FreeMode))
            unitInfoCs.spe_skill_1.Attack_Skill();

        else
            unitInfoCs.spe_skill_2.Attack_Skill();
    }
    #endregion



    #region # float CheckCritical(Abs_Skill skill, float atkDmg) : 크리티컬 확률 체크하는 함수
    private float CheckCritical(UnitInfo attacker, Abs_Skill skill, float atkDmg)
    {
        // int 범위를 구할때는 최대값이 제외된다. (크리티컬 확률에 해당하는지 체크)
        int randNum = Random.Range(1, 101);

        bool isCritical = false;

        print("크리티컬 랜덤 숫자 " + randNum);

        // 공격자의 공격이 크리티컬 확률에 해당한다면
        if (randNum <= attacker._unitData.criticRate)
        {
            // 링크 스킬이 있다면 링크 스킬 적용
            if (skill._link_Skill != null)
            {
                Debug.LogWarning("링크스킬적용!");

                skill._link_Skill.isStatusApply = true;

                UnitInfo targetUnitInfo = unitTargetSearchCs.GetComponent<UnitInfo>();

                string link_Id = skill._link_Id;
                int link_value = skill._link_Skill.linkValue_ps;
                print(link_value);
                int duration_s = skill._link_Skill.duration_s;
                print(duration_s);

                StartCoroutine(skill._link_Skill.Apply_Status_Effect(targetUnitInfo, link_Id, link_value, duration_s));

                isCritical = true;
                // 링크스킬 적용 후 기본 공격 값 반환
            }
            //링크 스킬이 없을 경우의 크리티컬이 적용됐다면
            else
            {
                // 공격 반환 값 = 기본스킬 데미지 * 크리티컬 배율
                atkDmg = skill._base_Value * skill._critical_Dmg;
            }

        }
        // 피격 효과 보여주기 + 사운드 출력
        StartCoroutine(unitInfoCs.Damaged_Vfx_On(skill, isCritical));

        return atkDmg;

    }
    #endregion

    #region # BeAttacked_By_OtherUnit(Transform other,float attack_Dmg) : 다른 유닛으로부터의 공격으로 피해를 입을 때 호출되는 함수
    // 기본공격 일 때와 스킬 공격 일 때 를 나눠야 함...
    public void BeAttacked_By_OtherUnit(Abs_Skill skill, eUnit_Attack_Property_States myAtkType, ref UnitInfo attacker, Transform other)
    {

        // 피해 입을 때 몸에 피격상태 나타내주는 함수 실행
        StartCoroutine(Get_DamagedBody());

        // 피격자의 유닛 데이터 가져오기
        new_Unit_Data damagedUnitData = other.GetComponent<NewUnitInfo>()._unitData;

        float attack_Dmg = skill._base_Value;

        // 데미지 계산하는 함수 실행
        unitInfoCs._unitData.hp -= unitInfoCs._this_Unit_ArmorCalculateCs.CalculateDamaged(attackType: myAtkType, ArmorType: damagedUnitData, attack_Dmg: CheckCritical(attacker, skill, attack_Dmg));

        if(!unitInfoCs._isDead)
            DeadCheck();
    }
    #endregion


    #region # DeadCheck() : 피격 받은 유닛의 사망 시 호출되는 함수
    public void DeadCheck()
    {
        // 피격 받은 우닛의 Hp가 0 이하가 됐을 때
        if (unitInfoCs._unitData.hp <= 0f)
        {
            // 유닛 사망상태로 전환
            unitInfoCs._isDead = true;

            // 유닛 행동 불가능한 상태로 전환
            unitInfoCs.canAct = false;

            // 유닛의 네비메쉬 속도 값 조정
            unitInfoCs._nav.speed = 0f;
            unitInfoCs._nav.acceleration = 0f;

            unitInfoCs._nav.velocity = Vector3.zero;

            // 유닛의 네비메쉬가 활성화 되어 있으면 isStop으로 멈추기
            if (unitInfoCs._nav.isOnNavMesh)
            {
                unitInfoCs._nav.isStopped = true;
            }

            // 유닛의 네비메쉬 컴포넌트 비활성화
            unitInfoCs._nav.enabled = false;

            // 유닛의 클래스가 플레이어 유닛 클래스라면
            if (monsterClass==null)
                // 네비메쉬 옵스태클 컴포넌트 비활성화
                playerClass.navObs.enabled = false;

            anim.ResetTrigger("isDie");


            if (unitInfoCs.gameObject.tag.Equals("Player"))
            {
                //if(playerClass.holdObPref!=null)
                //    playerClass.holdObPref.transform.SetParent(unitInfoCs.transform);
                playerClass.holdObj.color = new Color(0f, 0f, 0f, 0f);
                playerClass.arriveFlag.transform.SetParent(transform);
                playerClass.arriveFlag.gameObject.SetActive(false);
                unitInfoCs._enum_Unit_Action_Mode = unitInfoCs._enum_pUnit_Action_BaseMode;
                unitInfoCs._enum_Unit_Action_State = unitInfoCs._enum_pUnit_Action_BaseState;
            }

            else
            {
                unitInfoCs._enum_Unit_Action_Mode =   unitInfoCs._enum_mUnit_Action_BaseMode;
                unitInfoCs._enum_Unit_Action_State = unitInfoCs._enum_mUnit_Action_BaseState;

            }

            // 죽음 애니메이션 실행
            anim.SetTrigger("isDie");

            // 죽었을 때 실행되야 하는 함수 호출
            StartCoroutine(DieUnit());

            // 타겟 탐지 없음으로 전환
            unitInfoCs._isSearch = false;

            unitTargetSearchCs._targetUnit = null;
            unitTargetSearchCs._target_Body = null;


        }
    }
    #endregion

    IEnumerator Get_DamagedBody()
    {
        // 바디, 바디이외의 머태리얼 피격 머태리얼로 변환
        for (int i = 0; i < unitInfoCs.someMeshReners.Length; i++)
        {
            unitInfoCs.someMeshReners[i].material = unitInfoCs._damaged_Mtr;
            //0.157f
        }
        unitInfoCs.bodyMeshRener.material = unitInfoCs._damaged_Mtr;

        yield return new WaitForSeconds(0.05f);

        // 다시 원래 머태리얼로 할당
        for (int i = 0; i < unitInfoCs.someMeshReners.Length; i++)
        {
            unitInfoCs.someMeshReners[i].material = unitInfoCs.someMtr[i];
        }
        unitInfoCs.bodyMeshRener.material = unitInfoCs.bodyMtr;
    }

    #region # DieUnit() : 유닛 사망 시 호출되는 함수
    IEnumerator DieUnit()
    {
        unitInfoCs.atkSoundPlayer.volume = SoundManager.Instance.VolumeCheck(transform);

        unitInfoCs.atkSoundPlayer.PlayOneShot(unitInfoCs.use_Sfxs[1]);

        // 성 무너졌을 때 기본 상태로 변환되는 이벤트 함수 연결 해제
        Castle.Instance.OnCastleDown -= unitInfoCs.StopUnitAct;

        unitInfoCs.sprCol.enabled = false;

        float colorValue_a =1f;

        yield return new WaitForSeconds(0.1f);

        // 머태리얼 투명화
        for (int j = 0; j < unitInfoCs.someMeshReners.Length; j++)
        {
            unitInfoCs.someMeshReners[j].material = unitInfoCs.cloaking_someMtr[j];
            yield return null;
        }

        unitInfoCs.bodyMeshRener.material = unitInfoCs.cloaking_bodyMtr;

        yield return new WaitForSeconds(1f);

        Color cloaking_Mtr_Color = unitInfoCs.bodyMeshRener.material.color;

        // 머태리얼 투명도 조절
        while (colorValue_a>0.0f)
        {
            colorValue_a -= 0.01f;

            cloaking_Mtr_Color.a = colorValue_a;

            for (int j = 0; j < unitInfoCs.someMeshReners.Length; j++)
            {
                unitInfoCs.someMeshReners[j].material.color=cloaking_Mtr_Color;
                //0.157f
            }
            unitInfoCs.bodyMeshRener.material.color = cloaking_Mtr_Color;

            yield return new WaitForSeconds(0.02f);

        }

        // 사망한 유닛이 플레이어 유닛 일 때 만
        if (unitInfoCs.gameObject.tag.Equals("Player"))
        {
            // 유닛 사망 시 팩토리의 풀로 Push
            playerClass.unitFactory.units.Push(playerClass);

        }

        else if (unitInfoCs.gameObject.tag.Equals("Monster"))
        {
            // 몬스터 사망 시 팩토리의 풀로 Push
            monsterClass.monsterFactory.monsters.Push(monsterClass);

            MonsterSpawnManager.Instance.currentWave.deathMonsterCnt++;

            // 몬스터 처치 수 텍스트 변환
            txtManager.CountDeadMonster();

        }

        // 사망한 유닛 비활성화
        gameObject.SetActive(false);


        // 나중에 재생성 될 떄 원래 머태리얼로 할당받도록 하기
        yield return new WaitForSeconds(1f);

        // 일반 머태리얼로 할당
        unitInfoCs.bodyMeshRener.material = unitInfoCs.bodyMtr;

        for (int j = 0; j < unitInfoCs.someMeshReners.Length; j++)
        {
            unitInfoCs.someMeshReners[j].material = unitInfoCs.someMtr[j];
            //0.157f
        }

        cloaking_Mtr_Color = unitInfoCs.bodyMeshRener.material.color;

        cloaking_Mtr_Color.a = 1f;

        unitInfoCs._unit_CloakingMtr.color= cloaking_Mtr_Color;

        for (int j = 0; j < unitInfoCs.someMeshReners.Length; j++)
        {
            unitInfoCs.cloaking_someMtr[j].color = cloaking_Mtr_Color;
            yield return null;
            //0.157f
        }

    }
    #endregion

    #region # SearchTarget(매개변수 : 유닛 탐지 타입) : 유닛이 Idle 상태일 때 타겟 탐지 시 호출되는 함수
    public void SearchTarget(eUnit_targetSelectType target_Search_Type = eUnit_targetSelectType.Default)  // 유닛 탐지
    {
        switch (target_Search_Type)
        {
            case eUnit_targetSelectType.fixed_Target:
                unitTargetSearchCs.Search_For_Fixed_Target();
                break;

            case eUnit_targetSelectType.nearest_Target:
                unitTargetSearchCs.Search_For_longest_Target();
                break;

            case eUnit_targetSelectType.low_Health_Target:
                unitTargetSearchCs.Search_For_Lowhealth_Target();
                break;
        }
        
    }
    #endregion

    // 플레이어 유닛 이동
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

        if (distance > 0.5f+unitInfoCs._unitData.attackRange && distance <= unitInfoCs._unitData.sightRange)   // 유닛 시야범위보다 작다면
        {
            if (nav.velocity.Equals(0))
                print("오크이동안됨");
            anim.SetBool("isMove", true);
            if(gameObject.CompareTag("Monster"))
            {
                print("몬스터 이동");
                nav.SetDestination(unitTargetSearchCs._targetUnit.position-new Vector3(0f,0f,unitInfoCs._unitData.attackRange-1f));
            }
            else if(gameObject.CompareTag("Player"))
            {
                nav.SetDestination(unitTargetSearchCs._targetUnit.position);
                print(unitInfoCs._nav.isStopped);
                print(unitTargetSearchCs._targetUnit.position);

            }
        }

        // 공격 범위에 적이 들어왔을 때
        else if (distance <= unitInfoCs._unitData.attackRange+0.5f)
        {
            nav.SetDestination(transform.position);
            anim.SetBool("isMove", false);


            unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Attack;


            //unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Attack;
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
        //float distance = Vector3.Distance(transform.position, unitTargetSearchCs._targetUnit.position);

        //// 거리가 공격범위보다 크면 유닛 추적
        //if (distance > unitInfoCs._unitData.attackRange)
        //{
        //    unitInfoCs._enum_Unit_Action_State = unitInfoCs._enum_Unit_Attack_State;
        //}

        Vector3 _target_Direction = unitTargetSearchCs._targetUnit.position - unitInfoCs.transform.position;

        Quaternion rot = Quaternion.LookRotation(_target_Direction.normalized);

        rot.y = 0;
        transform.rotation = rot;
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
