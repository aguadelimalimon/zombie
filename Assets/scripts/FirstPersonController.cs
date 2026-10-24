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

    // Sistema de disparo
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 30f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Bloquea el cursor en el centro y lo oculta
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // --- 1. REVISAR SI ESTÁ EN EL SUELO ---
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Mantiene al jugador firmemente pegado al suelo
        }

        // --- 2. MOVIMIENTO DE LA CÁMARA (MOUSE) ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // --- 3. MOVIMIENTO (WASD) ---
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDir = transform.right * moveX + transform.forward * moveZ;
        Vector3 horizontalMove = moveDir * moveSpeed;

        // --- 4. SALTO (BARRA ESPACIADORA) ---
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // --- 5. APLICAR GRAVEDAD ---
        velocity.y += gravity * Time.deltaTime;

        // --- 6. MOVIMIENTO FINAL UNIFICADO (¡Una sola llamada a controller.Move!) ---
        Vector3 finalMove = horizontalMove + new Vector3(0, velocity.y, 0);
        controller.Move(finalMove * Time.deltaTime);

        // --- 7. DISPARO (CLICK IZQUIERDO) ---
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        
        // Presionar ESC para liberar el cursor si lo necesitas
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * bulletSpeed;
            }
            Destroy(bullet, 3f); // Destruye la bala a los 3 segundos para ahorrar memoria
        }
    }
}