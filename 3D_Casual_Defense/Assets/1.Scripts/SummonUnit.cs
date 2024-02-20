using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonUnit : MonoBehaviour
{
    protected float curGage; //* 현재 체력
    public float maxGage; //* 최대 체력

    public Slider gageSlider;

    public string playerID;

    public CreatePlayerUnit unitFactory;

    public int btnIndex;

    public Vector3 myPos;

    public Vector3 unitTr;

    private void Awake()
    {
        curGage = 3f;
        maxGage = curGage;
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
    public IEnumerator CreateUnit()
    {
        float time = 0f;

        while (time <= 3f)
        {
            gageSlider.value = time / maxGage;
            time += Time.deltaTime;
            print(time);
            yield return null;
            //
        }
        print(unitFactory);
        print(unitFactory.playerUnitFactorys[1]);
        PlayerUnitClass unit = unitFactory.playerUnitFactorys[btnIndex].CreatePlayerUnit();

        print(unit.name);
        unit.gameObject.SetActive(true);
        unit.transform.position = unitTr;

        yield return null;
        Destroy(gameObject);
    }
}
