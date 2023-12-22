using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsPlayerUnitFactory : MonoBehaviour
{
    public enum KnightClass
    {
        Default = 0,
        Knight

    }

    public enum ArcherClass
    {
        Default = 0,
        Archer

    }
    public KnightClass knightClass;

    public ArcherClass archerClass;

    // 플레이어 유닛 생산하는 생성자 함수
    public abstract PlayerUnitClass CreatePlayerUnit();
}
