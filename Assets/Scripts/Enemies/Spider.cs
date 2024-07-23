using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    public float radius = 1.0f; // Radio del movimiento en círculos
    public float speed = 2.0f; // Velocidad de movimiento

    private GameObject player;
    private Animator animatorSpider; 
    private Vector3 startPos;
    private Rigidbody enemyRb;

    private bool isColliding  = false; //con el enemigo

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position; // Inicializamos la posición de inicio y el animator
        animatorSpider = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player"); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isColliding)
        {
            Movement();
        }
        else
        {
            bool transitionAni = true;
            Attack(transitionAni);
        }
                           
    }

    private void Movement() // Método encargado de todos los movimientos
    {
        animatorSpider.SetBool("walk", true);
        animatorSpider.SetBool("attack1", false); 
        animatorSpider.SetBool("attack2", false);

        //LinearMovement();
        //CircularMovement();
        DetectEnemy();
        


    }

    private void Attack(bool transitionAnim)
    {
        AnimatorStateInfo stateInfo = animatorSpider.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2") || transitionAnim) // un modo de limitar el random para que no haya tantos cambios de estado si igual no se van a ejecutar
        {
            transitionAnim = false;
            float normalizedTime = stateInfo.normalizedTime;
            if (normalizedTime >= 0.98f) {
                // Ejecutar una acción cuando la animación está en la mitad o más avanzada
                int elegirAtaque = Random.Range(1,3);
                if (elegirAtaque == 1)
                {
                    Attack1();
                }else
                {
                    Attack2();
                }
            }            
            
        }


        
        
    }

    private void CircularMovement() // Movimiento circular
    {
        float angle = Time.time * speed;
        float x = startPos.x + radius * Mathf.Cos(angle);
        float z = startPos.z + radius * Mathf.Sin(angle);

        Vector3 newPos = new Vector3(x, startPos.y, z);
        enemyRb.MovePosition(newPos);  
    }

    private void LinearMovement()
    {
        // Movimiento lineal simple
        Vector3 movement = (transform.forward * -1) * speed * Time.deltaTime;
        enemyRb.MovePosition(transform.position + movement);

    }

    private void Trajectory()
    {
        // Hay que fijar una trayectoria para cuando no haya un enemigo (el player) cerca
    }


    private void OnTriggerStay(Collider collision) {
        if (collision.gameObject.CompareTag("Player"))
        {   
            isColliding = true;
            
        }
    }

    void OnTriggerExit(Collider collision)
    {
        isColliding = false;

    }
    private void DetectEnemy()
    {
        if (player != null)
        {
            // Calcula la dirección hacia el jugador
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            
            // Calcula la rotación que debería tener para mirar hacia el jugador
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            
            // Invertir la rotación en 180 grados alrededor del eje Y
            Quaternion invertRotation = Quaternion.AngleAxis(180f, Vector3.up);
            Quaternion adjustedRotation = targetRotation * invertRotation;

            // Rota la araña suavemente hacia la dirección del jugador
            transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRotation, Time.deltaTime * speed);

            // Movimiento hacia el jugador si es necesario
            
            
            Vector3 movement = lookDirection * speed * Time.deltaTime;
            enemyRb.MovePosition(transform.position + movement);
            
        }
        else
        {
            Debug.LogError("Player GameObject not found. Make sure it exists in the scene.");
        }
    }

        private void Attack1()
        {
            animatorSpider.SetBool("attack1", true);
            animatorSpider.SetBool("attack2", false);
            animatorSpider.SetBool("walk", false);
        }
        private void Attack2()
        { 
            animatorSpider.SetBool("attack2", true);
            animatorSpider.SetBool("attack1", false);
            animatorSpider.SetBool("walk", false);
        }

}
