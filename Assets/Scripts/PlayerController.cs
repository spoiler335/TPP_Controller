using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BulletController bulletPrefab;
    [SerializeField] private Transform barreltransform;
    [SerializeField] private Transform bulletParent;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private float rotationSpeed = 5f;
    private Transform cameraTransform;
    private float bulletMaxAirDistance = 25f;

    private InputManager input => DI.di.input;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
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
        CheckAndFire();
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
        Vector3 move = new Vector3(input.forward, 0, input.right);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);
        if (move != Vector3.zero) transform.forward = move;
    }

    private void CheckAndFire()
    {
        if (input.isFireClicked)
        {
            var bullet = Instantiate(bulletPrefab, barreltransform.position, Quaternion.identity, bulletParent);

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Player")
                {
                    Destroy(bullet);
                    return;
                }
                Debug.Log($"Fired At {hit.collider.name}");
                bullet.target = hit.point;
                bullet.hit = true;
            }
            else
            {
                bullet.target = cameraTransform.position + cameraTransform.forward * bulletMaxAirDistance;
                bullet.hit = false;
            }
        }
    }
}