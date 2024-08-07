using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour  //algunas variables son de la araña por que reutilice mucho codigo y no tuve tiempo de cambiarlo
{
    public float radius = 1.0f; // Radio del movimiento en círculos
    public float speed = 2.0f; // Velocidad de movimiento

    public Transform[] waypoints;  // Array para almacenar los puntos de ruta
    private int currentWaypoint = 0;  // Índice del punto de ruta actual
    public float rotationSpeed = 5f; // Velocidad de rotación hacia puntos de ruta
    private GameObject player;
    private Animator animatorSpider; 
    private Vector3 startPos;
    private Rigidbody enemyRb;
    public float detectionDistance;
    private bool isColliding  = false; //con el enemigo

    public bool isDead = false;
    [SerializeField] AudioClip atackSound; 

    // Start is called before the first frame update
    void Start()
    {
        //startPos = transform.position; // Inicializamos la posición de inicio y el animator
        animatorSpider = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player"); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //isDead = true;

        if (!isDead)
        {        
            if (DistanceToPlayer() < detectionDistance)
                {
                    //MoveTowardsPlayer();
                    if (!isColliding)
                    {
                        Movement();
                    }
                    else
                    {
                        bool transitionAni = true;
                        Attack(transitionAni);
                    }

                }else
                {
                    animatorSpider.SetBool("walk", true);
                    MoveTowardsWaypoint();
                }
        }else
        {
           
            animatorSpider.SetBool("dead", true);
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

    void MoveTowardsWaypoint()
    {
        // Obtener la dirección hacia el punto de ruta actual
        Vector3 targetPosition = waypoints[currentWaypoint].position;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

         // Rotar suavemente hacia el punto de ruta actual
        RotateTowards(targetPosition);

        // Mover al enemigo en dirección al punto de ruta actual
        transform.position += moveDirection * speed * Time.deltaTime;

        // Verificar si el enemigo ha llegado lo suficientemente cerca del punto de ruta actual
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Avanzar al siguiente punto de ruta
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }


    private float DistanceToPlayer()
    {
        // Calcular la distancia al jugador
        if (player != null)
        {
            return Vector3.Distance(transform.position, player.transform.position);
        }
        return Mathf.Infinity; // Retorna infinito si el jugador no está disponible
    }

    void RotateTowards(Vector3 targetPosition)
    {
        // Calcular la dirección hacia el punto de ruta
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);


        // Rotar suavemente hacia la dirección del punto de ruta
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Attack(bool transitionAnim)
    {
        AnimatorStateInfo stateInfo = animatorSpider.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.IsName("Bear_Attack1") || stateInfo.IsName("Bear_Attack3") || transitionAnim) // un modo de limitar el random para que no haya tantos cambios de estado si igual no se van a ejecutar
        {
            
            transitionAnim = false;
            float normalizedTime = stateInfo.normalizedTime;
            if (normalizedTime >= 0.98f) {
                // Ejecutar una acción cuando la animación está en la mitad o más avanzada
                int elegirAtaque = Random.Range(1,3);
                if (elegirAtaque == 1)
                {
                    Attack1();
                    AudioManager2.Instance.PlaySFXOne(atackSound);
                }else
                {
                    Attack2();
                    AudioManager2.Instance.PlaySFX(atackSound);
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
            

            // Rota el oso suavemente hacia la dirección del jugador
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);

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
