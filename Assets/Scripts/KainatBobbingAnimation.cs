using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KainatBobbingAnimation : MonoBehaviour
{
    [Header("Bobbing Settings")]
    public float bobbingDistance = 0.5f; // How far the object moves up and down
    public float bobbingDuration = 1f;   // Time for one bobbing cycle
    public Ease bobbingEase = Ease.InOutSine; // Easing for smooth movement

    private void Start()
    {
        PlayBobbingAnimation();
    }

    private void PlayBobbingAnimation()
    {
        // Animate the position of the object in a bobbing motion
        transform.DOMoveY(transform.position.y + bobbingDistance, bobbingDuration)
            .SetEase(bobbingEase) // Smooth transition
            .SetLoops(-1, LoopType.Yoyo); // Loop infinitely in a Yoyo (up-down) motion
    }
}
