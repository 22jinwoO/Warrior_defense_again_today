using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Node
{
    public bool isWalkAble; // 이동 가능한 노드인지 확인하는 변수
    public Vector3 worldPos;    // 유니티 상의 월드 좌표
    public int gridX;           // 그리드 가로
    public int gridY;           // 그리드 세로

    public int gCost;       // G 코스트 시작 노드에서 현재 지정된 길찾기 후보 노드까지의 거리
    public int hCost;       // H 코스트 목적지 노드에서 현재 지정된 길찾기 후보 노드까지의 거리
    public A_Node parentNode;   // 부모 노드



    public A_Node(bool nWalkable, Vector3 nWorldPos, int nGridX, int nGridY) // nWalkable 갈 수 있는 길인지 , nWorldPos 노드의 월드좌표
    {
        isWalkAble = nWalkable;
        worldPos = nWorldPos;
        gridX = nGridX;
        gridY = nGridY;

    }

    public int fCost    // 프로퍼티 사용, F 코스트는 G코스트 + H코스트 핪의 값을 가짐
    {
        get { return gCost + hCost; }
    }
}
