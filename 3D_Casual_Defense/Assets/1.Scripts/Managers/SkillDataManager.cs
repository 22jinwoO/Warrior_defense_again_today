using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataManager : MonoBehaviour
{
    // 일반 스킬 배열
    [Header("일반 스킬 배열")]
    public GeneralSkill[] genral_Skills;
    //
    // 특수 스킬 배열
    [Header("특수 스킬 배열")]
    public SpecialSkill[] special_Skills;

    [Header("스킬 링크효과 딕셔너리")]
    public SerializableDictionary <string, StatusEffect> _skill_Link_Dictionary;

    [SerializeField]
    StunStatus stunStatus = new StunStatus();

    [SerializeField]
    BleedingStatus bleedingStatus = new BleedingStatus();

    [SerializeField]
    PoisonStatus poisonStatus = new PoisonStatus();

    [SerializeField]
    BurningStatus burningStatus = new BurningStatus();

    [SerializeField]
    SlowStatus slowStatus = new SlowStatus();

    private void Awake()
    {
        for (int i = 0; i < genral_Skills.Length; i++)
        {
            Set_Link_Skill(genral_Skills[i]);
        }

        genral_Skills[1]._link_Skill.duration_s = 5;
        genral_Skills[1]._link_Skill.linkValue_ps = 2;
        print(genral_Skills[0]._link_Skill);
        print(genral_Skills[1]._link_Skill);
        print(genral_Skills[1]._link_Skill.duration_s);
        print(genral_Skills[1]._link_Skill.linkValue_ps);
        Add_skill_Link_Dictionary();
        //print(new _skill_Link_Dictionary["stun01"]);
    }

    // # 스킬의 링크 스킬 지정해주는 함수

    public void Set_Link_Skill(Abs_Skill skill)
    {
        switch (skill._link_Id)
        {
            case "stun01":
                skill._link_Skill = new StunStatus();
                break;

            case "bleed01":
                skill._link_Skill = new BleedingStatus();
                break;

            case "poison01":
                skill._link_Skill = new PoisonStatus();
                break;

            case "burn01":
                skill._link_Skill = new BurningStatus();
                break;

            case "slow01":
                skill._link_Skill = new SlowStatus();
                break;

            default:
                break;
        }
    }

//# 스킬의 링크 스킬 지정해주는 함수
    private void Add_skill_Link_Dictionary()
    {
        // 링크 스킬 스턴
        _skill_Link_Dictionary.Add(key: "stun01", value: stunStatus);

        // 링크 스킬 출혈
        _skill_Link_Dictionary.Add(key: "bleed01", value: bleedingStatus);

        // 링크 스킬 중독
        _skill_Link_Dictionary.Add(key: "poison01", value: poisonStatus);

        // 링크 스킬 화염피해
        _skill_Link_Dictionary.Add(key: "burn01", value: burningStatus);

        // 링크 스킬 이속감소
        _skill_Link_Dictionary.Add(key: "slow01", value: slowStatus);

    }
}
