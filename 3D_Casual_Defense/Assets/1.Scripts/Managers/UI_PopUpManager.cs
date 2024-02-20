using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_PopUpManager : MonoBehaviour
{
    [SerializeField]
    private GameObject popUp_Message;

    [SerializeField]
    private Button goLobbyBtn;

    [SerializeField]
    private Button retryBtn;

    [SerializeField]
    private Stage1_TextManager txtManager;

    [SerializeField]
    private Button usePopUpBtn;

    [SerializeField]
    public bool isUseShop;

    [SerializeField]
    private RectTransform buyUnitPopUp;

    private void Awake()
    {
        isUseShop = true;

        // 로비로 가는 버튼 함수 연결
        goLobbyBtn.onClick.AddListener(GoLobbyBtn);

        // 홈으로 이동하는 함수 연결
        retryBtn.onClick.AddListener(GoRetryBtn);

        // 홈으로 이동하는 함수 연결
        usePopUpBtn.onClick.AddListener(()=>StartCoroutine(UseUnitPopUp()));
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

    public IEnumerator UseUnitPopUp()
    {
        if (isUseShop)
        {
            while (buyUnitPopUp.anchoredPosition.y < 0f)
            {
                buyUnitPopUp.anchoredPosition += new Vector2(0, +10f);
                print(buyUnitPopUp.anchoredPosition.y);

                yield return null;
            }
            isUseShop = false;
            yield break;
        }

        if (!isUseShop)
        {
            while (buyUnitPopUp.anchoredPosition.y > -100f)
            {
                buyUnitPopUp.anchoredPosition += new Vector2(0, -10f);
                print(buyUnitPopUp.anchoredPosition.y);
                yield return null;
            }
            isUseShop = true;
            yield break;

        }
    }

    public void PlayerWinPopUp()
    {
        txtManager.PlayerWinTxt();
        SetActive_Message(popUp_Message, true);
    }

    public void DownCastlePopUp()
    {
        txtManager.PlayerLoseTxt();
        SetActive_Message(popUp_Message,true);
    }

    // 이미지 활성화 및 비활성화 해주는 함수
    private void SetActive_Message(GameObject Image, bool Active)
    {
        Image.SetActive(Active);
    }
}
