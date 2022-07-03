using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public float intensity;
    public float time;

    public static CameraShake Instance {get; private set;}
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;

    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;


    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

        cinemachineBasicMultiChannelPerlin = 
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera()
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
        }

        if (shakeTimer <= 0f)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }
}