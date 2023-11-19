using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField]
    GameObject _groundPref;

    [SerializeField]
    Vector3[,] _grounds_Transforms = new Vector3[10,10];

    float floatX, floatZ;

    private void Awake()
    {
        for (int i = 0; i < _grounds_Transforms.GetLength(0); i+=2)
        {
            floatZ = 0f;

            for (int j = 0; j < _grounds_Transforms.GetLength(1); j++)
            {
                if (j%2==0)
                {
                    GameObject groundPref = Instantiate(_groundPref);
                    _grounds_Transforms[i, j] = new Vector3(i, 0f, floatZ);
                    groundPref.transform.position = _grounds_Transforms[i, j];
                    floatZ += 2f;
                }

            }
        }
    }
}
