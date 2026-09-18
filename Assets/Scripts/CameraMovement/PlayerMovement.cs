using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        Move();
        ApplyGravity();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float vertical   = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        Vector3 inputDir = new Vector3(horizontal, 0f, vertical).normalized;

        if (inputDir.magnitude < 0.1f)
            return;

        // Move relative to camera direction so forward always matches the camera facing
        Vector3 moveDir;
        if (cameraTransform != null)
        {
            Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 camRight   = Vector3.ProjectOnPlane(cameraTransform.right,   Vector3.up).normalized;
            moveDir = (camForward * inputDir.z + camRight * inputDir.x).normalized;
        }
        else
        {
            moveDir = inputDir;
        }

        // Rotate player to face movement direction
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(moveDir),
            Time.deltaTime * 12f
        );

        controller.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f; // small constant keeps isGrounded reliable

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
