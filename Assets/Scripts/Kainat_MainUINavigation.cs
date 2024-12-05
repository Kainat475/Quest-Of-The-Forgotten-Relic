using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Kainat_MainUINavigation : MonoBehaviour
{
    public GameObject splashScreen;
    public GameObject mainMenuScreen;
    public GameObject introPage;

    public Transform destinationPosition;

    public Button playButton;

    public CanvasGroup splashCanvasGroup;
    public CanvasGroup mainMenuCanvasGroup;

    public GameObject cameraObjects;
    public KainatFadeInAnimation fadeInAnimationScript;

    private void Start()
    {
        splashScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
        cameraObjects.SetActive(false);

        splashCanvasGroup = splashScreen.GetComponent<CanvasGroup>();
        mainMenuCanvasGroup = mainMenuScreen.GetComponent<CanvasGroup>();

        splashCanvasGroup.DOFade(1f, 1f).SetEase(Ease.InOutQuad);
        StartCoroutine(FadeOutAnim());
    }

    IEnumerator FadeOutAnim()
    {
        yield return new WaitForSeconds(6f);
        splashCanvasGroup.DOFade(0f, 1f).SetEase(Ease.InOutQuad);

        cameraObjects.SetActive(true);
        fadeInAnimationScript.fadeOutPage();

        mainMenuScreen.SetActive(true) ;
        mainMenuCanvasGroup.DOFade(1f, 1f).SetEase(Ease.InOutQuad);
    }

    public void startButtonClicked()
    {
        // Erasing Menu Page
        mainMenuScreen.SetActive(false) ;
        introPage.transform.DOMoveY(destinationPosition.transform.position.y, 20f).SetEase(Ease.Linear).OnComplete(() =>
        {
            StartLevel1();
        });
    }

    private void StartLevel1()
    {
        fadeInAnimationScript.fadeInPage();
        StartCoroutine(addDelayBeforeLevel());
    }

    IEnumerator addDelayBeforeLevel()
    {
        yield return new WaitForSeconds(2f);
        // Loading Level 2 for now
        SceneManager.LoadScene(4);
    }
}
