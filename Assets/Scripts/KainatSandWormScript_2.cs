using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KainatSandWormScript_2 : MonoBehaviour
{
    [SerializeField] private Vector3 hiddenPosition;
    [SerializeField] private Vector3 poppedPosition;

    [SerializeField] private float popDuration = 0.5f;
    [SerializeField] private float hideDuration = 0.5f;

    [SerializeField] private float randomIntervalMin = 2f;
    [SerializeField] private float randomIntervalMax = 5f;

    private Collider2D wormCollider;
    Transform parentTransform;

    GameObject parentObject;

    // Start is called before the first frame update
    void Start()
    {
        wormCollider = GetComponent<Collider2D>();
        wormCollider.enabled = false;   // starting out as false, will enable nce pop happens

        // Get the parent transform of the current object
        parentTransform = transform.parent;

        // Optionally, you can access the parent GameObject
        parentObject = parentTransform?.gameObject;
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
        parentTransform.DORotate(poppedPosition, popDuration).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            parentTransform.DORotate(hiddenPosition, hideDuration).SetEase(Ease.InExpo).OnComplete(() =>
            {
                wormCollider.enabled = false; // Disable collision after hiding.
            });
        });
    }
}
