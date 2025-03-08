using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsSearchTarget : MonoBehaviour
{
    [SerializeField]
    protected NewUnitInfo unitInfoCs;

    [SerializeField]
    protected LayerMask _layerMask = 0;   // 오버랩스피어 레이어 마스크 변수

    public Transform _targetUnit = null;   // 유닛이 타겟으로 할 대상

    public Transform _target_Body = null;   // 타겟 대상의 바디 트랜스폼

    public IUnitActState _actStateCs = null;

    public abstract void SearchTarget();
}
