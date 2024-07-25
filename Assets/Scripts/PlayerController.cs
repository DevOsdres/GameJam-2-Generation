using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public float rotationSpeed = 300f;
    public float stamina = 5f;
    public float staminaRecoveryRate = 0.5f;
    public float staminaDepletionRate = 1f;

    [HideInInspector]
    public float currentStamina;

    private bool isRunning;
    private Rigidbody rb;
    private Animator animator;
    private bool isAttacking = false;
    private bool isDefending = false;
    private float attackCooldown = 0.5f;
    private float lastAttackTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentStamina = stamina;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        if (!isDefending && !isAttacking)
        {
            Move();
        }

        Rotate();
        HandleAttack();
        HandleDefend();
        HandleAnimations();
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 2.5f)
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

        Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
        float speed = isRunning ? runSpeed : walkSpeed;
        moveDirection *= speed;

        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

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

    void HandleAnimations()
    {
        float speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;

        animator.SetFloat("Speed", speed);
        animator.SetBool("IsRunning", isRunning && currentStamina > 2.5f);
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