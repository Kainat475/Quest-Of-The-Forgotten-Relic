using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;
using static DG.DemiLib.DeToggleColors;

public class KainatUIManager : MonoBehaviour
{
    public static KainatUIManager Instance;

    [Header("Fade Animation Screens")]
    [SerializeField] private GameObject FadePanel;
    [SerializeField] private Image fadeImage;

    [Header("Pause Screen")]
    [SerializeField] private GameObject pausePanel;

    [Header("Show Damage Taken Points")]
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private Vector2 damageTextScale;

    [Header("Danger Screen Components")]
    [SerializeField] private TextMeshProUGUI relicActivatedText;
    [SerializeField] private GameObject relicImage;
    [SerializeField] private GameObject dangerPanel;
    [SerializeField] private Image dangerPanelImage;

    [Header("Level End Screen Components")]
    [SerializeField] GameObject LevelEndPanel;
    [SerializeField] GameObject textBox1;
    [SerializeField] GameObject textBox2;
    [SerializeField] GameObject textBox3;
    [SerializeField] GameObject textBox4;

    [Header("Audio Related Components")]
    public AudioSource audioSource;
    public AudioClip hurtAudio;
    public AudioClip powerUpAudio;
    public AudioClip jumpAudio;
    public AudioClip deathAudio;
    public AudioClip walkAudio;    

