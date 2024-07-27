using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishInteraction : MonoBehaviour
{
    public GameObject[] foodPrefabs; // Array de prefabs de comida (filete, pez, hamburguesa)
    private bool isFull = false;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F) && !isFull)
        {
            if (PlayerProgress.Instance.collectedFoodCount >= 10)
            {
                SpawnFood();
                PlayerProgress.Instance.ResetCollectedFood(); // Reiniciar la comida recolectada
                isFull = true;
                PlayerProgress.Instance.SetFoodDelivered(true); // Marcar la comida como entregada
                StartCoroutine(EmptyDishAfterDelay());
            }
        }
    }

    void SpawnFood()
    {
        int currentLevel = PlayerProgress.Instance.currentLevel - 1; // Ajustar nivel actual para el array
        if (currentLevel < foodPrefabs.Length)
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(foodPrefabs[currentLevel], transform.position + new Vector3(0, i * 0.1f, 0), Quaternion.identity);
            }
        }
        else
        {
            Debug.LogError("Current level exceeds the number of food prefabs available.");
        }
    }

    IEnumerator EmptyDishAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        isFull = false;
    }
}

