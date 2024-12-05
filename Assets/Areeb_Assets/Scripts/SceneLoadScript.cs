using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadScript : MonoBehaviour
{
    // Will Keep All Scripts disabled for 2 seconds, fade in, enable scripts
    // after relic collected, disable scripts again, fade out, next level load

    [SerializeField] private GameObject[] iceSpikeObjects;
    [SerializeField] private GameObject[] batObjects;

    [SerializeField] private GameObject playerRef;
    PlayerScript playerScriptRef;

    private void Start()
    {
        playerScriptRef = playerRef.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {        
        if (playerScriptRef.enabled == false)
        {
            for (int i = 0; i < iceSpikeObjects.Length; i++)
            {
                iceSpikeObjects[i].SetActive(false);
            }
            for (int i = 0; i < batObjects.Length; i++)
            {
                batObjects[i].SetActive(false);
            }
        }
    }
}
