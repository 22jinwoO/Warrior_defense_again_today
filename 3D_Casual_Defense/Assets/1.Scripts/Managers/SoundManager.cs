using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public Transform soundPos;

    void Awake()
    {
        soundPos = GameObject.FindGameObjectWithTag("SoundPos").transform;
    }

    public float VolumeCheck(Transform Tr)
    {
        float distance = Vector3.Distance(Tr.position, soundPos.position);
        // sfxVol = 0f;

        if (distance <= 10f)
            return 1f;

        if (distance <= 15f)
            return 0.7f;

        if (distance <= 20f)
            return 0.4f;

        if (distance <= 25f)
            return 0.2f;
        else
            return 0.1f;
    }
}
