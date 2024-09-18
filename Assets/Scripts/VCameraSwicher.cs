using Cinemachine;
using UnityEngine;

public class VCameraSwicher : MonoBehaviour
{
    private InputManager input => DI.di.input;
    private int priorityBoost = 5;
    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (input.isAimClicked) StartAim();
        if (input.isAimReleased) CancelAim();
    }

    private void StartAim()
    {
        // Switch to aim camera
        Debug.Log("Start Aim");
        virtualCamera.Priority += priorityBoost;
    }

    private void CancelAim()
    {
        // Switch back to freelook camera
        Debug.Log("Cancel Aim");
        virtualCamera.Priority -= priorityBoost;
    }
}