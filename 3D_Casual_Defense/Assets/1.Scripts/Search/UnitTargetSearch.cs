using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTargetSearch : MonoBehaviour
{
    private UnitInfo unitInfoCs;    // 유닛 인포 스크립트

    [SerializeField]
    private ActUnit actUnitCs;    // 유닛 행동 스크립트


    public Transform _unitModelTr;

    [SerializeField]
    private MonsterUnitClass monsterUnit;



    private void Awake()
    {
        unitInfoCs = GetComponent<UnitInfo>();
        actUnitCs = GetComponent<ActUnit>();
        if (TryGetComponent(out MonsterUnitClass Mc))
        {
            monsterUnit = Mc;
        }
        //if (gameObject.CompareTag("Monster"))
        //{
        //    actUnitCs = GetComponent<ActUnit>();
        //}
        //print(actUnitCs);
    }

    // 구조체 필드 이니셜 라이징은 C# 9.0 에서 지원을 하지 않기 때문에 클래스를 따로 만듦

    public LayerMask _layerMask = 0;   // 오버랩스피어 레이어 마스크 변수

    public Transform _targetUnit = null;   // 유닛이 타겟으로 할 대상

    public Transform _target_Body = null;   // 유닛이 타겟으로 할 대상

    public bool isTracking; // 추적 확인 변수

    #region # Search_For_Near_Enemy() : 가장 가까운 거리의 적을 고정으로 탐지하는 함수 , 시야 범위에서 적 인식
    public void Search_For_Fixed_Target() // 고정 타겟을 탐지하는 함수 , 시야 범위에서 적 인식
    {

        Collider[] _cols; _cols = Physics.OverlapSphere(transform.position, unitInfoCs._unitData.sightRange, _layerMask); // 오버랩 스피어 생성

        foreach (var item in _cols)
        {
            print(item.gameObject.name);
        }
        Transform _shortestTarget = null;  // 가장 가까운 적을 의미하는 변수


        if (_cols.Length <= 0)  // 탐지된 적이 없다면 함수 탈출
        {
            return;
        }

        float _shortestDistance = 0f;

        // 선택 정렬
        for (int i = 0; i < _cols.Length; i++)
        {
            // 거리 기준 값 할당
            _shortestDistance = Vector3.Distance(transform.position, _cols[i].transform.position);

            int location = i;

            for (int j = i + 1; j < _cols.Length; j++)
            {
                if (_shortestDistance >= Vector3.Distance(transform.position, _cols[j].transform.position))
                {
                    // 정렬된 배열의 j번째 값을 타겟으로 부여
                    _shortestTarget = _cols[j].transform;

                    _shortestDistance = Vector3.Distance(transform.position, _cols[j].transform.position);

                    location = j;
                }
            }

            Collider temp = null;

            temp = _cols[location];
            _cols[location] = _cols[i];
            _cols[i] = temp;

        }



        //시야범위 밖의 타겟인데 자꾸 타겟을 인식해서 예외처리 구문 추가
        if (_shortestDistance > unitInfoCs._unitData.sightRange)
        {
            unitInfoCs._isSearch = false;
            print(gameObject.name + " : " + _shortestDistance + "예외처리 실행");
            return;
        }
        _shortestTarget = _cols[0].transform;
        _targetUnit = _shortestTarget; // 거리가 가장 가까운 적 타겟을 _targetUnit 변수에 할당
        _target_Body = _shortestTarget.GetComponent<UnitInfo>().body_Tr;

        //print(_targetUnit.name);
        if (!unitInfoCs._isTargetDead)
        {
            unitInfoCs._isSearch = true;

        }

        for (int i = 0; i < _cols.Length; i++)
        {
            Debug.LogWarning(Vector3.Distance(transform.position, _cols[i].transform.position));
        }

        // 타겟 바라보는 상태로 변환 (추격)
        unitInfoCs._enum_Unit_Action_State = unitInfoCs._enum_Unit_Attack_State;    // ㅇㅇ 업데이트에서 FSM 상태 실행중

    }
    #endregion

    #region # Search_For_longest_Target() : 가장 멀리있는 적 탐지하는 함수 , 시야 범위에서 적 인식
    public void Search_For_longest_Target() // 가장 멀리있는 적 탐지하는 함수 , 시야 범위에서 적 인식
    {
        //_cols = null;
        Collider[] _cols = Physics.OverlapSphere(transform.position, unitInfoCs._unitData.sightRange, _layerMask); // 오버랩 스피어 생성

        Transform _longestTarget = null;  // 가장 가까운 적을 의미하는 변수

        for (int t = 0; t < _cols.Length; t++)
        {
            Debug.LogWarning(t+" "+_cols[t].transform.position);
        }

        if (_cols.Length <= 0)  // 탐지된 적이 없다면 함수 탈출
        {
            return;
        }

        // 삽입 정렬
        int i, j;

        Collider Key;

        for (i = 1; i < _cols.Length; i++)
        {
            float _LongDistance = Vector3.Distance(transform.position, _cols[i].transform.position);

            Key = _cols[i];

            for (j = i - 1; j >= 0; j--)
            {
                if ( _LongDistance >= Vector3.Distance(transform.position, _cols[j].transform.position))
                {
                    _cols[j + 1] = _cols[j];
                }

                else
                    break;
            }

            _cols[j + 1] = Key;
        }

        _longestTarget = _cols[0].transform;

        for (i = 0; i < _cols.Length; i++)
        {
            Debug.LogWarning(Vector3.Distance(transform.position, _cols[i].transform.position));
        }

        _targetUnit = _longestTarget;  // 거리가 가장 먼 적을  _targetUnit 변수에 할당
        _target_Body = _longestTarget.GetComponent<UnitInfo>().body_Tr;

        unitInfoCs._isSearch = true;
        unitInfoCs._enum_Unit_Action_State = unitInfoCs._enum_Unit_Attack_State;

    }
    #endregion

    #region # Search_For_Lowhealth_Target() : 가장 낮은 체력의 타겟을 탐지하는 함수 , 시야 범위에서 적 인식
    public void Search_For_Lowhealth_Target() // 가장 체력이 낮은 적을 탐지하는 함수 , 시야 범위에서 적 인식
    {
        Collider[] _cols = Physics.OverlapSphere(transform.position, unitInfoCs._unitData.sightRange, _layerMask); // 오버랩 스피어 생성

        Transform _lowHeatlh_Target = null;  // 가장 가까운 적을 의미하는 변수

        if (_cols.Length <= 0)  // 탐지된 적이 없다면 함수 탈출
        {
            return;
        }

        int low = 0;
        int high = _cols.Length - 1;

        //퀵정렬
        QuickSort(_cols, low, high);

        // 낮은 체력으로 정렬된 몬스터 배열의 0번째를 타겟으로 할당
        _lowHeatlh_Target = _cols[0].transform;

        _targetUnit = _lowHeatlh_Target; // 가장 체력이 낮은 타겟을 _targetUnit 변수에 할당

        _target_Body = _lowHeatlh_Target.GetComponent<UnitInfo>().body_Tr;


        unitInfoCs._isSearch = true;

        unitInfoCs._enum_Unit_Action_State = unitInfoCs._enum_Unit_Attack_State;    // ㅇㅇ 업데이트에서 FSM 상태 실행중

    }
    #endregion

    #region # QuickSort() 함수 : 퀵정렬 기능 해주는 함수
    public void QuickSort(Collider[] arr, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(arr, low, high);
            QuickSort(arr, low, pivotIndex - 1);
            QuickSort(arr, pivotIndex + 1, high);
        }

        for (int i = 0; i < arr.Length; i++)
        {
            Debug.LogWarning($"{i}번째 유닛 HP : { arr[i].GetComponent<UnitInfo>()._unitData.hp}");
        }
    }
    #endregion

    #region # Partition() 함수 : 배열값 정렬해주는 함수
    private int Partition(Collider[] arr, int low, int high)
    {
        float pivot = arr[high].GetComponent<UnitInfo>()._unitData.hp;
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (arr[j].GetComponent<UnitInfo>()._unitData.hp <= pivot)
            {
                i++;
                Swap(arr, i, j);
            }
        }
        Swap(arr, i + 1, high);
        return i + 1;
    }
    #endregion

    #region # Swap() 함수 : 오버랩 스피어 배열 값 위치 변경해주는 함수
    private void Swap(Collider[] arr, int a, int b)
    {
        Collider temp = arr[a];
        arr[a] = arr[b];
        arr[b] = temp;
    }
    #endregion

    #region # Look_At_The_Target() : 유닛이 타겟을 감지 했을 때 타겟 쪽으로 몸을 회전하여 타겟을 바라보는 함수
    public void Look_At_The_Target(eUnit_Action_States next_Action_State = eUnit_Action_States.Default)    // 유닛이 타겟을 감지 했을 때 타겟 쪽으로 몸을 회전하여 타겟을 바라보는 함수
    {
        //float distance = Vector3.Distance(transform.position, _targetUnit.position);

        //// 거리가 공격범위보다 크면 유닛 추적
        //if (distance > unitInfoCs._unitData.attackRange)
        //{
        //    unitInfoCs._enum_Unit_Action_State = unitInfoCs._enum_Unit_Attack_State;
        //}

        //Vector3 dir = _targetUnit.position - transform.position;
        //dir.Normalize();
        //dir.y = 0;

        //Quaternion _lookRotation = Quaternion.LookRotation(dir.normalized);  // 타겟 쪽으로 바라보는 각도

        //Vector3 _euler = Quaternion.RotateTowards(transform.localRotation, _lookRotation, 600f * Time.deltaTime).eulerAngles; //800f는 회전속도

        //transform.localRotation = Quaternion.Euler(0, _euler.y, 0);

        //Quaternion _fireRotation = Quaternion.Euler(0, _lookRotation.eulerAngles.y, 0); // 유닛이 발사할 수 있는 방향의 각도
        if (next_Action_State.Equals(eUnit_Action_States.unit_Attack))
        {
            actUnitCs.Attack_Unit(next_Action_State);

        }


        //if (Quaternion.Angle(transform.localRotation, _fireRotation) <= 8f)   //각도 차이 값이 5f보다 작거나 같아졌을 때 유닛 공격
        //{
        //    _euler.y = 0;
        //    //print("용사와 몬스터의 각도 값 : " + Quaternion.Angle(transform.rotation, _fireRotation));

        //    //if (unitInfoCs._enum_Unit_Action_State != eUnit_Action_States.unit_Attack)
        //    //    return;


        //    // 1. 공격 속도 쿨타임 감소

        //    // 2. 공격속도 쿨타임이 0보다 작아졌다면 발사 후 공격속도 변수에 초기값 다시 넣어줌

        //}
        //else
        //{
        //    unitInfoCs._enum_Unit_Action_State = eUnit_Action_States.unit_Tracking;
        //}
    }
    #endregion

    #region # Look_At_The_Castle() : 몬스터가 성이랑 충돌했을 때 성을 바라보게 하는 함수
    public void Look_At_The_Castle(eUnit_Action_States next_Action_State = eUnit_Action_States.Default)    // 유닛이 타겟을 감지 했을 때 타겟 쪽으로 몸을 회전하여 타겟을 바라보는 함수
    {
        Vector3 dir = monsterUnit.castleTr.position - transform.position;
        //dir.Normalize();
        //dir.y = 0;

        Quaternion _lookRotation = Quaternion.LookRotation(dir.normalized);  // 타겟 쪽으로 바라보는 각도

        Vector3 _euler = Quaternion.RotateTowards(transform.localRotation, _lookRotation, 600f * Time.deltaTime).eulerAngles; //800f는 회전속도

        transform.localRotation = Quaternion.Euler(0, _euler.y, 0);

        Quaternion _fireRotation = Quaternion.Euler(0, _lookRotation.eulerAngles.y, 0); // 유닛이 발사할 수 있는 방향의 각도
        Debug.LogWarning("몬스터 성 공격219");
        unitInfoCs._anim.ResetTrigger("canCastleAtk");
        if (Quaternion.Angle(transform.localRotation, _fireRotation) <= 5f&& unitInfoCs._can_genSkill_Attack)   //각도 차이 값이 5f보다 작거나 같아졌을 때 유닛 공격
        {


            _euler.y = 0;
            //print("용사와 몬스터의 각도 값 : " + Quaternion.Angle(transform.rotation, _fireRotation));
            Debug.LogWarning("몬스터 성 공격226");

            if (unitInfoCs._enum_Unit_Action_State != eUnit_Action_States.unit_Attack)
                return;
            Debug.LogWarning("몬스터 성 공격228");

            unitInfoCs._anim.SetTrigger("canCastleAtk");



            //actUnitCs.Attack_Unit(next_Action_State);
            // 1. 공격 속도 쿨타임 감소

            // 2. 공격속도 쿨타임이 0보다 작아졌다면 발사 후 공격속도 변수에 초기값 다시 넣어줌

        }
    }
    #endregion
    
    public void Atk_Castle()
    {
        Debug.LogWarning("몬스터 성 공격 애니메이션 실행");
        unitInfoCs.atkSoundPlayer.volume = SoundManager.Instance.VolumeCheck(transform);

        unitInfoCs.atkSoundPlayer.PlayOneShot(unitInfoCs.use_Sfxs[0]);

        unitInfoCs._unitData._unit_Attack_CoolTime = 0f;
        Castle.Instance.Damaged_Castle(monsterUnit.castleTr);
    }

    public void Change_Atk_Anim()
    {
        unitInfoCs._anim.SetBool("canCastleAtk", false);

    }
}
