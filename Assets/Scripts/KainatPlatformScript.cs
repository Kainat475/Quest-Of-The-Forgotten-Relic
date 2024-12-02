using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KainatPlatformScript : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private Rigidbody2D rb_platform;
    [SerializeField] private Collider2D collider_platform;
    [SerializeField] private Vector3 platformScale;
    [SerializeField] private Animator platformAnimator;

    [SerializeField] private float currentTime;
    [SerializeField] private float totalTime = 3f;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private float collapseDelay = 1f;
    [SerializeField] public bool playerOnPlatform;

    [SerializeField] public bool isCollapsing;

    void Start()
    {
        platformScale = transform.localScale;
        rb_platform = GetComponent<Rigidbody2D>();
        collider_platform = GetComponent<Collider2D>();

        // So it doesn't fall down
        rb_platform.bodyType = RigidbodyType2D.Kinematic;
        originalPosition = transform.position;
        playerOnPlatform = false;
        isCollapsing = false;
    }

    void Update()
    {
        if (playerOnPlatform)
            currentTime += Time.deltaTime;
        else
            currentTime = 0;

        if (currentTime >= totalTime && playerOnPlatform && !isCollapsing)
        {
            StartCoroutine(collapsePlatform());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCollapsing)
        {
            platformAnimator.SetBool("Shake", true);
            playerOnPlatform = true;  
        }
              
    }

    private IEnumerator collapsePlatform()
    {
        isCollapsing = true;

        // Enabling physics so the platform falls
        collider_platform.enabled = false;
        rb_platform.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(collapseDelay);

        ResetPlatform();
    }

    private void ResetPlatform()
    {
        transform.localScale = Vector3.zero;
        rb_platform.velocity = Vector2.zero;
        transform.position = originalPosition;
        transform.DOScale(platformScale, 0.75f).SetEase(Ease.InQuad);

        collider_platform.enabled = true;
        rb_platform.bodyType = RigidbodyType2D.Kinematic;
        platformAnimator.ResetTrigger("Shake");

        playerOnPlatform = false;
        platformAnimator.SetBool("Shake", false);

        currentTime = 0.01f;
        isCollapsing = false;
    }
}
