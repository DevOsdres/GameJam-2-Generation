using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public GameObject player;
    public float speed = 5f;

    bool isColliding = false;

    public float rotationSpeed = 5f; // Velocidad de rotación del gatito
    public Transform[] waypoints;  // Array para almacenar los puntos de ruta (los puntos que seguirá el gatito)
    private int currentWaypoint = 0;  // Índice del punto de ruta actual

    Animator animatorCat;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsWaypoint();
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
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
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
