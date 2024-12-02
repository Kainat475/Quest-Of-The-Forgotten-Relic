using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class KainatRelicScript : MonoBehaviour
{
    [SerializeField] private Animator relicAnimator;
    [SerializeField] private SpriteRenderer spriteRenderer; // Drag the object with SpriteRenderer here

    [Header("Cinemachine Settings")]
    [SerializeField] private KainatEarthQuakeScript vCam_cameraShake_script;

    [Header("SafeZoneTrigger Settings")]
    [SerializeField] private Collider2D safeZoneTrigger;

    private string color1Hexa = "#F9DED3";
    private string color2Hexa = "#F9A989";
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    [SerializeField] private float animationDuration = 1f;

    void Start()
    {
        ColorUtility.TryParseHtmlString(color1Hexa, out color1);
        ColorUtility.TryParseHtmlString(color2Hexa, out color2);

        relicAnimator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            AnimateColor();
        }
    }

    private void AnimateColor()
    {
        // Loop between two colors indefinitely
        spriteRenderer.DOColor(color2, animationDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                spriteRenderer.DOColor(color1, animationDuration)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(AnimateColor);
            });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            relicAnimator.SetBool("relicPickedUp", true);
            StartCoroutine(KainatUIManager.Instance.addDelayThenDisappear(2f));

            transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InOutSine);
            StartCoroutine(KainatUIManager.Instance.addDelayThenDisappear(1f, gameObject));

            KainatUIManager.Instance.relicCollected();
            vCam_cameraShake_script.StartShake(10f);     // Starting Shaking once the relic is collected
            safeZoneTrigger.enabled = true;
        }
    }
}
