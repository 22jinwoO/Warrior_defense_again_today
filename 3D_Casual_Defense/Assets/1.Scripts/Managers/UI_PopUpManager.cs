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

    private void Awake()
    {
        // 로비로 가는 버튼 함수 연결
        goLobbyBtn.onClick.AddListener(GoLobbyBtn);

        // 홈으로 이동하는 함수 연결
        retryBtn.onClick.AddListener(GoRetryBtn);
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
