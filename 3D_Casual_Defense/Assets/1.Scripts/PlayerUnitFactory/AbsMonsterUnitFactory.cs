using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsMonsterUnitFactory : MonoBehaviour
{
    // 오크 프리팹
    public MonsterUnitClass monsterPrefab;

    // 오브젝트 풀링을 위한 몬스터 유닛 풀
    public Stack<MonsterUnitClass> monsters = new Stack<MonsterUnitClass>();

    // 인스펙터 창을 정리하기 위한 팩토리 프리팹 리스트
    [SerializeField]
    protected Transform prefabLists;

    #region #InitObjPool() : 오브젝트 풀링 초기값 셋팅하는 함수
    public void InitObjPool(MonsterUnitClass playerUnitPref)
    {
        MonsterUnitClass monsterUnit = null;


        // 오브젝트 풀링을 위해 프리팹 미리 생성
        for (int i = 0; i < 10; i++)
        {
            monsterUnit = Instantiate(playerUnitPref);
            monsterUnit.gameObject.SetActive(false);
            monsterUnit.transform.SetParent(prefabLists);
            monsterUnit.monsterFactory = this;
            monsters.Push(monsterUnit);
        }

    }
    #endregion

    // 몬스터 유닛 생산하는 생성자 함수
    public abstract MonsterUnitClass CreateMonsterUnit();
}
