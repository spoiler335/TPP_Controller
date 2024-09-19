using Cinemachine;
using UnityEngine;

public class VCameraSwicher : MonoBehaviour
{
    [SerializeField] private Canvas freelookCanvas;
    [SerializeField] private Canvas aimCanvas;


    private InputManager input => DI.di.input;
    private int priorityBoost = 5;
    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        freelookCanvas.enabled = true;
        aimCanvas.enabled = false;
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

        freelookCanvas.enabled = false;
        aimCanvas.enabled = true;
    }

    private void CancelAim()
    {
        // Switch back to freelook camera
        Debug.Log("Cancel Aim");
        virtualCamera.Priority -= priorityBoost;

        freelookCanvas.enabled = true;
        aimCanvas.enabled = false;
    }
}