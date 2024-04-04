using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAnim : MonoBehaviour
{
    [SerializeField]
    private GameObject circleObj;

    [SerializeField]
    private float rotSpeed;

    [SerializeField]
    private GameObject holdObj;


    void Update()
    {
        //transform.Rotate(new Vector3(0, 0, rotSpeed * Time.deltaTime));
    }
}
