using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsMonsterUnitFactory : MonoBehaviour
{
    [SerializeField]
    public Vector3 spawnPoint = new Vector3(5.1f, 8.5f, 5.9f);

    // 오크 프리팹
    public MonsterUnitClass monsterPrefab;
    public enum OrcClass
    {
        Default = 0,
        Orc

    }
    public OrcClass orcClass;

    // 몬스터 유닛 생산하는 생성자 함수
    public abstract MonsterUnitClass CreateMonsterUnit();
}
