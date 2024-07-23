using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 720f;
    public float jumpHeight = 2f;
    public float runJumpHeight = 3f;
    public float stamina = 5f; // Duración del sprint en segundos
    public float staminaRecoveryRate = 0.5f; // Tasa de recuperación de stamina por segundo
    public float staminaDepletionRate = 1f; // Tasa de agotamiento de stamina por segundo
    [HideInInspector]
    public float currentStamina; // Cambiado a public

    private CharacterController characterController;
    private Vector3 moveDirection;
    private float verticalSpeed = 0f;
    private bool isGrounded;
    private bool isRunning;

    public int foodCount = 0;
    public int foodNeeded = 10;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentStamina = stamina;
    }

    void Update()
    {
        Move();
        Rotate();
        Jump();
    }

    void Move()
    {
        float speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            isRunning = true;
            speed = runSpeed;
            currentStamina -= staminaDepletionRate * Time.deltaTime;
        }
        else
        {
            isRunning = false;
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, stamina);

        float moveDirectionY = moveDirection.y;
        moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
        moveDirection.y = moveDirectionY;

        if (characterController.isGrounded)
        {
            verticalSpeed = -1f;
            if (Input.GetButtonDown("Jump"))
            {
                verticalSpeed = Mathf.Sqrt(isRunning ? runJumpHeight : jumpHeight * -2f * Physics.gravity.y);
            }
        }
        else
        {
            verticalSpeed += Physics.gravity.y * Time.deltaTime;
        }

        moveDirection.y = verticalSpeed;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void Rotate()
    {
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        float mouseRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, mouseRotation, 0);
    }

    void Jump()
    {
        if (characterController.isGrounded)
        {
            isGrounded = true;
            if (Input.GetButtonDown("Jump"))
            {
                verticalSpeed = Mathf.Sqrt((isRunning ? runJumpHeight : jumpHeight) * -2f * Physics.gravity.y);
            }
        }
        else
        {
            isGrounded = false;
            verticalSpeed += Physics.gravity.y * Time.deltaTime;
        }
    }

    public void CollectFood()
    {
        foodCount++;
        Debug.Log("Food collected: " + foodCount);

        if (foodCount >= foodNeeded)
        {
            ReturnToNest();
        }
    }

    void ReturnToNest()
    {
        // Código para regresar al nido
        Debug.Log("Returned to nest with enough food!");
    }
}
