using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    public float speed, maxSpeed;
    float movX, movZ;
    public float rotationSpeed = 300f;
    public float stamina = 5f;
    public float staminaRecoveryRate = 0.5f;
    public float staminaDepletionRate = 1f;
    public float currentStamina;
    [SerializeField] private bool isAttacking = false;

    [HideInInspector]
    private bool isCharging;
    private Rigidbody rb;
    private Animator animator;

    public Slider staminaSlider; // Referencia al Slider de UI
    private bool isDefending = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentStamina = stamina;

        // Configurar el Slider de estamina
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = stamina;
            staminaSlider.value = currentStamina;
        }
    }

    void Update()
    {
        movZ = Input.GetAxis("Vertical");
        movX = Input.GetAxis("Horizontal");

        if (isAttacking || isDefending)
        {
            rb.Sleep();
        }
        else
        {
            if (movZ != 0 || movX != 0)
            {
                Move();
            }
            else
            {
                animator.SetFloat("Speed", 0f);
            }

            if (currentStamina <= 0.6f && !isCharging)
            {
                isCharging = true;
                StartCoroutine(ResetStamina());
            }
            else
            {
                StopCoroutine(ResetStamina());
            }
        }

        if (!isPlaying("Attack"))
        {
            isAttacking = false;
        }

        Rotate();
        HandleAttack();
        HandleDefend();

        // Actualizar el valor del Slider de estamina
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }

    void Move()
    {
        currentStamina = Mathf.Clamp(currentStamina, 0, stamina);
        Vector3 moveDirection = transform.forward * movZ + transform.right * movX;

        if (Input.GetKey(KeyCode.LeftShift) && currentStamina >= 0.6f && movZ == 1)
        {
            currentStamina -= staminaDepletionRate * Time.deltaTime;
            rb.MovePosition(transform.position + moveDirection * maxSpeed);
            animator.SetFloat("Speed", 0.6f);
        }
        else
        {
            rb.MovePosition(transform.position + moveDirection * speed);
            animator.SetFloat("Speed", 0.2f);
        }
    }

    void Rotate()
    {
        float mouseRotation = Input.GetAxis("Mouse X");
        float rotation = mouseRotation * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
    }

    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
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

    IEnumerator ResetStamina()
    {
        yield return new WaitForSeconds(5);
        currentStamina = stamina;
        isCharging = false;
    }

    public void TakeDamage()
    {
        animator.SetTrigger("GetHit");
        Debug.Log("Player took damage");
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        Debug.Log("Player died");
    }

    bool isPlaying(string stateName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}