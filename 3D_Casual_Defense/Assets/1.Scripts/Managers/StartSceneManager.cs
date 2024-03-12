using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField]
    private Button startBtn;

    [SerializeField]
    private Image startBtnImg;

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private TextMeshProUGUI titleBackText;


    [SerializeField]
    private Image fadeInImg;


    [SerializeField]
    private RectTransform leftUnitImg;

    [SerializeField]
    private RectTransform rightUnitImg;

    [SerializeField]
    private bool isEnd;

    private void Awake()
    {
        startBtnImg=startBtn.GetComponent<Image>();
        startBtn.onClick.AddListener(GotoRobbyScene);
        StartCoroutine(FadeOut());

    }

    private void Update()
    {
        if (isEnd)
        {
            StartCoroutine(BtnFadeIn());
            isEnd = false;
        }
    }
    private IEnumerator FadeOut()
    {
        Color color = fadeInImg.color;
        while (color.a>0f)
        {
            print("페이드인 체크");
            color.a -= 0.1f;
            fadeInImg.color = color;
            yield return new WaitForSecondsRealtime(0.07f);
        }
        fadeInImg.gameObject.SetActive(false);
        StartCoroutine(Anim_LeftUnit());
        StartCoroutine(Anim_RightUnit());



    }

    private IEnumerator BtnFadeIn()
    {
        Color color2 = startBtnImg.color;
        Color color3 = titleText.color;
        Color color4 = titleBackText.color;

        while (color2.a < 1f)
        {
            print("페이드인 체크2");
            color2.a += 0.1f;
            color3.a += 0.1f;
            color4.a += 0.1f;
            startBtnImg.color = color2;
            titleText.color = color3;
            titleBackText.color = color4;
            yield return new WaitForSecondsRealtime(0.07f);
        }
        startBtn.interactable = true;
    }

    private IEnumerator Anim_LeftUnit()
    {
        while (leftUnitImg.anchoredPosition.x < -476)
        {
            leftUnitImg.anchoredPosition += new Vector2(20f, 0);

            yield return new WaitForSecondsRealtime(0.012f);
        }
    }

    private IEnumerator Anim_RightUnit()
    {
        while (rightUnitImg.anchoredPosition.x > 496)
        {
            rightUnitImg.anchoredPosition += new Vector2(-20f, 0);

            yield return new WaitForSecondsRealtime(0.010f);

        }
        isEnd = true;
    }


    private void GotoRobbyScene()
    {
        SceneManager.LoadScene("2.LobbyScene");
    }
}
