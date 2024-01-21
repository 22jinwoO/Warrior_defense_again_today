using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField]
    private Button startBtn;
    private void Awake()
    {
        startBtn.onClick.AddListener(GotoRobbyScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GotoRobbyScene()
    {
        SceneManager.LoadScene("2.LobbyScene");
    }
}
