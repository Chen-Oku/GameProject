using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerSak : MonoBehaviour
{
    public CharacterController charController;
    public Transform cameraTransform;
    public float speed = 6.0f;
    public float runSpeed = 12.0f; // Velocidad de correr
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float turnSmoothTime = 0.1f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float coyoteTime = 0.2f; // Tiempo de coyote

    private float turnSmoothVelocity;
    private Vector3 velocity;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isDashing;
    private float dashTime;
    private float coyoteTimeCounter; // Contador de tiempo de coyote

    private PlayerAttack playerAttack; // Referencia al script PlayerAttack

    void Start()
    {
        charController = charController ?? GetComponent<CharacterController>();
        playerAttack = GetComponent<PlayerAttack>(); // Obtener el componente PlayerAttack
    }

    void Update()
    {
        isGrounded = charController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            canDoubleJump = true;
            coyoteTimeCounter = coyoteTime; // Reiniciar el contador de tiempo de coyote
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // Disminuir el contador de tiempo de coyote
        }

        if (!isDashing)
        {
            Move();
            Jump();
        }

        Dash(); // Permitir dash incluso si el personaje está corriendo

        ApplyGravity();
        charController.Move(velocity * Time.deltaTime);

        // Detectar interacción con la puerta
        InteractWithDoor();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : speed;

        Vector3 moveDir = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * direction;
        charController.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
    }

    public void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || coyoteTimeCounter > 0f)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                coyoteTimeCounter = 0f; // Resetear el contador de tiempo de coyote después de saltar
            }
            else if (canDoubleJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                canDoubleJump = false;
            }
        }
    }

    private void Dash()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetKeyDown(KeyCode.E) && direction.magnitude >= 0.1f) // Presionar E para dash
        {
            StartCoroutine(DashCoroutine(direction));
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    private IEnumerator DashCoroutine(Vector3 direction)
    {
        isDashing = true;
        dashTime = dashDuration;

        float targetAngle = cameraTransform.eulerAngles.y;
        Vector3 dashDir = Quaternion.Euler(0f, targetAngle, 0f) * direction;

        while (dashTime > 0)
        {
            charController.Move(dashDir.normalized * dashSpeed * Time.deltaTime);
            dashTime -= Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    private void InteractWithDoor()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 2f))
            {
                Debug.Log("Raycast hit: " + hit.collider.name); // Log para verificar el objeto golpeado
                OpenDoors door = hit.collider.GetComponent<OpenDoors>();
                if (door != null)
                {
                    door.Interact();
                    Debug.Log("Interacted with door: " + door.name); // Log para verificar la interacción
                }
            }
            else
            {
                Debug.Log("Raycast no hit"); // Log para verificar que el Raycast no golpeó nada
            }
        }
    }
}