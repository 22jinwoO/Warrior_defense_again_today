using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.GameCenter;

public class Rush : SpecialSkill
{
    
    public override void Attack_Skill()
    {
        Collider[] hitCols = Physics.OverlapSphere(transform.position, 20f,unitTargetSearchCs._layerMask);

        Transform _longTarget = null;  // 가장 가까운 적을 의미하는 변수

        //print(_cols.Length);  
        if (hitCols.Length <= 0)  // 탐지된 적이 없다면 함수 탈출
        {
            //print("적 없음!");
            return;
        }

        float _longDistance = 0f;   // 거리 무한 값 할당

        foreach (var _colTarget in hitCols)
        {
            float _distance = Vector3.SqrMagnitude(transform.position - _colTarget.transform.position);
            if (_distance > _longDistance)
            {
                _longDistance = _distance;
                _longTarget = _colTarget.transform;
            }
        }
        //
        unitTargetSearchCs._targetUnit = _longTarget; // 거리가 가장 가까운 적 타겟을 _targetUnit 변수에 할당
        unitTargetSearchCs._target_Body = _longTarget.GetComponent<UnitInfo>().body_Tr;
        //print(_targetUnit.name);
        unitInfoCs._isSearch = true;

        // 유닛 회피 무시 상태로 전환
        unitInfoCs._nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        unitInfoCs._nav.speed = 14f;
        unitInfoCs._nav.acceleration = 20f;
        unitInfoCs._nav.SetDestination(_longTarget.position);

        print("애니메이션 호출 함수");
    }

    // 충돌하면 공격 애미네이션 실행하고 데미지 들어가게 하도록

    
}
