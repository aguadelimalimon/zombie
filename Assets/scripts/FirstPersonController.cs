
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;

    private CharacterController controller;
    private float verticalRotation = 0f;

    // Variables de Gravedad y Salto
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Bloquear y ocultar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Revisar si está en el suelo
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 2. Movimiento de la cámara
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraTransform.localRotation =
            Quaternion.Euler(verticalRotation, 0f, 0f);

        // 3. Movimiento WASD
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDir =
            transform.right * moveX +
            transform.forward * moveZ;

        Vector3 horizontalMove = moveDir * moveSpeed;

        // 4. Salto
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 5. Gravedad
        velocity.y += gravity * Time.deltaTime;

        // 6. Movimiento final
        Vector3 finalMove =
            horizontalMove + new Vector3(0, velocity.y, 0);

        controller.Move(finalMove * Time.deltaTime);

        // 7. Liberar cursor con ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
