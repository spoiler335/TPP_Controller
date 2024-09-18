using UnityEngine;

public class InputManager
{
    private PlayerControls inputActions;

    public InputManager()
    {
        inputActions = new PlayerControls();
        inputActions.Enable();
        Debug.Log("Input Enabled");
    }

    public float forward => inputActions.Player.Move.ReadValue<Vector2>().x;
    public float right => inputActions.Player.Move.ReadValue<Vector2>().y;
    public bool isJumpClicked => inputActions.Player.Jump.WasPressedThisFrame();
    public bool isAimClicked => inputActions.Player.Aim.WasPressedThisFrame();
    public bool isAimReleased => inputActions.Player.Aim.WasReleasedThisFrame();

    public bool isFireClicked => inputActions.Player.Shoot.WasPressedThisFrame();

}