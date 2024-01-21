using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitDataManager;

public abstract class MonsterUnitClass : UnitInfo
{
    [Header("몬스터 유닛 머태리얼")]
    public Material mosterUnitMtr;

    public abstract void InitUnitInfoSetting(CharacterData character_Data);     // 유닛 정보 초기화 시켜주는 함수

}
