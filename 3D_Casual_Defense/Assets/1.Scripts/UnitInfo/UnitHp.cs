using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHp : MonoBehaviour
{
    [Header("유닛 정보 스크립트")]
    [SerializeField]
    private NewUnitInfo unitInfoCs;

    [Header("애니메이터")]
    public Animator _anim;       // 애니메이터

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

    private void Awake()
    {
        print(someMeshReners);
        unitInfoCs = GetComponent<NewUnitInfo>();
        _anim = GetComponent<Animator>();

        // 머태리얼 인스턴스
        _unit_NomralMtr = Instantiate(_unit_NomralMtr);

        _unit_CloakingMtr = Instantiate(_unit_CloakingMtr);

        _damaged_Mtr = Instantiate(_damaged_Mtr);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(Clocking_Body());
        }
    }
    #region # float CheckCritical(Abs_Skill skill, float atkDmg) : 크리티컬 확률 체크하는 함수
    private float CheckCritical(UnitInfo attacker, AbsNewSKill skill, float atkDmg)
    {
        // int 범위를 구할때는 최대값이 제외된다. (크리티컬 확률에 해당하는지 체크)
        int randNum = Random.Range(1, 101);

        bool isCritical = false;

        print("크리티컬 랜덤 숫자 " + randNum);

        //// 공격자의 공격이 크리티컬 확률에 해당한다면
        //if (randNum <= attacker._unitData.criticRate)
        //{
        //    // 링크 스킬이 있다면 링크 스킬 적용
        //    if (skill._link_Skill != null)
        //    {
        //        Debug.LogWarning("링크스킬적용!");

        //        skill._link_Skill.isStatusApply = true;

        //        UnitInfo targetUnitInfo = unitTargetSearchCs.GetComponent<UnitInfo>();

        //        string link_Id = skill._link_Id;
        //        int link_value = skill._link_Skill.linkValue_ps;
        //        print(link_value);
        //        int duration_s = skill._link_Skill.duration_s;
        //        print(duration_s);

        //        StartCoroutine(skill._link_Skill.Apply_Status_Effect(targetUnitInfo, link_Id, link_value, duration_s));

        //        isCritical = true;
        //        // 링크스킬 적용 후 기본 공격 값 반환
        //    }
        //    //링크 스킬이 없을 경우의 크리티컬이 적용됐다면
        //    else
        //    {
        //        // 공격 반환 값 = 기본스킬 데미지 * 크리티컬 배율
        //        atkDmg = skill._base_Value * skill._critical_Dmg;
        //    }

        //}
        //// 피격 효과 보여주기 + 사운드 출력
        //StartCoroutine(unitInfoCs.Damaged_Vfx_On(skill, isCritical));

        return atkDmg;

    }
    #endregion

    #region # BeAttacked_By_OtherUnit(Transform other,float attack_Dmg) : 다른 유닛으로부터의 공격으로 피해를 입을 때 호출되는 함수
    // 기본공격 일 때와 스킬 공격 일 때 를 나눠야 함...
    public void NewBeAttacked_By_OtherUnit(AbsNewSKill skill, eUnit_Attack_Property_States AtkType, ref UnitInfo attacker, Transform other)
    {

        // 피해 입을 때 몸에 피격상태 나타내주는 함수 실행
        StartCoroutine(Get_DamagedBody());

        // 피격자의 유닛 데이터 가져오기
        new_Unit_Data damagedUnitData = other.GetComponent<NewUnitInfo>()._unitData;

        float attack_Dmg = skill._base_Value;
        //Debug.LogWarning($"{skill}");
        //Debug.LogWarning($"{AtkType}");
        //Debug.LogWarning($"{attacker}");
        //Debug.LogWarning($"{other}");
        //Debug.LogWarning($"{skill}");        // 데미지 계산하는 함수 실행
        //Debug.LogWarning($"{damagedUnitData}");        // 데미지 계산하는 함수 실행
        //Debug.LogWarning($"{attack_Dmg}");        // 데미지 계산하는 함수 실행
        //Debug.LogWarning($"{unitInfoCs._this_Unit_ArmorCalculateCs}");        // 데미지 계산하는 함수 실행
        unitInfoCs._unitData.hp -= unitInfoCs._this_Unit_ArmorCalculateCs.CalculateDamaged(attackType: AtkType, ArmorType: damagedUnitData, attack_Dmg: CheckCritical(attacker, skill, attack_Dmg));

        //if (!unitInfoCs._isDead)
        DeadCheck();
    }
    #endregion

    IEnumerator Clocking_Body()
    {
        yield return new WaitForSeconds(0.1f);

        // 머태리얼 투명화
        for (int j = 0; j < someMeshReners.Length; j++)
        {
            print(someMeshReners[j].material);
            print(_unit_CloakingMtr);
            someMeshReners[j].material = _unit_CloakingMtr;
            yield return null;
        }

        bodyMeshRener.material = _unit_CloakingMtr;

        float colorValue_a = 1f;

        yield return new WaitForSeconds(1f);

        Color cloaking_Mtr_Color = bodyMeshRener.material.color;

        // 머태리얼 투명도 조절
        while (colorValue_a > 0.0f)
        {
            colorValue_a -= 0.01f;

            cloaking_Mtr_Color.a = colorValue_a;

            for (int j = 0; j < someMeshReners.Length; j++)
            {
                someMeshReners[j].material.color = cloaking_Mtr_Color;
                //0.157f
            }
            bodyMeshRener.material.color = cloaking_Mtr_Color;

            yield return new WaitForSeconds(0.02f);

        }

    }

    IEnumerator Get_DamagedBody()
    {
        // 바디, 바디이외의 머태리얼 피격 머태리얼로 변환
        for (int i = 0; i < someMeshReners.Length; i++)
        {
            someMeshReners[i].material = _damaged_Mtr;
            //0.157f
        }
        bodyMeshRener.material = _damaged_Mtr;

        yield return new WaitForSeconds(0.05f);

        // 다시 원래 머태리얼로 할당
        for (int i = 0; i < someMeshReners.Length; i++)
        {
            someMeshReners[i].material = _unit_NomralMtr;
        }
        bodyMeshRener.material = _unit_NomralMtr;
    }



    #region # DeadCheck() : 피격 받은 유닛의 사망 시 호출되는 함수
    public void DeadCheck()
    {
        // 피격 받은 우닛의 Hp가 0 이하가 됐을 때
        if (unitInfoCs._unitData.hp <= 0f)
        {
            // 유닛 사망상태로 전환
            //unitInfoCs._isDead = true;

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

            //// 유닛의 클래스가 플레이어 유닛 클래스라면
            //if (monsterClass == null)
            //    // 네비메쉬 옵스태클 컴포넌트 비활성화
            //    playerClass.navObs.enabled = false;

            //anim.ResetTrigger("isDie");


            //if (unitInfoCs.gameObject.tag.Equals("Player"))
            //{
            //    //if(playerClass.holdObPref!=null)
            //    //    playerClass.holdObPref.transform.SetParent(unitInfoCs.transform);
            //    playerClass.holdObj.color = new Color(0f, 0f, 0f, 0f);
            //    playerClass.arriveFlag.transform.SetParent(transform);
            //    playerClass.arriveFlag.gameObject.SetActive(false);
            //    unitInfoCs._enum_Unit_Action_Mode = unitInfoCs._enum_pUnit_Action_BaseMode;
            //    unitInfoCs._enum_Unit_Action_State = unitInfoCs._enum_pUnit_Action_BaseState;
            //}

            //else
            //{
            //    unitInfoCs._enum_Unit_Action_Mode = unitInfoCs._enum_mUnit_Action_BaseMode;
            //    unitInfoCs._enum_Unit_Action_State = unitInfoCs._enum_mUnit_Action_BaseState;

            //}

            // 죽음 애니메이션 실행
            _anim.SetTrigger("isDie");

            // 죽었을 때 실행되야 하는 함수 호출
            //StartCoroutine(DieUnit());

            // 타겟 탐지 없음으로 전환
            unitInfoCs._isSearch = false;

            //unitTargetSearchCs._targetUnit = null;
            //unitTargetSearchCs._target_Body = null;


        }
    }
    #endregion

    //IEnumerator Get_DamagedBody()
    //{
    //    // 바디, 바디이외의 머태리얼 피격 머태리얼로 변환
    //    for (int i = 0; i < unitInfoCs.someMeshReners.Length; i++)
    //    {
    //        unitInfoCs.someMeshReners[i].material = unitInfoCs._damaged_Mtr;
    //        //0.157f
    //    }
    //    unitInfoCs.bodyMeshRener.material = unitInfoCs._damaged_Mtr;

    //    yield return new WaitForSeconds(0.05f);

    //    // 다시 원래 머태리얼로 할당
    //    for (int i = 0; i < unitInfoCs.someMeshReners.Length; i++)
    //    {
    //        unitInfoCs.someMeshReners[i].material = unitInfoCs.someMtr[i];
    //    }
    //    unitInfoCs.bodyMeshRener.material = unitInfoCs.bodyMtr;
    //}

    //#region # DieUnit() : 유닛 사망 시 호출되는 함수
    //IEnumerator DieUnit()
    //{
    //    unitInfoCs.atkSoundPlayer.volume = SoundManager.Instance.VolumeCheck(transform);

    //    unitInfoCs.atkSoundPlayer.PlayOneShot(unitInfoCs.use_Sfxs[1]);

    //    // 성 무너졌을 때 기본 상태로 변환되는 이벤트 함수 연결 해제
    //    Castle.Instance.OnCastleDown -= unitInfoCs.StopUnitAct;

    //    unitInfoCs.sprCol.enabled = false;

    //    float colorValue_a = 1f;

    //    yield return new WaitForSeconds(0.1f);

    //    // 머태리얼 투명화
    //    for (int j = 0; j < unitInfoCs.someMeshReners.Length; j++)
    //    {
    //        unitInfoCs.someMeshReners[j].material = unitInfoCs.cloaking_someMtr[j];
    //        yield return null;
    //    }

    //    unitInfoCs.bodyMeshRener.material = unitInfoCs.cloaking_bodyMtr;

    //    yield return new WaitForSeconds(1f);

    //    Color cloaking_Mtr_Color = unitInfoCs.bodyMeshRener.material.color;

    //    while (colorValue_a > 0.0f)
    //    {
    //        colorValue_a -= 0.01f;
    //        cloaking_Mtr_Color.a = colorValue_a;
    //        for (int j = 0; j < unitInfoCs.someMeshReners.Length; j++)
    //        {
    //            unitInfoCs.someMeshReners[j].material.color = cloaking_Mtr_Color;
    //            //0.157f
    //        }
    //        unitInfoCs.bodyMeshRener.material.color = cloaking_Mtr_Color;

    //        yield return new WaitForSeconds(0.02f);

    //    }

    //    // 사망한 유닛이 플레이어 유닛 일 때 만
    //    if (unitInfoCs.gameObject.tag.Equals("Player"))
    //    {
    //        // 유닛 사망 시 팩토리의 풀로 Push
    //        playerClass.unitFactory.units.Push(playerClass);

    //    }

    //    else if (unitInfoCs.gameObject.tag.Equals("Monster"))
    //    {
    //        // 몬스터 사망 시 팩토리의 풀로 Push
    //        monsterClass.monsterFactory.monsters.Push(monsterClass);

    //        MonsterSpawnManager.Instance.currentWave.deathMonsterCnt++;
    //        r
    //        // 몬스터 처치 수 텍스트 변환
    //        txtManager.CountDeadMonster();

    //    }

    //    // 사망한 유닛 비활성화
    //    gameObject.SetActive(false);


    //    // 나중에 재생성 될 떄 원래 머태리얼로 할당받도록 하기
    //    yield return new WaitForSeconds(1f);

    //    // 일반 머태리얼로 할당
    //    unitInfoCs.bodyMeshRener.material = unitInfoCs.bodyMtr;

    //    for (int j = 0; j < unitInfoCs.someMeshReners.Length; j++)
    //    {
    //        unitInfoCs.someMeshReners[j].material = unitInfoCs.someMtr[j];
    //        //0.157f
    //    }
    //    cloaking_Mtr_Color = unitInfoCs.bodyMeshRener.material.color;
    //    cloaking_Mtr_Color.a = 1f;
    //    unitInfoCs._unit_CloakingMtr.color = cloaking_Mtr_Color;

    //    for (int j = 0; j < unitInfoCs.someMeshReners.Length; j++)
    //    {
    //        unitInfoCs.cloaking_someMtr[j].color = cloaking_Mtr_Color;
    //        yield return null;
    //        //0.157f
    //    }

    //}
    //#endregion
}
