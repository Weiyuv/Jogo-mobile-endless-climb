using UnityEngine;

public class FPSManager : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0; // disable VSync
        Application.targetFrameRate = 60; // set FPS
    }
}