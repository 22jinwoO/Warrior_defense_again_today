using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonUnit : MonoBehaviour
{
    protected float curGage; //* 현재 체력
    public float maxGage; //* 최대 체력

    public Slider gageSlider;

    private void Awake()
    {
        curGage = 3f;
        maxGage = curGage;

        StartCoroutine(CreateUnit());
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

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
