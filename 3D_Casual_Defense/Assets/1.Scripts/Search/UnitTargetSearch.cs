using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTargetSearch : MonoBehaviour
{
    private UnitInfo unitInfoCs;    // 유닛 인포 스크립트

    [SerializeField]
    private ActUnit actUnitCs;    // 유닛 행동 스크립트

    private void Awake()
    {
        unitInfoCs = GetComponent<UnitInfo>();
        //actUnitCs = GetComponent<ActUnit>();
        if (gameObject.CompareTag("Monster"))
        {
            actUnitCs = GetComponent<ActUnit>();
        }
        //print(actUnitCs);
    }

    // 구조체 필드 이니셜 라이징은 C# 9.0 에서 지원을 하지 않기 때문에 클래스를 따로 만듦

    public LayerMask _layerMask = 0;   // 오버랩스피어 레이어 마스크 변수

    public Transform _targetUnit = null;   // 유닛이 타겟으로 할 대상

    public bool isTracking; // 추적 확인 변수

    #region # Search_For_Near_Enemy() : 가장 가까운 적 탐지하는 함수 , 시야 범위에서 적 인식
    public void Search_For_Near_Enemy() // 가장 가까운 적 탐지하는 함수 , 시야 범위에서 적 인식
    {
        Collider[] _cols = Physics.OverlapSphere(transform.position, unitInfoCs._unitData._unit_Outlook, _layerMask); // 오버랩 스피어 생성
        Transform _shortestTarget = null;  // 가장 가까운 적을 의미하는 변수

        //print(_cols.Length);  
        if (_cols.Length <= 0)  // 탐지된 적이 없다면 함수 탈출
        {
            //print("적 없음!");
            return;
        }

        //CancelInvoke(); // 근데 이게 안돼

        float _shortestDistance = Mathf.Infinity;   // 거리 무한 값 할당

        foreach (var _colTarget in _cols)
        {
            float _distance = Vector3.SqrMagnitude(transform.position - _colTarget.transform.position);
            if (_shortestDistance > _distance)
            {
                _shortestDistance = _distance;
                _shortestTarget = _colTarget.transform;
            }
        }
        //
        _targetUnit = _shortestTarget; // 거리가 가장 가까운 적 타겟을 _targetUnit 변수에 할당

        //print(_targetUnit.name);
        unitInfoCs.isSearch = true;
        unitInfoCs._enum_Unit_Action_Type = unitInfoCs._enum_Unit_Attack_Type;    // ㅇㅇ 업데이트에서 FSM 상태 실행중

    }
    #endregion


    #region # Look_At_The_Target() : 유닛이 타겟을 감지 했을 때 타겟 쪽으로 몸을 회전하여 타겟을 바라보는 함수
    public void Look_At_The_Target()    // 유닛이 타겟을 감지 했을 때 타겟 쪽으로 몸을 회전하여 타겟을 바라보는 함수
    {
        Vector3 dir = _targetUnit.position - transform.position;
        //dir.Normalize();
        //dir.y = 0;

        Quaternion _lookRotation = Quaternion.LookRotation(dir.normalized);  // 타겟 쪽으로 바라보는 각도

        Vector3 _euler = Quaternion.RotateTowards(transform.rotation, _lookRotation, 100f * Time.deltaTime).eulerAngles;

        transform.rotation = Quaternion.Euler(0, _euler.y, 0);

        Quaternion _fireRotation = Quaternion.Euler(0, _lookRotation.eulerAngles.y, 0); // 유닛이 발사할 수 있는 방향의 각도


        if (Quaternion.Angle(transform.rotation, _fireRotation) <= 5f)
        {
            _euler.y = 0;
            //print("용사와 몬스터의 각도 값 : " + Quaternion.Angle(transform.rotation, _fireRotation));

            if (unitInfoCs._enum_Unit_Action_Type != eUnit_Action_States.unit_Attack)
                return;

            actUnitCs.Attack_Unit();
            // 1. 공격 속도 쿨타임 감소

            // 2. 공격속도 쿨타임이 0보다 작아졌다면 발사 후 공격속도 변수에 초기값 다시 넣어줌

        }
    }
    #endregion


}
