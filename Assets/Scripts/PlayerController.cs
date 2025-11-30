using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float animationSmoothTime = 0.1f;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 7.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private float rotationSpeed = 5f;
    private Transform cameraTransform;

    private InputManager input => DI.di.input;

    private Animator anim;
    private int moveXId;
    private int moveZId;

    private Vector2 currAnimBlend;
    private Vector2 animVelocity;
    private Vector3 move;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Assert.IsNotNull(controller);
        anim = GetComponent<Animator>();
        Assert.IsNotNull(anim);
        moveXId = Animator.StringToHash("MoveX");
        moveZId = Animator.StringToHash("MoveZ");
    }

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CheckAndKeepPlayerOnGround();
        CheckInputAndMovePlayer();
        CheckAndJump();
        BringPlayerOnToGround();
        ControllPlayerRotation();
    }

    private void BringPlayerOnToGround()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void CheckAndKeepPlayerOnGround()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0) playerVelocity.y = 0f;
    }

    private void ControllPlayerRotation()
    {
        var targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void CheckAndJump()
    {
        if (input.isJumpClicked && groundedPlayer)
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }

    private void CheckInputAndMovePlayer()
    {
        if (move == null) move = new Vector3(input.forward, 0, input.right);

        currAnimBlend = Vector2.SmoothDamp(currAnimBlend, input.moveVec, ref animVelocity, animationSmoothTime);

        move.x = currAnimBlend.x;
        move.z = currAnimBlend.y;

        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);
        if (move != Vector3.zero) transform.forward = move;

        anim.SetFloat(moveXId, currAnimBlend.x);
        anim.SetFloat(moveZId, currAnimBlend.y);
    }
}