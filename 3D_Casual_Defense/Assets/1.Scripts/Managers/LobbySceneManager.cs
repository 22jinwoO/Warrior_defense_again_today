using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbySceneManager : MonoBehaviour
{
    [Header("스테이지 1 버튼")]
    [SerializeField]
    private Button goStage1_Btn;

    private void Awake()
    {
        goStage1_Btn.onClick.AddListener(GotoStage1);
    }

    // 스테이지 1 씬 불러오는 함수
    private void GotoStage1()
    {
        SceneManager.LoadScene("3.Stage1_Scene");
    }
}
