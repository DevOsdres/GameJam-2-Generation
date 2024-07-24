using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 720f;
    public float stamina = 5f;
    [HideInInspector]
    public float currentStamina;

    public float staminaRecoveryRate = 0.5f;
    public float staminaDepletionRate = 1f;

    private bool isRunning;
    private CharacterController characterController;
    private Vector3 moveDirection;
    private Animator animator;
    private bool isAttacking = false;
    private bool isDefending = false;
    private float attackCooldown = 0.5f; // Tiempo de enfriamiento entre ataques
    private float lastAttackTime = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentStamina = stamina;
        
        // Ocultar y bloquear el cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        Rotate();
        HandleAttack();
        HandleDefend();
        HandleAnimations(); // Llamar a la función de manejo de animaciones
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

        float verticalInput = Input.GetAxis("Vertical");
        moveDirection = transform.forward * verticalInput * speed;

        characterController.SimpleMove(moveDirection);

        // Actualizar animaciones
        float movementMagnitude = moveDirection.magnitude;
        animator.SetFloat("Speed", movementMagnitude);
        animator.SetBool("IsWalking", movementMagnitude > 0 && !isRunning);
        animator.SetBool("IsRunning", isRunning && movementMagnitude > 0);
    }

    void Rotate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float mouseRotation = Input.GetAxis("Mouse X");

        float rotation = (horizontalInput + mouseRotation) * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
    }

    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking && Time.time > lastAttackTime + attackCooldown)
        {
            isAttacking = true;
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack");
            // Aumentar la velocidad de la animación de ataque
            animator.speed = 1.5f; // Ajusta este valor para hacer el ataque más rápido
            StartCoroutine(ResetAttackState());
        }
    }

    void HandleDefend()
    {
        if (Input.GetMouseButtonDown(1) && !isDefending)
        {
            StartDefending();
        }
        else if (Input.GetMouseButtonUp(1) && isDefending)
        {
            StopDefending();
        }

        // Mantener la defensa si el botón está presionado
        if (Input.GetMouseButton(1))
        {
            ContinueDefending();
        }
    }

    void StartDefending()
    {
        isDefending = true;
        animator.SetBool("Defend", true);
        // Aumentar la velocidad de la animación de defensa al inicio
        animator.speed = 1.5f; // Ajusta este valor para hacer la defensa más rápida
    }

    void StopDefending()
    {
        isDefending = false;
        animator.SetBool("Defend", false);
        animator.speed = 1f; // Restaurar la velocidad normal de animación
    }

    void ContinueDefending()
    {
        // Asegurarse de que la animación de defensa se mantenga
        if (!animator.GetBool("Defend"))
        {
            animator.SetBool("Defend", true);
        }
        animator.speed = 1f; // Velocidad normal para la animación continua
    }

    IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        animator.speed = 1f; // Restaurar la velocidad normal de animación
    }

    void HandleAnimations()
    {
        float speed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;

        animator.SetFloat("Speed", speed);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsAttacking", isAttacking);
        animator.SetBool("Defend", isDefending);
    }

    public void TakeDamage()
    {
        animator.SetTrigger("GetHit");
        // Aquí puedes añadir lógica adicional para recibir daño
    }

    // Método para manejar la muerte del personaje
    public void Die()
    {
        animator.SetTrigger("Die");
        // Aquí puedes añadir lógica adicional para la muerte del personaje
    }
}