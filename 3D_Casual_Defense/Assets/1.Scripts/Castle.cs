using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class Castle : Singleton<Castle>
{
    public Transform _castle_Pos;

    public Transform[] caslteModels;
    //
    //
    public float _castle_maxHp;

    public float _castle_Hp;

    [SerializeField]


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
    private GameObject[] downVfxs;

    [SerializeField]
    private GameObject[] downSmokes;

    public float halfColiderValue;

    //델리게이트 선언
    public delegate void CastleDownHandler();

    //이벤트 선언
    public event CastleDownHandler OnCastleDown;


    private void Awake()
    {
        _castle_maxHp= 100f;
        _castle_Hp = 100f;


        isDown = false;
        halfColiderValue = 1.4f;
        _source = GetComponent<AudioSource>();

        txtManager = Stage1_TextManager.Instance;


        txtManager.ShowCastleHpTxt();
    }
    private void Update()
    {
        if (_castle_Hp<=0f&&!isDown)
        {
            StartCoroutine(DownCaslte());
            isDown = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
            StartCoroutine(DownCaslte());
    }

    // 성 내구도 0 이 됐을 때 무너지는 연출 구현한 함수
    private IEnumerator DownCaslte()
    {
        //이벤트 호출
        OnCastleDown();

        // 불타는 이펙트 활성화
        for(int i=0; i<downVfxs.Length;i++)
        {
            downVfxs[i].SetActive(true);
        }

        // 무너질 때 연기 이펙트 활성화
        for (int i = 0; i < downSmokes.Length; i++)
        {
            downSmokes[i].SetActive(true);
        }

        // 사운드 실행
        _source.PlayOneShot(_downCaslteClip);

        // 성 오브젝트 흔들리면서 기준 y값까지 y값 낮추기
        while (transform.localPosition.y>=-6f)
        {
            UnityEngine.Debug.LogWarning("성다운");

            StartCoroutine(HitCastle(0.3f,transform));
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);

            yield return new WaitForSecondsRealtime(0.1f);
        }

        for (int i = 0; i < downVfxs.Length; i++)
        {
            downVfxs[i].SetActive(false);
        }
        
        for (int i = 0; i < downSmokes.Length; i++)
        {
            downSmokes[i].SetActive(false);
        }

        //플레이어 패배시 팝업창 활성화
        uiManager.DownCastlePopUp();
    }

    public void Damaged_Castle(Transform castleTr)
    {
        //_anim.SetBool("isHit", true);
        _source.pitch = Random.Range(0.6f, 1.1f);
        // 카메라 거리에 따른 히트 사운드 볼륨 값 조절
        _source.volume=SoundManager.Instance.VolumeCheck(castleTr);

        // 히트사운드 실행
        _source.PlayOneShot(_hitCaslteClip);

        // 피격 연출 실행
        StartCoroutine(HitCastle(0.1f, castleTr));

        // 피격시 성 내구도 -1
        _castle_Hp -= 1;

        // 피격 시 성 내구도 텍스트 반영
        txtManager.ShowCastleHpTxt();
    }

    // 성 피격 연출 함수
    private IEnumerator HitCastle(float shakeValue,Transform castleNum)
    {
        UnityEngine.Debug.LogWarning("성흔ㄷ르림");
        castleNum.position = new Vector3(castleNum.position.x - shakeValue, castleNum.position.y, castleNum.position.z);
        yield return new WaitForSecondsRealtime(0.03f);
        yield return null;

        castleNum.position = new Vector3(castleNum.position.x + shakeValue, castleNum.position.y, castleNum.position.z);
        yield return new WaitForSecondsRealtime(0.03f);
        yield return null;

        castleNum.position = new Vector3(castleNum.position.x, castleNum.position.y, castleNum.position.z - shakeValue);
        yield return new WaitForSecondsRealtime(0.03f);

        yield return null;

        castleNum.position = new Vector3( castleNum.position.x, castleNum.position.y, castleNum.position.z + shakeValue);
        yield return new WaitForSecondsRealtime(0.03f);

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
