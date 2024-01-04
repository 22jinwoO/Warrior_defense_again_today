using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Abs_Bullet : MonoBehaviour
{
    // 스킬
    public Abs_Skill _skill;

    // 타겟 유닛
    public Transform _target_Unit;

    // 타겟 유닛
    public Transform _target_BodyTr;

    //타겟을 바라보는 방향
    public Vector3 _target_Direction;

    // 생성 위치
    public Transform _start_Pos;

    // 발사한 유닛의 UnitInfo.cs
    public UnitInfo unitInfoCs;

    // 공격 데미지
    public float atkDmg;

}
