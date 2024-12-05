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
        //StartShake();
    }

    public void StartShake()
    {
        var noise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 1f;
    }

    public void StopShake()
    {
        var noise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0f;
    }
}
