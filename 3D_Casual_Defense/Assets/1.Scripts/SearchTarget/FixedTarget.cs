using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTarget : AbsSearchTarget
{
    private void Awake()
    {
        unitInfoCs = GetComponent<NewUnitInfo>();
    }

    public override void SearchTarget()
    {
        {
            // 오버랩 스피어 생성
            Collider[] _cols = Physics.OverlapSphere(transform.position, unitInfoCs._unitData.sightRange, _layerMask); 

            foreach (var item in _cols)
            {
                print(item.gameObject.name);
            }



            // 탐지된 적이 없다면 함수 탈출
            if (_cols.Length <= 0)  
            {
                unitInfoCs._nav.isStopped = true;
                return;
            }

            // 각도 전환 스크립트, 공격 스크립트 따로 추가하기
            unitInfoCs._nav.isStopped = false;

            // 가장 가까운 적을 의미하는 변수
            Transform _shortestTarget = null;

            // 거리 무한 값 할당
            float _shortestDistance = _shortestDistance = Vector3.Distance(transform.position, _cols[0].transform.position);

            // 선택 정렬
            for (int i = 0; i < _cols.Length; i++)
            {

                int location = i;

                for (int j = i + 1; j < _cols.Length; j++)
                {
                    if (_shortestDistance >= Vector3.Distance(transform.position, _cols[j].transform.position))
                    {
                        _shortestDistance = Vector3.Distance(transform.position, _cols[j].transform.position);
                        _shortestTarget = _cols[j].transform;
                        location = j;
                    }
                }

                Collider temp = null;

                temp = _cols[location];
                _cols[location] = _cols[i];
                _cols[i] = temp;

                //_shortestTarget = _cols[i].transform;
                //_shortestDistance = Vector3.Distance(transform.position, _cols[i].transform.position);
            }

            _shortestTarget = _cols[0].transform;

            //시야범위 밖의 타겟인데 자꾸 타겟을 인식해서 예외처리 구문 추가
            if (_shortestDistance > unitInfoCs._unitData.sightRange)
            {
                unitInfoCs._isSearch = false;
                print(gameObject.name + " : " + _shortestDistance + "예외처리 실행");
                return;
            }

            _targetUnit = _shortestTarget; // 거리가 가장 가까운 적 타겟을 _targetUnit 변수에 할당
            _target_Body = _shortestTarget.GetComponent<NewUnitInfo>().body_Tr;



            _actStateCs.Exit();

            // 타겟 바라보는 상태로 변환 (추격)
            //unitInfoCs._enum_Unit_Action_State = unitInfoCs._enum_Unit_Attack_State;    // ㅇㅇ 업데이트에서 FSM 상태 실행중

        }
    }
}
