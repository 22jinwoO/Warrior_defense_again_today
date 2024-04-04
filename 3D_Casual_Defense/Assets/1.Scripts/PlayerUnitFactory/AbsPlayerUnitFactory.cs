using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsPlayerUnitFactory : MonoBehaviour
{
    // 오브젝트 풀링을 위한 플레이어 유닛 풀
    public Stack<PlayerUnitClass> units = new Stack<PlayerUnitClass>();

    public PlayerUnitClass playerUnitPrefab;

    public string unitId;

    // 인스펙터 창을 정리하기 위한 팩토리 프리팹 리스트
    [SerializeField]
    protected Transform prefabLists;

    #region #InitObjPool() : 오브젝트 풀링 초기값 셋팅하는 함수
    public void InitObjPool(PlayerUnitClass playerUnitPref)
    {
        PlayerUnitClass playerUnit = null;


        // 오브젝트 풀링을 위해 프리팹 미리 생성
        for (int i = 0; i < 10; i++)
        {
            playerUnit = Instantiate(playerUnitPref);
            playerUnit.gameObject.SetActive(false);
            playerUnit.transform.SetParent(prefabLists);
            playerUnit.unitFactory = this;
            units.Push(playerUnit);
        }

    }
    #endregion

    // 플레이어 유닛 생산하는 생성자 함수
    public abstract PlayerUnitClass CreatePlayerUnit();
}
