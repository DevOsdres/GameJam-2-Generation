using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public float rotationSpeed = 300f;
    public float stamina = 5f;
    public float jumpHeight = 2f;
    public float staminaRecoveryRate = 0.5f;
    public float staminaDepletionRate = 1f;

    [HideInInspector]
    public float currentStamina;

    private bool isRunning;
    private bool canDoubleJump;
    private Rigidbody rb;
    private Animator animator;
    private bool isAttacking = false;
    private bool isDefending = false;
    private float attackCooldown = 0.5f;
    private float lastAttackTime = 0f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentStamina = stamina;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        GroundCheck();
        if (!isDefending && !isAttacking)
        {
            Move();
        }

        Rotate();
        HandleAttack();
        HandleDefend();
        HandleJump();
        HandleAnimations();
    }

    void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        if (isGrounded)
        {
            canDoubleJump = false;
            animator.SetBool("Jump", false); // Se asegura de que la animación de salto esté desactivada
        }
    }

    void Move()
    {
        // Permitir correr solo si la estamina es suficiente
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            isRunning = true;
            currentStamina -= staminaDepletionRate * Time.deltaTime;
        }
        else
        {
            isRunning = false;
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, stamina);

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // Define la dirección del movimiento
        Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
        float speed = isRunning ? runSpeed : walkSpeed;
        moveDirection *= speed;

        if (isGrounded)
        {
            rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        }
        else
        {
            rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        }

        float movementMagnitude = new Vector3(moveDirection.x, 0, moveDirection.z).magnitude;
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
            animator.SetBool("IsAttacking", true);
            animator.speed = 1.5f;
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

        if (Input.GetMouseButton(1))
        {
            ContinueDefending();
        }
    }

    void StartDefending()
    {
        isDefending = true;
        animator.SetBool("Defend", true);
        animator.speed = 1.5f;
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    void StopDefending()
    {
        isDefending = false;
        animator.SetBool("Defend", false);
        animator.speed = 1f;
    }

    void ContinueDefending()
    {
        if (!animator.GetBool("Defend"))
        {
            animator.SetBool("Defend", true);
        }
        animator.speed = 1f;
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(jumpHeight * 2f * 9.81f), rb.velocity.z);
                animator.SetTrigger("Jump");
            }
            else if (isRunning && !canDoubleJump)
            {
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(jumpHeight * 2f * 9.81f), rb.velocity.z);
                canDoubleJump = true;
                animator.SetTrigger("Jump");
            }
        }
    }

    void HandleAnimations()
    {
        float speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;

        // Actualiza la animación solo si la estamina permite correr
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsRunning", isRunning && currentStamina > 0); // Solo permitir correr si la estamina es mayor a 2.5
        animator.SetBool("IsWalking", !isRunning && speed > 0);
        animator.SetBool("IsAttacking", isAttacking);
        animator.SetBool("Defend", isDefending);
    }

    IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
        animator.speed = 1f;
    }

    public void TakeDamage()
    {
        animator.SetTrigger("GetHit");
    }

    public void Die()
    {
        animator.SetTrigger("Die");
    }
}