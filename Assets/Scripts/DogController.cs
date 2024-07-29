using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DogController : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animatorDog;

    public Transform[] waypoints;  // Array para almacenar los puntos de ruta
    private int currentWaypoint = 0;  // Índice del punto de ruta actual
    public float speed = 5f;
    public float rotationSpeed = 5f; // Velocidad de rotación del perrito

    public GameObject comida;
    void Start()
    {
        animatorDog = GetComponent<Animator>();
        StartCoroutine(MoveToWaypoints());
    }

    // Update is called once per frame
    void Update() //prototipo para probar la comida
    {
    if (comida != null)
        {
            if (comida.activeSelf)
            {
                Vector3 x = GetFood();
                Debug.Log(x);
            }
            else
            {
                // no esta activo
            }
        }
        //GetFood();
    }


    

    IEnumerator MoveToWaypoints()
    {
        while (true)
        {
            Vector3 targetPosition = waypoints[currentWaypoint].position;
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            
            animatorDog.SetBool("walk", true);
            animatorDog.SetBool("sit", false);

            //se mueve el perro, mientras la distancia hacia el punto objetivo sea alta (no ha llegado)
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                // Rotar hacia el punto
                RotateTowards(targetPosition);

                // Mover el perro a la direccion del punto objetivo
                transform.position += moveDirection * speed * Time.deltaTime;

                yield return null;  // Esperar al siguiente frame
            }

            
            animatorDog.SetBool("walk", false);
            animatorDog.SetBool("sit", true);

            
            yield return new WaitForSeconds(10);

            // Elegir un nuevo punto de ruta aleatorio
            currentWaypoint = Random.Range(0, waypoints.Length);
        }
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Rotar suavemente hacia la dirección del punto de ruta
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }



    public Vector3 GetFood()
    {
        if (comida != null)
        {
            return comida.transform.position;
        }
        else
        {
            Debug.LogWarning("El GameObject objetivo no está asignado.");
            return Vector3.zero; // Devuelve una posición por defecto en caso de que targetObject sea null
        }
    }

}
