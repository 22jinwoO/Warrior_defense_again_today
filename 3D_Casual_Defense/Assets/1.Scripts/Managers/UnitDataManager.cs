using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDataManager : Singleton<UnitDataManager>
{
    // 제이슨=====================================================

    [Header("Json 데이터 에셋")]
    public TextAsset character_Json_Data;  // Json 데이터 에셋 이 파일은 유니티 상에서 드래그해서 넣어줍니다.

    [Header("제이슨 파일 변환 데이터형")]
    public All_Character_Data All_character_Datas;  // 변환 데이터형

    // =====================================================제이슨


    // 스킬 데이터 매니저 =====================================================
    [Header("스킬 데이터 매니저")]
    [SerializeField]
    private SkillDataManager skillDataManagerCs;

    // ===================================================== 스킬 데이터 매니저


    // 플레이어 유닛 팩토리 =====================================================

    [Header("스톰윈드 기사 팩토리")]
    [SerializeField]
    private KnightFactory knight_Factory;

    [Header("스톰윈드 궁수 팩토리")]
    [SerializeField]
    private ArcherFactory archer_Factory;

    // =====================================================플레이어 유닛 팩토리 


    // 딕셔너리=====================================================

    //[Header("스킬 딕셔너리")]
    //public SerializableDictionary<string,Abs_Skill> _skill_Dictionary;

    [Header("방어 속성 딕셔너리")]
    public SerializableDictionary<string, eUnit_Defense_Property_States> _armor_Dictionary;


    [Header("방어 속성에 따른 스크립트 딕셔너리")]
    public SerializableDictionary<eUnit_Defense_Property_States,ArmorCalculate> _armorCs_Dictionary;

    [Header("타겟 선정 딕셔너리")]
    public SerializableDictionary<string, eUnit_targetSelectType> _targetSelect_Dictionary;

    [Header("플레이어 유닛 데이터 딕셔너리")]
    public SerializableDictionary<string, CharacterData> _unitInfo_Dictionary;

    // ===================================================== 딕셔너리



    private void Awake()
    {
        // 버튼에 스트링으로 딕셔너리 키 값 넣어주기 방법 1.
        // 버튼 배열 가져오고 버튼마다 리스트 인덱스 값 하나씩 넣어주기
        List<string> asdf = new List<string>(_unitInfo_Dictionary.Keys);
        asdf.AddRange(_unitInfo_Dictionary.Keys);

        // 방법 2.
        //List<string> asdf = new List<string>();
        //asdf.AddRange(_unitInfo_Dictionary.Keys);


        All_character_Datas = JsonUtility.FromJson<All_Character_Data>(character_Json_Data.text);   // Json파일의 텍스트들을 datas 값에 넣어주고


        Add_Armor_Dictionary(); // 아머 속성 딕셔너리에 키, 값 할당하는 함수

        Add_TargetSelect_Dictionary();  //타겟 선정 딕셔너리에 키, 값 할당하는 함수

        Add_ArmorScript_Dictionary(); // 아머 속성에 해당하는 스크립트 추가해주는 함수

        Add_UnitData_Dictionary(); // 유닛 데이터에 해당하는 제이슨 파일 값 키, 값 할당하는 함수


        // 링크스킬, 스킬데이터가 장착된 스킬을 유닛 데이터에 할당
        // 플레이어 유닛
        Set_UnitSkillData(unit_ID : "hum_warr01");
        Set_UnitSkillData(unit_ID : "hum_arch01");
        Set_UnitSkillData(unit_ID : "hum_warr02");

        // 몬스터 (오크)
        Set_UnitSkillData(unit_ID : "orc_hunt01");
        Set_UnitSkillData(unit_ID : "orc_warr01");
        Set_UnitSkillData(unit_ID : "orc_sham01");
        Set_UnitSkillData(unit_ID : "orc_boss01");

        //foreach (var item in All_character_Datas.CharacterDatas)
        //{
        //    print(item.unit_class);
        //}

        //Instantiate(skillDataManagerCs.genral_Skills[0]);
    }

    [System.Serializable]
    public class All_Character_Data
    {
        public CharacterData[] CharacterDatas;    // CharacterDatas 이름은 시트 이름이랑 같아야 함
    }


    [System.Serializable]
    public class CharacterData   // CharacterDatas 구글 시트에 해당하는 값을 갖고오기 위한 클래스
    {
        public int no; // 캐릭터 넘버
        public string char_id;   // 캐릭터 id
        public string unit_class; // 유닛 클래스
        public int level;    // 레벨
        public int hp;   // 체력
        public string defenseType;   // 방어 타입
        public int moveSpeed;   // 이동속도
        public int sightRange;   // 시야 범위
        public int attackRange;   // 공격 범위
        public int criticRate;    // 크리티컬 확률
        public string generalSkill;   // 일반스킬
        public string generalSkillName;   // 일반스킬 이름
        public string specialSkill1;   // 특수 스킬 , 자유모드 일 때 사용하는 스킬
        public string specialSkill1Name;   // 특수 스킬 1 이름
        public string specialSkill2;   // 특수 스킬 , 홀드모드 일 때 사용하는 스킬
        public string specialSkill2Name;   // 특수 스킬 2 이름
        public string targetSelectType;   // 유닛 설정 타입

        public Abs_Skill unit_Gen_Skill;
        public Abs_Skill unit_Spc_Skill;
        public Abs_Skill unit_Spc_Skill2;        
        
        public AbsNewSKill new_unit_Gen_Skill;
        public AbsNewSKill new_unit_Spc_Skill;
        public AbsNewSKill new_unit_Spc_Skill2;

        public eUnit_Defense_Property_States unit_Armor_property;
        public ArmorCalculate unit_ArmorCalculateCs;
        public eUnit_targetSelectType unit_targetSelectType;


        //= Instance._skill_Dictionary.TryGetValue();

        //public eUnit_Defense_Property_States unit_Armor_property;
        //public armor

        //_unitData._eUnit_Defense_Property = UnitDataManager.Instance._armor_Dictionary[_unitData.defenseType];

        //// 유닛 방어구 속성 데미지 계산을 위한 스크립트 할당
        //_this_Unit_ArmorCalculateCs = UnitDataManager.Instance._armorCs_Dictionary[_unitData._eUnit_Defense_Property];

        //// 유닛 타겟 설정 타입 할당

        //_unit_targetSelectType = UnitDataManager.Instance._targetSelect_Dictionary[_unitData.targetSelectType];

        //_eUnit_Target_Search_Type = UnitDataManager.Instance._targetSelect_Dictionary[_unitData.targetSelectType];


    }

    // 링크스킬, 스킬데이터가 장착된 스킬을 유닛 데이터에 할당해주는 함수
    public void Set_UnitSkillData(string unit_ID)
    {
        // 몬스터가 아닐 경우 (플레이어 유닛일 경우 일 때만)
        bool isNotMonster = unit_ID != "orc_warr01" && unit_ID != "orc_hunt01" && unit_ID != "orc_sham01" && unit_ID != "orc_boss01";

        _unitInfo_Dictionary[unit_ID].unit_Gen_Skill = skillDataManagerCs.Set_skill_Dictionary[_unitInfo_Dictionary[unit_ID].generalSkill];

        // 플레이어 유닛일 경우
        if (isNotMonster)
        {
            // 유닛 ID에 해당하는 첫번째 특수스킬 장착
            _unitInfo_Dictionary[unit_ID].unit_Spc_Skill = skillDataManagerCs.Set_skill_Dictionary[_unitInfo_Dictionary[unit_ID].specialSkill1];

            // 유닛 ID에 해당하는 두번째 특수스킬 장착
            _unitInfo_Dictionary[unit_ID].unit_Spc_Skill2 = skillDataManagerCs.Set_skill_Dictionary[_unitInfo_Dictionary[unit_ID].specialSkill2];
        }

        // 플레이어와 몬스터 둘 다 유닛 데이터에 해당하는 방어구 속성 장착
        _unitInfo_Dictionary[unit_ID].unit_Armor_property = Instance._armor_Dictionary[_unitInfo_Dictionary[unit_ID].defenseType];
        _unitInfo_Dictionary[unit_ID].unit_ArmorCalculateCs = Instance._armorCs_Dictionary[_unitInfo_Dictionary[unit_ID].unit_Armor_property];

        // 타겟 선정 타입 할당
        _unitInfo_Dictionary[unit_ID].unit_targetSelectType = Instance._targetSelect_Dictionary[_unitInfo_Dictionary[unit_ID].targetSelectType];
    }


    #region # Add_Armor_Dictionary() : 방어속성 딕셔너리에 키, 값 추가해주는 함수
    private void Add_Armor_Dictionary()
    {
        // 패딩 갑옷 키, 값 추가
        _armor_Dictionary.Add(key : "padd", value : eUnit_Defense_Property_States.padding_Armor);

        // 판금 갑옷 아머 키, 값 추가
        _armor_Dictionary.Add(key : "plate", value : eUnit_Defense_Property_States.plate_Armor);

        // 쇠사슬 갑옷 키, 값 추가
        _armor_Dictionary.Add(key : "chain", value : eUnit_Defense_Property_States.chain_Armor);


    }
    #endregion

    #region # Add_TargetSelect_Dictionary() : 타겟 선정 딕셔너리에 키, 값 추가해주는 함수
    private void Add_TargetSelect_Dictionary()
    {
        // 타겟 고정 타겟 선정 타입 키, 값 할당
        _targetSelect_Dictionary.Add(key : "타겟 고정", value : eUnit_targetSelectType.fixed_Target);

        // 낮은 체력 타겟 선정 타입 키, 값 할당
        _targetSelect_Dictionary.Add(key : "낮은 체력", value : eUnit_targetSelectType.low_Health_Target);

        // 가까운 적 타겟 선정 타입 키, 값 할당
        _targetSelect_Dictionary.Add(key : "가까운 적", value : eUnit_targetSelectType.nearest_Target);

    }
    #endregion

    #region # Add_UnitData_Dictionary() : 유닛 데이터 딕셔너리에 키, 값 추가해주는 함수
    private void Add_UnitData_Dictionary()
    {
        
        // 궁수 데이터 키, 값 할당
        _unitInfo_Dictionary.Add(key : All_character_Datas.CharacterDatas[0].char_id, value : All_character_Datas.CharacterDatas[0]);

        // 기사 데이터 키, 값 할당
        _unitInfo_Dictionary.Add(key : All_character_Datas.CharacterDatas[1].char_id, value : All_character_Datas.CharacterDatas[1]);

        // 망치 기사 데이터 키, 값 할당
        _unitInfo_Dictionary.Add(key: All_character_Datas.CharacterDatas[2].char_id, value: All_character_Datas.CharacterDatas[2]);


        // 오크 헌터 데이터 키, 값 할당
        _unitInfo_Dictionary.Add(key: All_character_Datas.CharacterDatas[3].char_id, value: All_character_Datas.CharacterDatas[3]);

        // 오크 주술사 데이터 키, 값 할당
        _unitInfo_Dictionary.Add(key: All_character_Datas.CharacterDatas[4].char_id, value: All_character_Datas.CharacterDatas[4]);

        // 오크 전사 데이터 키, 값 할당
        _unitInfo_Dictionary.Add(key: All_character_Datas.CharacterDatas[5].char_id, value: All_character_Datas.CharacterDatas[5]);

        // 오크 보스 데이터 키, 값 할당
        _unitInfo_Dictionary.Add(key: All_character_Datas.CharacterDatas[6].char_id, value: All_character_Datas.CharacterDatas[6]);
    }
    #endregion

    #region # Add_ArmorScript_Dictionary() : 방어구 속성에 따른 스크립트 키, 값 추가해주는 함수
    private void Add_ArmorScript_Dictionary()
    {
        // 판금 갑옷
        _armorCs_Dictionary.Add(key : eUnit_Defense_Property_States.plate_Armor, value : new PlateArmor());

        // 쇠사슬 갑옷
        _armorCs_Dictionary.Add(key : eUnit_Defense_Property_States.chain_Armor, value: new ChainArmor());

        // 천갑옷
        _armorCs_Dictionary.Add(key: eUnit_Defense_Property_States.padding_Armor, value: new PaddingArmor());
    }
    #endregion


}
