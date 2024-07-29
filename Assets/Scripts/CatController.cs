using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    private GameObject player;
    public float speed = 5f;

    bool isColliding = false;

    public float rotationSpeed = 5f; // Velocidad de rotación del gatito
    public Transform[] waypoints;  // Array para almacenar los puntos de ruta (los puntos que seguirá el gatito)
    private int currentWaypoint = 0;  // Índice del punto de ruta actual

    private bool onRoute;

    private bool onAction;
    private bool onSound;

    private bool onJump;

    private bool boo = true;
    Animator animatorCat;
    // Start is called before the first frame update
    void Start()
    {
        //transform.position = waypoints[0].position;
        animatorCat = GetComponent<Animator>();
        player = GameObject.Find("Player");
        onRoute = true;
        onAction = false;
        onJump = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        //if hay comida
        AnimatorStateInfo stateInfo = animatorCat.GetCurrentAnimatorStateInfo(0);
        float duration = stateInfo.length;
        //Debug.Log("La duración de la animación actual es: " + duration + " segundos.");

        if (stateInfo.IsName("Idle"))
        {
            int azar = Random.Range(1,101);
            Debug.Log(azar);
            if (azar >= 1 && azar <= 30)
            {
                animatorCat.SetBool("walk",true);
                onRoute = true;
                onAction = true;
            }else if (azar > 30 && azar <=60)
            {
                onAction = true;
                onSound = true;
                animatorCat.SetBool("sound", true);   
            }
            else if (azar > 60 && azar <= 100)
            {
                onJump = true;
                onAction = true;
                animatorCat.SetBool("walk", false);
                animatorCat.SetBool("jump", true);
                animatorCat.SetBool("sound",  false);
            }
        }else
        {
            //AnimatorStateInfo stateInfo = animatorCat.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Walk"))
            {
                MoveTowardsWaypoint();
            }else if(stateInfo.IsName("sound"))
            { 
                onAction = false;
                onSound = false;
                animatorCat.SetBool("sound", false);
            }else
            {
                onJump = false;
                animatorCat.SetBool("jump", false);
            }        
        }
   



        
        
    }

    IEnumerator Wait(int time)
    {
        yield return new WaitForSeconds(time);
    }

    void MoveTowardsWaypoint()
    {
        
        
        // Obtener la dirección hacia el punto de ruta actual
        Vector3 targetPosition = waypoints[currentWaypoint].position;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

         // Rotar suavemente hacia el punto de ruta actual
        RotateTowards(targetPosition);

        // Mover al gatito en dirección al punto de ruta actual
        transform.position += moveDirection * speed * Time.deltaTime;

        // Verificar si el gatito ha llegado lo suficientemente cerca del punto de ruta actual
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Avanzar al siguiente punto de ruta
            currentWaypoint = Random.Range(0,waypoints.Length);
            onRoute = false;
            onAction = false;
            animatorCat.SetBool("walk",false);
            
            //currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void sound()
    {
        
    }

    void RotateTowards(Vector3 targetPosition)
    {
        // Calcular la dirección hacia el punto de ruta
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Rotar suavemente hacia la dirección del punto de ruta
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void Eat()
    {
        animatorCat.SetBool("eat",true);
    }

}
