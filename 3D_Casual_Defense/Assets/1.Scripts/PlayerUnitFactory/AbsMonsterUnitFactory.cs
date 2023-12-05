using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsMonsterUnitFactory : MonoBehaviour
{
    public enum OrcClass
    {
        Default = 0,
        Orc

    }
    public OrcClass orcClass;

    // 플레이어 유닛 생산하는 생성자 함수
    public abstract MonsterUnitClass CreateMonsterUnit();
}
