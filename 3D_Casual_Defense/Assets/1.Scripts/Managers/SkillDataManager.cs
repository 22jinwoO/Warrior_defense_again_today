using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitDataManager;

public class SkillDataManager : MonoBehaviour
{
    // 일반 스킬 배열
    [Header("일반 스킬 배열")]
    public GeneralSkill[] genral_Skills;
    //
    // 특수 스킬 배열
    [Header("특수 스킬 배열")]
    public SpecialSkill[] special_Skills;

    [Header("셋팅한 스킬 할당하는 딕셔너리")]
    public SerializableDictionary<string, Abs_Skill> Set_skill_Dictionary;

    [Header("스킬 딕셔너리")]
    public SerializableDictionary<string, SkillData> _skill_Dictionary;

    [Header("스킬 링크효과 딕셔너리")]
    public SerializableDictionary <string, LinkSkillData> _skill_Link_Dictionary;

    [Header("스킬 공격 속성 딕셔너리")]
    public SerializableDictionary<string, eUnit_Attack_Property_States> _skill_AtkType_Dictionary;

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

    [Header("스킬 Json 데이터 에셋")]
    public TextAsset skill_Json_Data;  // Json 데이터 에셋 이 파일은 유니티 상에서 드래그해서 넣어줍니다.

    [Header("스킬 제이슨 파일 변환 데이터형")]
    public All_Skill_Data all_Skill_Datas;  // 변환 데이터형

    [Header("링크 스킬 제이슨 파일 변환 데이터형")]
    public All_LinkSkill_Data all_LinkSkill_Datas;  // 변환 데이터형



    private void Awake()
    {
        all_Skill_Datas = JsonUtility.FromJson<All_Skill_Data>(skill_Json_Data.text);   // Json파일의 텍스트들을 datas 값에 넣어주고

        all_LinkSkill_Datas = JsonUtility.FromJson<All_LinkSkill_Data>(skill_Json_Data.text);   // Json파일의 텍스트들을 datas 값에 넣어주고
        
        // 스킬 공격 속성 딕셔너리에 키, 값 할당
        Add_Skill_AtkType_Dictionary();

        // 링크스킬에 링크스킬 데이터 할당
        Add_skill_Link_Dictionary(5);

        // 스킬 데이터 할당
        Add_skill_Dictionary(num:genral_Skills.Length);




        //링크 스킬 데이터 할당
        for (int i = 0; i < genral_Skills.Length; i++)
        {           
            Debug.LogWarning(genral_Skills[i]._link_Id);
            Set_Link_Skill(genral_Skills[i]);
        }

        for (int i = 0; i < special_Skills.Length; i++)
        {
            Debug.LogWarning(special_Skills[i]._link_Id);
            Set_Link_Skill(special_Skills[i]);
        }

        // 세팅 완료된 스킬 할당
        Add_Set_Skill_Dictionary();

    }

    // 초기 데이터 설정 할당 완료된 스킬 딕셔너리에 스킬ID를 키, 스킬ID에 해당하는 스킬을 값 할당해주는 함수
    #region # Add_Skill_Dictionary() : 스킬 딕셔너리에 키, 값 추가해주는 함수
    private void Add_Set_Skill_Dictionary()
    {
        //_skill_Dictionary.Add(key : "베기", value : skillDataManagerCs.genral_Skills[0]);
        Set_skill_Dictionary.Add(key: "1_101", value: genral_Skills[0]);
        Set_skill_Dictionary.Add(key: "1_201", value: genral_Skills[1]);
        Set_skill_Dictionary.Add(key: "1_302", value: genral_Skills[2]);
        Set_skill_Dictionary.Add(key: "1_202", value: genral_Skills[3]);

        // 스폐셜 스킬
        Set_skill_Dictionary.Add(key: "2_202", value: special_Skills[0]);
        Set_skill_Dictionary.Add(key: "2_303", value: special_Skills[1]);
        Set_skill_Dictionary.Add(key: "2_304", value: special_Skills[2]);
        Set_skill_Dictionary.Add(key: "2_901", value: special_Skills[3]);
    }
    #endregion

    // 스킬 공격 속성 딕셔너리에 키, 값 할당하는 함수
    private void Add_Skill_AtkType_Dictionary()
    {
        _skill_AtkType_Dictionary.Add(key: "pierce", value: eUnit_Attack_Property_States.piercing_Attack);
        _skill_AtkType_Dictionary.Add(key: "crush", value: eUnit_Attack_Property_States.crushing_attack);
        _skill_AtkType_Dictionary.Add(key: "slash", value: eUnit_Attack_Property_States.slash_Attack);
    }

    // # 스킬 지정해주는 함수

