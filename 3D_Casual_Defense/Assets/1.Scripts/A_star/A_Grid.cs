using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Grid : MonoBehaviour
{

    public LayerMask WayMask;
    public LayerMask UnwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;    //노드 반지름
    A_Node[,] grid; // 노드 배열

    private float nodeDiameter; // 노드의 지름
    private int gridSizeX;  //그리드의 가로 길이
    private int gridSizeY;  // 그리드의 새로 길이


    private void Start()
    {
        nodeDiameter = nodeRadius * 2; // 설정한 반지름으로 지름을 구함
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter); // 그리드의 가로 사이즈
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter); // 그리드의 가로 사이즈

        CreateGrid();

    }

    private void CreateGrid()   // 그리드 만들어주는 함수
    {
        grid = new A_Node[gridSizeX,gridSizeY]; // 그리드의 가로와 세로사이즈 만큼 노드 이차원 배열 생성

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward*gridWorldSize.y/2; // 자신의 위치 벡터값 - 30 / 15 - 30/15
        print(worldBottomLeft); // transform.position에 맵 위치를 넣고, 맵위치에서 그리드 가로 반지름과, 그리드 세로사이즈 반지름을 빼줘서 시작 위치를 구함.
        Vector3 worldPoint;

        for(int x = 0; x<gridSizeX; x++) 
        {
            // 내가 만든 게임에서는 노드가 길이랑만 충돌했을 때 이동 가능하도록 해야함..
            for (int y = 0; y < gridSizeY; y++)
            {
                worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                // 월드 좌표 = X 값 : 월드 좌표 왼쪽 시작점 + 1,0,0 * (x * 노드 지름 + 노드 반지름) + Y값 : 0,0,1 * (y * 노드 지름 + 노드 반지름)
                bool walkable = (Physics.CheckSphere(worldPoint, nodeRadius, WayMask)); // Physics.CheckSphere에 WayMask 레이어마스크를 가진 장애물과 충돌했다면 true, 충돌하지 않았다면 false를 반환

                if (walkable.Equals(true))
                {
                    walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, UnwalkableMask)); // Physics.CheckSphere에 UnwalkableMask레이어 마스크를 가진 유닛과 충돌했다면 true, 충돌하지 않았다면 false를 반환하기 때문에 ! 를 붙여줘야함.
                }

                grid[x, y] = new A_Node(walkable, worldPoint,x,y);  // grid[x, y] 이차원 배열 값에 노드 생성

            }
        }


    }

    // 노드의 주변 노드 (8 방면)를 찾는 함수
    public List<A_Node> GetNeighbors(A_Node node)
    {
        List<A_Node> neighbors = new List<A_Node>();
        for(int x= -1; x<=1;x++)
        {
            for(int y =-1; y <=1; y++)
            {
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // X,Y의 값이 Grid 범위 안에 있을 경우
                if(checkX >= 0 && checkX< gridSizeX && checkY >=0&&checkY<gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    // 유니티의 WorldPosition으로 부터 그리드 상의 노드를 찾는 함수
    public A_Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if(grid!=null)
        {
            foreach(A_Node n in grid)
            {
                Gizmos.color = (n.isWalkAble) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }

}
