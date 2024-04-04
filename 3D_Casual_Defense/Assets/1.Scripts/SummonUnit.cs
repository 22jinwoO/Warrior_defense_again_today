using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonUnit : MonoBehaviour
{
    protected float curGage; //* 현재 체력
    public float maxGage; //* 최대 체력

    //public Slider gageSlider;

    public string playerID;

    public CreatePlayerUnit unitFactory;

    public int btnIndex;

    public Vector3 myPos;

    public Vector3 unitTr;

    private void Awake()
    {
        //curGage = 3f;
        //maxGage = curGage;
        //transform.position = unitTr;
    }

    private void Update()
    {
        // 포탈이 마우스 드랍이 끝난 위치에 소환되게 하고 걔가 컨버스 갖고 있게끔 만들어보기

        //transform.localPosition = myPos;
        //transform.position = unitTr;
        // 오브젝트에 따른 HP Bar 위치 이동
        //transform.localPosition = Camera.main.WorldToScreenPoint(portal.position + new Vector3(0, -0.8f, 0));
    }

    #region # CreateUnit() : 플레이어 유닛 생산자
    public IEnumerator CreateUnit()
    {
        float time = 0f;

        while (time <= 3f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // 버튼인덱스에 해당하는 플레이어 유닛 팩토리 찾아서 유닛 생산
        PlayerUnitClass unit = unitFactory.playerUnitFactorys[btnIndex].CreatePlayerUnit();

        // 유닛 초기값 할당하는 함수 호출
        unit.InitUnitInfoSetting(UnitDataManager.Instance._unitInfo_Dictionary[unitFactory.playerUnitFactorys[btnIndex].unitId]);

        // 유닛 오브젝트 활성화
        unit.gameObject.SetActive(true);

        unit.transform.position = unitTr;

        yield return null;

        // 소환 이펙트 파괴
        Destroy(gameObject);
    }
    #endregion
}
