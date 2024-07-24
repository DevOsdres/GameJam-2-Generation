using System.Collections;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 100f; // Velocidad de rotación en grados por segundo

    void Update()
    {
        // Rotar el objeto alrededor de su propio eje Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

