using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField]
    Camera[] cameras = new Camera[2];

    private void Awake()
    {
        //Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //Screen.SetResolution(1920, 1080, true);

        for (int i = 0; i < cameras.Length; i++)
        {
            Rect rect = cameras[i].rect;

            float scaleHeight = ((float)Screen.width / Screen.height) / ((float)16 / 9);
            float scaleWidth = 1f / scaleHeight;

            if (scaleHeight < 1)
            {
                rect.height = scaleHeight;
                rect.y = (1f - scaleHeight) / 2f;
            }
            else
            {
                rect.width = scaleWidth;
                rect.x = (1f - scaleWidth) / 2f;
            }
            cameras[i].rect = rect;

        }
    }
}
