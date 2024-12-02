using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class KainatEarthQuakeScript : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    public bool keepShaking;

    void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void StartShake(float intensity)
    {
        var noise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        noise.m_AmplitudeGain = intensity;
    }

    public void StopShake()
    {
        var noise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 1f;
    }
}
