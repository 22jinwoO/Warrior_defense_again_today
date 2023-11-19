using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterUnitClass : UnitInfo
{
    [SerializeField]
    protected UnitTargetSearch unitTargetSearchCs;

    public abstract void Act_By_Unit(); // 유닛 상태에 따라서 행동하는 동작들을 구현하는 함수

    public abstract void InitUnitInfoSetting();     // 유닛 정보 초기화 시켜주는 함수

}
