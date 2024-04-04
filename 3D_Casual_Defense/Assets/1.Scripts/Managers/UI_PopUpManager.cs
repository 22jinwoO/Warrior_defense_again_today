using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_PopUpManager : MonoBehaviour
{
    [Header("플레이어 스크립트")]
    public Player _playerCs;

    [Header("팝업창 게임오브젝트")]
    [SerializeField]
    private GameObject popUp_Message;

    [Header("로비로 가기 버튼")]
    [SerializeField]
    private Button goLobbyBtn;

    [Header("다시하기 버튼")]
    [SerializeField]
    private Button retryBtn;

    [Header("스테이지 1 텍스트 매니저 스크립트")]
    [SerializeField]
    private Stage1_TextManager txtManager;

    [Header("유닛 상점 팝업창 활성화 버튼")]
    [SerializeField]
    private Button usePopUpBtn;

    [Header("유닛 상점 팝업창 비활성화 버튼")]
    [SerializeField]
    private Button usePopDownBtn;

    [Header("유닛 상점 활성화 상태인지 비활성화 상태인지 확인하는 변수")]
    [SerializeField]
    public bool isUseShop;

    [Header("유닛 상점 렉트 트랜스폼 변수")]
    [SerializeField]
    private RectTransform buyUnitPopUp;

    [Header("캐슬 HpBar 이미지")]
    public Image castleHpBar;

    [Header("몬스터 스폰 매니저 스크립트")]
    [SerializeField]
    private MonsterSpawnManager monsterSpawnManager;

    [Header("스폰된 몬스터 모두 사망하게 하는 버튼")]
    [SerializeField]
    private Button allDeadMonsterBtn;

    [Header("웨이브 대기 시간 스킵 버튼")]
    [SerializeField]
    private Button skipWaveTimeBtn;

    private void Awake()
    {
        isUseShop = true;

        // 캐슬 Hp 반영하는 텍스트
        txtManager.ShowCastleHpTxt();

        // 로비로 가는 버튼 함수 연결
        goLobbyBtn.onClick.AddListener(GoLobbyBtn);

        // 홈으로 이동하는 함수 연결
        retryBtn.onClick.AddListener(GoRetryBtn);

        // 유닛 상점 활성화/ 비활성화 버튼 함수 연결
        usePopUpBtn.onClick.AddListener(()=>StartCoroutine(UseUnitPopUp()));
        usePopDownBtn.onClick.AddListener(()=>StartCoroutine(UseUnitPopDown()));

        // 모든 몬스터 사망시키는 버튼 함수 연결
        allDeadMonsterBtn.onClick.AddListener(monsterSpawnManager.DeadAllMonster);

        // 웨이브 대기시간 스킵하는 버튼 함수 연결
        skipWaveTimeBtn.onClick.AddListener(monsterSpawnManager.SkipWaveStartTime);
    }

    //홈버튼에서 호출되는 함수
    public void GoLobbyBtn()
    {
        SceneManager.LoadScene("2.LobbyScene");
    }

    //다시하기 버튼에서 호출되는 함수
    public void GoRetryBtn()
    {
        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(scene.name);
    }

    #region # UseUnitPopUp() 함수 : 플레이어 유닛 구매 상점 팝업창 활성화 하는 함수
    public IEnumerator UseUnitPopUp()
    {
        if (_playerCs.canPlay)
        {
            usePopUpBtn.gameObject.SetActive(false);
            yield return null;

            usePopDownBtn.gameObject.SetActive(true);

            // 팝업창이 내려가있다면 렉트트랜스폼 y 값 증가
            if (isUseShop)
            {
                while (buyUnitPopUp.anchoredPosition.y < 0f)
                {
                    buyUnitPopUp.anchoredPosition += new Vector2(0, +10f);
                    yield return null;
                }
                isUseShop = false;
                yield break;
            }

        }
    }
    #endregion

    #region # UseUnitPopDown() 함수 : 플레이어 유닛 구매 상점 팝업창 비활성화 하는 함수
    public IEnumerator UseUnitPopDown()
    {
        usePopDownBtn.gameObject.SetActive(false);
        yield return null;

        usePopUpBtn.gameObject.SetActive(true);

        // 팝업창이 올라가있다면 렉트트랜스폼 y 값 감소
        if (!isUseShop)
        {
            while (buyUnitPopUp.anchoredPosition.y > -100f)
            {
                buyUnitPopUp.anchoredPosition += new Vector2(0, -10f);
                yield return null;
            }
            isUseShop = true;
            yield break;

        }
    }
    #endregion

    #region # PlayerWinPopUp() 함수 : 플레이어 승리 시 팝업창 활성화 해주는 함수
    public void PlayerWinPopUp()
    {
        txtManager.PlayerWinTxt();
        SetActive_Message(popUp_Message, true);
    }
    #endregion

    #region # DownCastlePopUp() 함수 : 플레이어 패배 시 팝업창 활성화 해주는 함수
    public void DownCastlePopUp()
    {
        txtManager.PlayerLoseTxt();
        SetActive_Message(popUp_Message,true);
    }
    #endregion

    #region # SetActive_Message() 함수 : 이미지 활성화 및 비활성화 해주는 함수
    // 이미지 활성화 및 비활성화 해주는 함수
    private void SetActive_Message(GameObject Image, bool Active)
    {
        Image.SetActive(Active);
    }
    #endregion
}
