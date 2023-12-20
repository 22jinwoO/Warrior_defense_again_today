using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterUnitClass : UnitInfo
{
    [Header("유닛의 타겟 선정 타입마다 필요한 함수들을 구현해놓은 UnitTargetSearch 스크립트")]
    [SerializeField]
    protected UnitTargetSearch unitTargetSearchCs;


    [Header("유닛의 행동들을 구현해놓은 ActUnit 스크립트")]
    [SerializeField]
    protected ActUnit actUnitCs;

    public abstract void InitUnitInfoSetting();     // 유닛 정보 초기화 시켜주는 함수

}
