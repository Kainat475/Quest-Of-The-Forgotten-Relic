using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class AreebRelicScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer; // Drag the object with SpriteRenderer here

    [Header("Chest Related components")]
    [SerializeField] private Animator chestAnimator;
    [SerializeField] private Animator bubblesAnimator;

    [Header("Relic related components")]
    float relicScaleValue = 1.78f;
    public bool chestOpened;
    [SerializeField] private Collider2D relicCollider;

    private string color1Hexa = "#99BAF1";
    private string color2Hexa = "#80ACF6";
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    [SerializeField] private float animationDuration = 1f;

    void Start()
    {
        ColorUtility.TryParseHtmlString(color1Hexa, out color1);
        ColorUtility.TryParseHtmlString(color2Hexa, out color2);

        chestOpened = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        relicCollider = GetComponent<Collider2D>();

        AnimateColor();
    }

    private void Update()
    {
        if(chestOpened)
        {            
            ScaleRelic();
            chestOpened = false;
            relicCollider.enabled = false;
        }
    }

    public void ScaleRelic()
    {
        chestOpened = true;
        chestAnimator.SetBool("open_chest", true);
        bubblesAnimator.SetTrigger("bubble_burst");
        StartCoroutine(RelicFromChest());
    }

    IEnumerator RelicFromChest()
    {
        yield return new WaitForSeconds(0.4f);
        spriteRenderer.sortingOrder = 3;
        Sequence animation_sequence = DOTween.Sequence();
        animation_sequence.Join(transform.DOScale(new Vector2(relicScaleValue, relicScaleValue), animationDuration).SetEase(Ease.InOutSine));
        animation_sequence.Join(transform.DOMoveY(transform.position.y + 1.5f, animationDuration).SetEase(Ease.InBounce));
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

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Only Tigger detected");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Shard Trigger Pulled");
            chestOpened = true;
            StartCoroutine(add2DelayThenDisappear());

            KainatUIManager.Instance.relicCollected_Areeb();
        }
    }

    IEnumerator add2DelayThenDisappear()
    {
        yield return new WaitForSeconds(4f);
        transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InOutSine);
    }
}
