/*
    ===================================================
   ㅣ           유닛의 행동을 구현한 스크립트            ㅣ
    ===================================================
 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    
    private void Awake()
    {
        unitInfoCs = GetComponent<UnitInfo>();

        nav = GetComponent<NavMeshAgent>();
        //print(_unitData._unit_Attack_CoolTime);
        anim = GetComponent<Animator>();
        unitTargetSearchCs = GetComponent<UnitTargetSearch>();

        txtManager = Stage1_TextManager.Instance;
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
        anim.SetBool("isMove", false);

        anim.ResetTrigger("isAttack");

        //if (nav.enabled)    // 네비메쉬 에이전트가 활성화 되어 있다면
        //{
        //    nav.isStopped = true;
        //}
        // 기본 스킬로 공격
        if (unitInfoCs._can_genSkill_Attack)
        {
            if (nav.enabled)
            {
                nav.isStopped = true;  // 이동 가능 상태로 변환
            }

            Vector3 _target_Direction = unitTargetSearchCs._targetUnit.position - unitInfoCs.transform.position;

            Quaternion rot2 = Quaternion.LookRotation(_target_Direction.normalized);

            unitInfoCs.transform.rotation = rot2;

            unitInfoCs.transform.rotation = Quaternion.Euler(0, rot2.eulerAngles.y, 0);



            if (unitInfoCs._can_SpcSkill_Attack&&gameObject.name=="Knight(Clone)")    // 스킬은 근거리 원거리 나눌 필요 x, UseSkill 함수 호출 플레이어 유닛이라면 
            {
                Debug.Log("스킬공격!!!");
                if (unitTargetSearchCs._targetUnit == null)
                {
                    return;
                }
                anim.SetTrigger("isAttack");
                unitInfoCs.spe_skill_2.Attack_Skill();

                unitInfoCs._unitData._unit_Current_Skill_CoolTime = 0f;

                // 몬스터 유닛일 땐 공격 후 딜레이를 주어 다음 상태로 변환
                if (gameObject.CompareTag("Monster")&&unitInfoCs._enum_Unit_Action_Mode.Equals(eUnit_Action_States.monster_NormalPhase))
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
                
                unitInfoCs._unitData._unit_Attack_CoolTime = 0f;


                // 몬스터 유닛일 땐 공격 후 딜레이를 주어 다음 상태로 변환
                if (gameObject.CompareTag("Monster") && unitInfoCs._enum_Unit_Action_Mode.Equals(eUnit_Action_States.monster_NormalPhase))
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
        //print("애니메이션 호출 함수");

        // 여기서부터 10에서 0으로 바뀜,,
        //print("165오브젝트이름 " + unitInfoCs.gameObject.name);
        //print("165크리티컬 확률 " + unitInfoCs._unitData.criticRate);

        //unitInfoCs.gen_skill.unitInfoCs = unitInfoCs;   // 나중에 유닛 awake 문에서 한번만 실행하도록 변경하기

        if (nav.enabled)    // 네비메쉬 에이전트가 활성화 되어 있다면
        {
            nav.isStopped = true;
        }

        unitInfoCs.gen_skill.unitTargetSearchCs = unitTargetSearchCs;

        if (unitInfoCs.atkSoundPlayer!=null)
        {
            //unitInfoCs.atkSoundPlayer.PlayOneShot(unitInfoCs.atkSoundPlayer.GetComponent<AudioClip>());

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
        Vector3 direction = attacker.transform.position - transform.position;
        //StartCoroutine(unitInfoCs.Damaged_Vfx_On(skill));


        // 피격자의 유닛 데이터 가져오기
        unit_Data damagedUnitData = other.GetComponent<UnitInfo>()._unitData;

        float attack_Dmg = skill._base_Value;

        // 데미지 계산하는 함수 실행
        unitInfoCs._unitData.hp -= unitInfoCs._this_Unit_ArmorCalculateCs.CalculateDamaged(attackType: myAtkType, ArmorType: damagedUnitData, attack_Dmg: CheckCritical(attacker, skill, attack_Dmg));

        DeadCheck();
    }
    #endregion


    public void DeadCheck()
    {
        // 피격 받은 우닛의 Hp가 0 이하가 됐을 때
        if (unitInfoCs.canAct && unitInfoCs._unitData.hp <= 0f)
        {
            unitInfoCs.canAct = false;
            unitInfoCs._nav.speed = 0f;
            unitInfoCs._nav.acceleration = 0f;

            unitInfoCs._nav.velocity = Vector3.zero;

            //unitInfoCs._nav.enabled = false;
            if (unitInfoCs._nav.isOnNavMesh)
            {
                unitInfoCs._nav.isStopped = true;
            }
            anim.ResetTrigger("isDie");

            //unitInfoCs._isDead = true;
            //attacker._isTargetDead = true;
            if (unitInfoCs.gameObject.tag.Equals("Player"))
            {
                unitInfoCs._enum_Unit_Action_Mode = unitInfoCs._enum_pUnit_Action_BaseMode;
                unitInfoCs._enum_Unit_Action_State = unitInfoCs._enum_pUnit_Action_BaseState;
            }

            else
            {
                unitInfoCs._enum_Unit_Action_Mode =   unitInfoCs._enum_mUnit_Action_BaseMode;
                unitInfoCs._enum_Unit_Action_State = unitInfoCs._enum_mUnit_Action_BaseState;

            }
            anim.SetTrigger("isDie");
            StartCoroutine(DieUnit());

            //UnitTargetSearch asdf = attacker.unitTargetSearchCs;
            unitInfoCs._isSearch = false;

            unitTargetSearchCs._targetUnit = null;
            unitTargetSearchCs._target_Body = null;


        }
    }
    IEnumerator Get_DamagedBody()
    {
        print("피격당한 유닛 이름 : " + gameObject.name);
        //Material asdf = GetComponentInChildren<Material>();
        //Material asdfd= aaa;


        // 임시 머태리얼 할당
        //for (int i = 0; i < unitInfoCs.someMeshReners.Length; i++)
        //{
        //    unitInfoCs.someMtr[i] = unitInfoCs.someMeshReners[i].material;
        //    //0.157f
        //}

        // 바디, 바디이외의 머태리얼 피격 머태리얼로 변환
        for (int i = 0; i < unitInfoCs.someMeshReners.Length; i++)
        {
            unitInfoCs.someMeshReners[i].material = unitInfoCs._damaged_Mtr;
            //0.157f
        }
        unitInfoCs.bodyMeshRener.material = unitInfoCs._damaged_Mtr;

        // aaa = asdf;
        yield return new WaitForSeconds(0.1f);

        // 다시 원래 머태리얼로 할당
        for (int i = 0; i < unitInfoCs.someMeshReners.Length; i++)
        {
            unitInfoCs.someMeshReners[i].material = unitInfoCs.someMtr[i];
        }
        unitInfoCs.bodyMeshRener.material = unitInfoCs.bodyMtr;

        //for (int i = 0; i < unitInfoCs.someMeshReners.Length; i++)
        //{
        //    unitInfoCs.someMeshReners[i].material.color = new Color(1f, 0.238555f, 0f);
        //    //0.157f
        //}
        //unitInfoCs.bodyMeshRener.material.color = new Color(1f, 0.238555f, 0f);

        //// aaa = asdf;
        //yield return new WaitForSeconds(0.1f);
        //for (int i = 0; i < unitInfoCs.someMeshReners.Length; i++)
        //{
        //    unitInfoCs.someMeshReners[i].material.color = Color.white;
        //    unitInfoCs.someMeshReners[i].material.color = Color.white;

        //}
        //unitInfoCs.bodyMeshRener.material.color = Color.white;

    }


    IEnumerator DieUnit()
    {
        unitInfoCs.atkSoundPlayer.volume = SoundManager.Instance.VolumeCheck(transform);

        unitInfoCs.atkSoundPlayer.PlayOneShot(unitInfoCs.use_Sfxs[1]);

        // 성 무너졌을 때 기본 상태로 변환되는 이벤트 함수 연결 해제
        Castle.Instance.OnCastleDown -= unitInfoCs.OnCastleDown;

        unitInfoCs.sprCol.enabled = false;

        float colorValue_a =1f;

        yield return new WaitForSeconds(0.1f);



        // 클로킹 됐다가 노말 머태리얼로 다시 돌아옴; 이유는?,,, 아마 겟데미지드 함수 때문일듯 (해결)

        for (int j = 0; j < unitInfoCs.someMeshReners.Length; j++)
        {
            unitInfoCs.someMeshReners[j].material = unitInfoCs.cloaking_someMtr[j];
            yield return null;
            //0.157f
        }
        unitInfoCs.bodyMeshRener.material = unitInfoCs.cloaking_bodyMtr;

        yield return new WaitForSeconds(1f);

        Color cloaking_Mtr_Color = unitInfoCs.bodyMeshRener.material.color;

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
        if (unitInfoCs.gameObject.tag.Equals("Player")&& unitInfoCs.GetComponent<PlayerUnitClass>().holdOb!=null)
        {
            unitInfoCs.GetComponent<PlayerUnitClass>().holdOb.SetActive(false);
        }
        else
        {
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

        if (distance > unitInfoCs._unitData.attackRange && distance <= unitInfoCs._unitData.sightRange)   // 유닛 시야범위보다 작다면
        {
            anim.SetBool("isMove", true);
            nav.SetDestination(unitTargetSearchCs._targetUnit.position);
        }

        // 공격 범위에 적이 들어왔을 때
        else if (distance <= unitInfoCs._unitData.attackRange)
        {
            nav.SetDestination(transform.position);
            anim.SetBool("isMove", false);
            if (unitInfoCs._can_genSkill_Attack)
            {
                unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Attack;

            }
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
