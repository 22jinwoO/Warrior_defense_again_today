using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    [SerializeField]
    Vector3 spawnPoint;

    [SerializeField]
    AbsMonsterUnitFactory[] monsterUnitFactorys;

    public NavMeshSurface nms;

    private void Awake()
    {
        //nms = GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<NavMeshSurface>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TestTime());

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateOrc();
        }
    }

    private void CreateOrc()
    {
        //  유닛 생산자
        print("스페이스 바 눌림!");
        monsterUnitFactorys[0].orcClass = AbsMonsterUnitFactory.OrcClass.Orc;
        MonsterUnitClass orc = monsterUnitFactorys[0].CreateMonsterUnit();
        orc.transform.position = spawnPoint;
        orc.gameObject.name = "오크";
    }
    IEnumerator TestTime()
    {
        yield return new WaitForSeconds(5f);
        print("초기화");
        nms.BuildNavMesh();
        StartCoroutine(TestTime());


    }
}
