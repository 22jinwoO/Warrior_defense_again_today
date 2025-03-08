using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackEvent : MonoBehaviour
{
    [SerializeField]
    private NewUnitInfo unitInfoCs;    
    
    [SerializeField]
    private IGenSkill skillCS;

    [SerializeField]
    private NavMeshAgent nav;

    private void Awake()
    {
        skillCS = GetComponent<IGenSkill>();
        nav = GetComponent<NavMeshAgent>();
    }

    #region # AnimEvent_Normal_Atk() : 일반 스킬 애니메이션 동작 시 호출되는 애니메이션 이벤트 함수
    public void AnimEvent_Normal_Atk()  // 일반 스킬 사용 시 호출되는 애니메이션
    {

        if (nav.enabled)    // 네비메쉬 에이전트가 활성화 되어 있다면
        {
            nav.isStopped = true;
        }

        //unitInfoCs.gen_skill.unitTargetSearchCs = unitTargetSearchCs;

        //if (unitInfoCs.atkSoundPlayer != null)
        //{
        //    // 사운드 오디오 소스 할당
        //    unitInfoCs.atkSoundPlayer.pitch = Random.Range(0.7f, 1.4f);
        //    //unitInfoCs.atkSoundPlayer.volume = Random.Range(0.2f, 0.4f);

        //    // 거리에 따른 볼륨 크기 조절
        //    unitInfoCs.atkSoundPlayer.volume = SoundManager.Instance.VolumeCheck(transform);

        //    // 오디오 클립 변수 생성하고 어웨이크문에서 오디오 클립 할당하도록 수정하기
        //    unitInfoCs.atkSoundPlayer.PlayOneShot(unitInfoCs.use_Sfxs[0]);

        //}

        // 일반 스킬 사용
        skillCS.Attack_Skill();
    }
    #endregion

    #region # AnimEvent_SKill_Atk() : 특수 스킬 애니메이션 동작 시 호출되는 애니메이션 이벤트 함수
    public void AnimEvent_SKill_Atk()  // 일반 스킬 사용 시 호출되는 애니메이션
    {
        if (nav.enabled)    // 네비메쉬 에이전트가 활성화 되어 있다면
        {
            nav.isStopped = true;
        }

        //if (unitInfoCs.atkSoundPlayer != null)
        //{

        //    // 사운드 오디오 소스 할당
        //    unitInfoCs.atkSoundPlayer.pitch = Random.Range(0.7f, 1.4f);
        //    //unitInfoCs.atkSoundPlayer.volume = Random.Range(0.2f, 0.4f);

        //    // 거리에 따른 볼륨 크기 조절
        //    unitInfoCs.atkSoundPlayer.volume = SoundManager.Instance.VolumeCheck(transform);

        //    // 오디오 클립 변수 생성하고 어웨이크문에서 오디오 클립 할당하도록 수정하기
        //    unitInfoCs.atkSoundPlayer.PlayOneShot(unitInfoCs.use_Sfxs[0]);

        //}

        // 특수 스킬 사용
        if (unitInfoCs._enum_Unit_Action_Mode.Equals(eUnit_Action_States.unit_FreeMode))
            unitInfoCs.spe_skill_1.GetComponent<IGenSkill>().Attack_Skill();

        else
            unitInfoCs.spe_skill_2.GetComponent<IGenSkill>().Attack_Skill();
    }
    #endregion
}
