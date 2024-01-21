using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsMonsterUnitFactory : MonoBehaviour
{
    [SerializeField]
    public Vector3 spawnPoint = new Vector3(13.5f, 0, -5f);


    public enum OrcClass
    {
        Default = 0,
        Orc

    }
    public OrcClass orcClass;

    // 몬스터 유닛 생산하는 생성자 함수
    public abstract MonsterUnitClass CreateMonsterUnit();
}
