using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KainatFadeInAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float animationDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //fadeOutPage();
    }

    // Update is called once per frame
    public void fadeOutPage()
    {
        spriteRenderer.DOFade(0f, animationDuration).SetEase(Ease.InOutSine);
    }

    public void fadeInPage()
    {
        spriteRenderer.DOFade(1f, animationDuration).SetEase(Ease.InOutSine);
    }
}