    private void Add_skill_Dictionary(int num)
    {
        for (int i = 0; i < num; i++)
        {
            //스킬 딕셔너리에 스킬 아이디를 키, 스킬의 제이슨 파일 데이터를 값으로 갖는 딕셔너리에 값을 추가
            _skill_Dictionary.Add(key: all_Skill_Datas.SkillDatas[i].skill_id, value: all_Skill_Datas.SkillDatas[i]);

            // 일반스킬에 링크스킬 연결
            genral_Skills[i].Set_Init_Skill(all_Skill_Datas.SkillDatas[i]);

            // 일반 스킬의 공격 타입 할당
            genral_Skills[i]._skill_AtkType = _skill_AtkType_Dictionary[genral_Skills[i]._damgeType];

            Instantiate(genral_Skills[i], transform);

            
        }

        // 링크 스킬 스턴
        for (int i = genral_Skills.Length; i < genral_Skills.Length+special_Skills.Length; i++)
        {
            //스킬 딕셔너리에 스킬 아이디를 키, 스킬의 제이슨 파일 데이터를 값으로 갖는 딕셔너리에 값을 추가
            _skill_Dictionary.Add(key: all_Skill_Datas.SkillDatas[i].skill_id, value: all_Skill_Datas.SkillDatas[i]);

            // 특수 스킬에 링크스킬 연결
            special_Skills[i - genral_Skills.Length].Set_Init_Skill(all_Skill_Datas.SkillDatas[i]);

            // 특수 스킬의 공격 타입 할당
            special_Skills[i- genral_Skills.Length]._skill_AtkType = _skill_AtkType_Dictionary[special_Skills[i - genral_Skills.Length]._damgeType];

            Instantiate(special_Skills[i - genral_Skills.Length], transform);


        }

    }


    public void Set_Link_Skill(Abs_Skill skill)
    {
        switch (skill._link_Id)
        {
            case "stun01":
                skill._link_Skill = new StunStatus();
                Debug.LogWarning("스턴할당");
                // 링크 스킬의 데이터 = 링크 스킬 데이터 할당
                break;

            case "bleed01":
                skill._link_Skill = new BleedingStatus();
                break;

            case "poison01":
                skill._link_Skill = new PoisonStatus();
                Debug.LogWarning("독 할당");

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


        // 링크스킬이 공백(데이터 없음)이 아닐 때만 링크스킬에 해당하는 데이터 할당
        if (skill._link_Id!=null&&skill._link_Id!="")
        {
            skill._link_Skill.link_id = _skill_Link_Dictionary[key: skill._link_Id].link_id;
            skill._link_Skill.link_name = _skill_Link_Dictionary[key: skill._link_Id].link_name;
            skill._link_Skill.link_script = _skill_Link_Dictionary[key: skill._link_Id].link_script;
            skill._link_Skill.linkValue_ps = _skill_Link_Dictionary[key: skill._link_Id].linkValue_ps;
            skill._link_Skill.duration_s = _skill_Link_Dictionary[key: skill._link_Id].duration_s;
            skill._link_Skill.moveSpeedreduce = _skill_Link_Dictionary[key: skill._link_Id].moveSpeedreduce;

            print(skill._link_Skill.link_id);
            print(skill._link_Skill.link_name);
            print(skill._link_Skill.link_script);
            print(skill._link_Skill.linkValue_ps);
            print(skill._link_Skill.duration_s);
            print(skill._link_Skill.moveSpeedreduce);
        }


        //print(skill._link_Skill.link_id);
        //print(skill._link_Skill.link_name);
        //print(skill._link_Skill.link_script);
        //print(skill._link_Skill.linkValue_ps);
        //print(skill._link_Skill.duration_s);
        //print(skill._link_Skill.moveSpeedreduce);

    }




    // # 스킬의 링크 스킬 지정해주는 함수

    private void Add_skill_Link_Dictionary(int num)
    {
        for (int i = 0; i < num; i++)
        {
            _skill_Link_Dictionary.Add(key: all_LinkSkill_Datas.SkillLinkDatas[i].link_id, value: all_LinkSkill_Datas.SkillLinkDatas[i]);

        }
        // 링크 스킬 스턴

        //// 링크 스킬 스턴
        //_skill_Link_Dictionary.Add(key: "stun01", value: stunStatus);

        //// 링크 스킬 출혈
        //_skill_Link_Dictionary.Add(key: "bleed01", value: bleedingStatus);

        //// 링크 스킬 중독
        //_skill_Link_Dictionary.Add(key: "poison01", value: poisonStatus);

        //// 링크 스킬 화염피해
        //_skill_Link_Dictionary.Add(key: "burn01", value: burningStatus);

        //// 링크 스킬 이속감소
        //_skill_Link_Dictionary.Add(key: "slow01", value: slowStatus);

    }


    //링크 스킬 데이터
    [System.Serializable]
    public class All_LinkSkill_Data
    {
        public LinkSkillData[] SkillLinkDatas;    // SkillDatas 이름은 시트 이름이랑 같아야 함
    }

    [System.Serializable]
    public class LinkSkillData
    {
        //no	link_id	link_name	link_script	linkValue_ps	duration_s	moveSpeedreduce																			

        public int no;

        public string link_id;

        public string link_name;


        public string link_script;

        public int linkValue_ps;

        public int duration_s;

        public int moveSpeedreduce;
    }


    // 스킬 데이터
    [System.Serializable]
    public class All_Skill_Data
    {
        public SkillData[] SkillDatas;    // SkillDatas 이름은 시트 이름이랑 같아야 함
    }


    [System.Serializable]
    public class SkillData
    {
        // skill_id	skill_name	skill_script	coolTm	targetType	useRange	castingType	traceType	areaShape	areaLength	areaWidth	damageType	baseValue	criticDamage	link_id											

        public string skill_id;

        public string skill_name;

        public string skill_script;

        public int coolTm;
        public string use_Range;
        public string targetType;

        public string castingType;

        public string traceType;
        public string areaShape;
        public int areaLength;
        public int areaWidth;
        public string damageType;
        public int baseValue;
        public int criticDamage;
        public string link_id;

    }


}
