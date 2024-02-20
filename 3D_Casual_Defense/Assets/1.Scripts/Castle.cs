using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Castle : Singleton<Castle>
{
    public Transform _castle_Pos;
    [SerializeField]
    private Transform caslteModel;
    public float _castle_Hp;
    public Animator _anim;

    [SerializeField]
    private BoxCollider _boxCollider;

    [SerializeField]
    private AudioSource _source;

    [SerializeField]
    private AudioClip _hitCaslteClip;

    [SerializeField]
    private AudioClip _downCaslteClip;


    [SerializeField]
    private bool isDown;


    [SerializeField]
    private UI_PopUpManager uiManager;


    [SerializeField]
    private Stage1_TextManager txtManager;

    [SerializeField]
    private GameObject downVfxs;


    //델리게이트 선언
    public delegate void CastleDownHandler();

    //이벤트 선언
    public event CastleDownHandler OnCastleDown;


    private void Awake()
    {
        isDown = false;
        _boxCollider = GetComponent<BoxCollider>();
        //_anim = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
        //_hitCaslteClip = _source.GetComponent<AudioClip>();
        caslteModel.position = new Vector3(caslteModel.position.x - 0.1f, caslteModel.position.y, caslteModel.position.z);

        txtManager = Stage1_TextManager.Instance;

        _castle_Hp = 100f;

        txtManager.ShowCastleHpTxt();
    }
    private void Update()
    {
        if (_castle_Hp<=0f&&!isDown)
        {
            StartCoroutine(DownCaslte());
            isDown = true;
        }
    }

    // 성 내구도 0 이 됐을 때 무너지는 연출 구현한 함수
    private IEnumerator DownCaslte()
    {
        //이벤트 호출
        OnCastleDown();

        downVfxs.SetActive(true);
        _boxCollider.enabled = false;
        _source.PlayOneShot(_downCaslteClip);

        yield return new WaitForSecondsRealtime(8f);

        while (transform.position.y>=-12f)
        {
            UnityEngine.Debug.LogWarning("성다운");

            StartCoroutine(HitCastle(0.3f));
            transform.position = new Vector3(transform.position.x, transform.position.y- 0.1f, transform.position.z);
            yield return new WaitForSecondsRealtime(0.1f);
        }

        //플레이어 패배시 팝업창 활성화
        uiManager.DownCastlePopUp();
        downVfxs.SetActive(false);

    }

    public void Damaged_Castle()
    {
        //_anim.SetBool("isHit", true);
        _source.pitch = Random.Range(0.6f, 1.1f);
        // 카메라 거리에 따른 히트 사운드 볼륨 값 조절
        _source.volume=SoundManager.Instance.VolumeCheck(transform);

        // 히트사운드 실행
        _source.PlayOneShot(_hitCaslteClip);

        // 피격 연출 실행
        StartCoroutine(HitCastle(0.1f));

        // 피격시 성 내구도 -1
        _castle_Hp -= 1;

        // 피격 시 성 내구도 텍스트 반영
        txtManager.ShowCastleHpTxt();
    }

    // 성 피격 연출 함수
    private IEnumerator HitCastle(float shakeValue)
    {
        UnityEngine.Debug.LogWarning("성흔ㄷ르림");
        caslteModel.position = new Vector3(caslteModel.position.x - shakeValue, caslteModel.position.y, caslteModel.position.z);
        yield return new WaitForSecondsRealtime(0.03f);
        caslteModel.position = transform.position;
        yield return null;

        caslteModel.position = new Vector3(caslteModel.position.x + shakeValue, caslteModel.position.y, caslteModel.position.z);
        yield return new WaitForSecondsRealtime(0.03f);
        caslteModel.position = transform.position;
        yield return null;

        caslteModel.position = new Vector3(caslteModel.position.x, caslteModel.position.y, caslteModel.position.z - shakeValue);
        yield return new WaitForSecondsRealtime(0.03f);

        caslteModel.position = transform.position;
        yield return null;

        caslteModel.position = new Vector3( caslteModel.position.x, caslteModel.position.y, caslteModel.position.z + shakeValue);
        yield return new WaitForSecondsRealtime(0.03f);

        caslteModel.position = transform.position;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 성이 몬스터의 상태를 변경해주는게 아니라 몬스터가 성이랑 충돌했을 때 상태가 변경되도록 구현하기
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<UnitInfo>()._anim.SetBool("isMove", false);

            other.GetComponent<UnitInfo>()._enum_Unit_Action_Mode = eUnit_Action_States.monster_AttackCastlePhase;
            other.GetComponent<UnitInfo>()._enum_Unit_Action_State = eUnit_Action_States.unit_Attack;
        }
    }
}
