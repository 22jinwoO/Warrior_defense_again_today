using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerUnitSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private Renderer spawnMaterial;

    public bool canCreate;

    private Color spawnColor;

    private void Awake()
    {
        canCreate = true;
        spawnColor= new Color(0f, 0.99f, 0.36f);
        spawnMaterial = GetComponent<Renderer>();
        spawnMaterial.material.color = spawnColor;
    }

    public void SetBase()
    {
        canCreate = true;
        spawnMaterial.material.color = spawnColor;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            canCreate = false;
            print("충돌 됨 - 태그");
            spawnMaterial.material.color = Color.red;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            canCreate = true;

            print("충돌 나가기 - 태그");
            spawnMaterial.material.color = spawnColor;
        }
    }
}
