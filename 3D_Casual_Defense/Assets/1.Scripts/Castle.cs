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

    private void Awake()
    {
        isDown = false;
        _boxCollider = GetComponent<BoxCollider>();
        //_anim = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
        //_hitCaslteClip = _source.GetComponent<AudioClip>();
        caslteModel.position = new Vector3(caslteModel.position.x - 0.1f, caslteModel.position.y, caslteModel.position.z);
        
        _castle_Hp = 100f;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _anim.SetBool("isHit", true);
        }

        if (_castle_Hp<=0f&&!isDown)
        {
            StartCoroutine(DownCaslte());
            isDown = true;
        }
    }

    private IEnumerator DownCaslte()
    {
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
    }

    public void Damaged_Castle()
    {
        //_anim.SetBool("isHit", true);

        _source.volume=SoundManager.Instance.VolumeCheck(transform);
        _source.PlayOneShot(_hitCaslteClip);
        StartCoroutine(HitCastle(0.1f));
        _castle_Hp -= 1;
    }


    private IEnumerator HitCastle(float shakeValue)
    {
        UnityEngine.Debug.LogWarning("성흔ㄷ르림");
        caslteModel.position = new Vector3(caslteModel.position.x - shakeValue, caslteModel.position.y, caslteModel.position.z);
        yield return new WaitForSecondsRealtime(0.1f);
        caslteModel.position = transform.position;
        yield return null;

        caslteModel.position = new Vector3(caslteModel.position.x + shakeValue, caslteModel.position.y, caslteModel.position.z);
        yield return new WaitForSecondsRealtime(0.1f);
        caslteModel.position = transform.position;
        yield return null;

        caslteModel.position = new Vector3(caslteModel.position.x, caslteModel.position.y, caslteModel.position.z - shakeValue);
        yield return new WaitForSecondsRealtime(0.1f);

        caslteModel.position = transform.position;
        yield return null;

        caslteModel.position = new Vector3( caslteModel.position.x, caslteModel.position.y, caslteModel.position.z + shakeValue);
        yield return new WaitForSecondsRealtime(0.1f);

        caslteModel.position = transform.position;
        yield return null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<UnitInfo>()._anim.SetBool("isMove", false);

            other.GetComponent<UnitInfo>()._enum_Unit_Action_Mode = eUnit_Action_States.monster_AttackCastlePhase;
            other.GetComponent<UnitInfo>()._enum_Unit_Action_State = eUnit_Action_States.unit_Attack;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UnityEngine.Debug.LogWarning("혹시되나");
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<UnitInfo>()._anim.SetBool("isMove", false);

            other.GetComponent<UnitInfo>()._enum_Unit_Action_Mode = eUnit_Action_States.monster_AngryPhase;
            other.GetComponent<UnitInfo>()._enum_Unit_Action_State = eUnit_Action_States.unit_Idle;
        }
    }
}
