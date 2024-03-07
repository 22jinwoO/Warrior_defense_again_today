using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField]
    private Button startBtn;

    [SerializeField]
    private RectTransform leftUnitImg;

    [SerializeField]
    private RectTransform rightUnitImg;

    private void Awake()
    {
        startBtn.onClick.AddListener(GotoRobbyScene);

        StartCoroutine(Anim_LeftUnit());
        StartCoroutine(Anim_RightUnit());
    }

    // Update is called once per frame
    void Update()
    {






    }

    private IEnumerator Anim_LeftUnit()
    {
        while (leftUnitImg.anchoredPosition.x < -670)
        {
            leftUnitImg.anchoredPosition += new Vector2(20f, 0);

            yield return new WaitForSecondsRealtime(0.012f);
        }
    }

    private IEnumerator Anim_RightUnit()
    {
        while (rightUnitImg.anchoredPosition.x > 630)
        {
            rightUnitImg.anchoredPosition += new Vector2(-20f, 0);

            yield return new WaitForSecondsRealtime(0.010f);

        }
    }


    private void GotoRobbyScene()
    {
        SceneManager.LoadScene("2.LobbyScene");
    }
}