    // This is called when the script is loaded or a value is changed in the editor
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;            
        }
        else
        {
            // Destroy this instance if one already exists (this ensures there is only one)
            Destroy(gameObject);
        }
    }

    public static void DestroySingleton()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }
    }

    void Start()
    {
        FadePanel.SetActive(true);
        LevelEndPanel.SetActive(false);
        pausePanel.SetActive(false);

        Debug.Log("Start called.");
        fadeImage = FadePanel.GetComponent<Image>();
        fadeImage.DOFade(0f, 1f).SetEase(Ease.InOutSine);
        Debug.Log(fadeImage.color.a);
        //FadePanel.SetActive(false);
        StartCoroutine(addDelayThenDisappear(1, FadePanel));
        Debug.Log(fadeImage.color.a);

        damageText.text = "";
        damageTextScale = damageText.transform.localScale;

        dangerPanelImage = dangerPanel.GetComponent<Image>();
        //relicCollected();
        //AnimateWarning();
    }

    public void showDamageText(float damage)
    {
        damageText.text = damage.ToString();
        //damageText.DOFade(0, 0.2f).SetEase(Ease.InQuad);

        // to play more than one animation together
        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.Join(damageText.transform.DOScale(Vector3.zero, 1.25f).SetEase(Ease.InOutQuad));

        Color originalColor = damageText.color;
        sequence.Join(DOTween.To( () => originalColor.a, alpha => 
        damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha), 0f, 1.25f));

        sequence.OnComplete(() => resetDamageTextProperties(originalColor)); 
    }

    private void resetDamageTextProperties(Color originalColor)
    {
        // resetting text properly
        damageText.text = "";

        // resetting color properly
        originalColor.a = 1f;
        damageText.color = originalColor;

        // resetting scale properly
        damageText.transform.localScale = damageTextScale;
    }

    public void relicCollected()
    {
        relicImage.SetActive(true);

        // meaning display the boots powerup and enable dangerPanel
        dangerPanel.SetActive(true);

        dangerPanelImage.DOFade(0.40f, 1f).From(0f)
                .SetEase(Ease.InQuad);
    }

    public void relicCollected_Areeb()
    {
        relicImage.SetActive(true);
    }

    public void dangerAverted()
    {
        dangerPanelImage.DOFade(0f, 1f).From(0.40f)
                .SetEase(Ease.InQuad);
        //dangerPanel.SetActive(false);
        StartCoroutine(addDelayThenDisappear(1.2f, dangerPanel));
        // Call the camera shakeStop functin after this to stop earthquake
    }

    public IEnumerator addDelayThenDisappear(float delay, GameObject toDisappear = null)
    {
        yield return new WaitForSeconds(delay);
        if (toDisappear != null)
        {
            toDisappear.SetActive(false);
        }
    }

    private void AnimateWarning()
    {
        // Loop between two colors indefinitely
        relicActivatedText.DOColor(Color.red, 0.35f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                relicActivatedText.DOColor(Color.black, 0.35f)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(AnimateWarning);
            });
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void resumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;        
    }

    public void returnToMainMenu()
    {
        Debug.Log("Main menu called");
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        //FadePanel.SetActive(true);
        //fadeImage.DOFade(1f, 1f).SetEase(Ease.InOutSine);
        //StartCoroutine(addDelayBeforeRestartingLevel(0));
    }

    public void restartLevel()
    {
        Debug.Log("restart called");
        Time.timeScale = 1;
        //FadePanel.SetActive(true);
        //fadeImage.DOFade(1f, 1f).SetEase(Ease.InOutSine);
        int sceneNum = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneNum);
        //StartCoroutine(addDelayBeforeRestartingLevel(sceneNum));
    }

    public void level2Ended()
    {
        FadePanel.SetActive(true);
        fadeImage.DOFade(1f, 1f).SetEase(Ease.InOutSine);
        StartCoroutine(displayDialogueBoxes());
        //LevelEndPanel.SetActive(true);
        //fadeImage.DOFade(0f, 1f).SetEase(Ease.InOutSine);
        //StartCoroutine(addDelayThenDisappear(1f));  
    }

    public void level3Ended()
    {
        FadePanel.SetActive(true);
        fadeImage.DOFade(1f, 1f).SetEase(Ease.InOutSine);
        StartCoroutine(displayDialogueBoxes());  
    }

    public void level4Ended()
    {
        FadePanel.SetActive(true);
        fadeImage.DOFade(1f, 1f).SetEase(Ease.InOutSine);
        SceneManager.LoadScene(3);
    }

    IEnumerator addDelayBeforeRestartingLevel(int levelID)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelID);
    }

    IEnumerator displayDialogueBoxes()
    {
        yield return new WaitForSeconds(1F);
        LevelEndPanel.SetActive(true);
        fadeImage.DOFade(0f, 1f).SetEase(Ease.InOutSine);

        textBox1.SetActive(true);
        yield return new WaitForSeconds(2F);
        textBox1.SetActive(false);
        yield return new WaitForSeconds(0.5F);

        //StartCoroutine(displayDialogueBox(textBox2, 2f, 0.5f));
        textBox2.SetActive(true);
        yield return new WaitForSeconds(2F);
        textBox2.SetActive(false);
        yield return new WaitForSeconds(0.5F);

        //StartCoroutine(displayDialogueBox(textBox3, 2f, 0.5f));
        textBox3.SetActive(true);
        yield return new WaitForSeconds(2F);
        textBox3.SetActive(false);
        yield return new WaitForSeconds(0.5F);

        //StartCoroutine(displayDialogueBox(textBox4, 2f, 2f));
        textBox4.SetActive(true);
        yield return new WaitForSeconds(2F);
        textBox4.SetActive(false);
        yield return new WaitForSeconds(2F);

        fadeImage.DOFade(1f, 1f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(2F);
        SceneManager.LoadScene(5);
    }

    // ----------------------------------------------------------------------- //
    //                              Audio Functions                            //
    // ----------------------------------------------------------------------- //

    public void playWalkAudio()
    {
        audioSource.PlayOneShot(walkAudio);
    }

    public void playDeathAudio()
    {
        audioSource.PlayOneShot(deathAudio);
    }

    public void playHurtAudio()
    {
        audioSource.PlayOneShot(hurtAudio);
    }

    public void playJumpAudio()
    {
        audioSource.PlayOneShot(jumpAudio);
    }
    public void playPowerUpAudio()
    {
        audioSource.PlayOneShot(powerUpAudio);
    }
}
