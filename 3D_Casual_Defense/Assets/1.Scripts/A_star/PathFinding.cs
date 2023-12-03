using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    A_Grid grid;

    public Transform startObject;
    public Transform targetObject;


    private void Awake()
    {
        grid = GetComponent<A_Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        A_Node startNode = grid.GetNodeFromWorldPoint(startPos);
        A_Node targetNode = grid.GetNodeFromWorldPoint(targetPos);

        List<A_Node> openList = new List<A_Node>();
        HashSet<A_Node> closedList = new HashSet<A_Node>();
        openList.Add(startNode);

        while (openList.Count>0)
        {
            A_Node currentNode = openList[0];
            // 열린 목록에 F cost가 가장 작은 노드를 찾는다, 만약에 F 코스트가 같다면 H cost를 계산하여 열린 목록에 추가한다.
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost.Equals(currentNode.fCost) && openList[i].hCost<currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            // 탐색된 노드는 열린 목록에서 제거하고 끝난 목록에 추가한다.
            openList.Remove(currentNode);
            closedList.Add(currentNode);


            // 탐색된 노드가 목표 노드라면 탐색 종료
            if (currentNode.Equals(targetNode))
            {
                
            }
        }

        

    }
}
