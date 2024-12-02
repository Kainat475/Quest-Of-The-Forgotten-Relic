using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KainatSandWormScript_1 : MonoBehaviour
{
    [SerializeField] private Vector3 hiddenPosition;
    [SerializeField] private Vector3 poppedPosition;

    [SerializeField] private float popDuration = 0.5f;
    [SerializeField] private float hideDuration = 0.5f;

    [SerializeField] private float randomIntervalMin = 2f;
    [SerializeField] private float randomIntervalMax = 5f;

    private Collider2D wormCollider;

    // Start is called before the first frame update
    void Start()
    {
        wormCollider = GetComponent<Collider2D>();
        wormCollider.enabled = false;   // starting out as false, will enable nce pop happens

        StartCoroutine(popWorm());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator popWorm()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(randomIntervalMin, randomIntervalMax));
            startPopping();
            yield return new WaitForSeconds(hideDuration + popDuration);
        }
    }

    private void startPopping()
    {
        wormCollider.enabled = true;
        transform.DORotate(poppedPosition, popDuration).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            transform.DORotate(hiddenPosition, hideDuration).SetEase(Ease.InExpo).OnComplete(() =>
            {
                wormCollider.enabled = false; // Disable collision after hiding.
            });
        });
    }
}
