using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishInteraction : MonoBehaviour
{
    public Transform dishTransform; // Asigna esta referencia en el Inspector
    public float radius = 0.5f; // Radio alrededor del plato donde se instanciarán los objetos de comida
    public float foodHeightOffset = 0.5f; // Offset de altura para los objetos tipo Food
    public float foodLifetime = 10f; // Tiempo en segundos antes de que los objetos de comida sean destruidos
    private bool isPlayerNearDish = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearDish = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearDish = false;
        }
    }

    void Update()
    {
        if (isPlayerNearDish && Input.GetKeyDown(KeyCode.F))
        {
            DeliverFood();
        }
    }

    void DeliverFood()
    {
        if (PlayerProgress.Instance.foodDelivered)
        {
            Debug.Log("Food has already been delivered.");
            return;
        }

        if (PlayerProgress.Instance.collectedFoodCount >= 10)
        {
            Debug.Log("Food delivered!");
            PlayerProgress.Instance.DeliverFood(); // Llamar al método DeliverFood del PlayerProgress para actualizar el estado
            InstantiateFoodOnDish();
        }
        else
        {
            Debug.Log("Not enough food to deliver.");
        }
    }

    void InstantiateFoodOnDish()
    {
        if (PlayerProgress.Instance.foodPrefabs.Count > 0)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 position = dishTransform.position + new Vector3(Mathf.Cos(i * 2 * Mathf.PI / 10) * radius, foodHeightOffset, Mathf.Sin(i * 2 * Mathf.PI / 10) * radius);
                Debug.Log("Instantiating food at position: " + position);
                int foodIndex = PlayerProgress.Instance.currentLevel - 1;
                if (foodIndex >= 0 && foodIndex < PlayerProgress.Instance.foodPrefabs.Count)
                {
                    GameObject foodInstance = Instantiate(PlayerProgress.Instance.foodPrefabs[foodIndex], position, Quaternion.identity);
                    Debug.Log("Food instance created: " + foodInstance.name);
                    foodInstance.transform.localScale = PlayerProgress.Instance.foodPrefabs[foodIndex].transform.localScale; // Mantener el tamaño original del prefab
                    foodInstance.transform.SetParent(dishTransform); // Asegurarnos de que la comida se instancie como hija del plato
                    foodInstance.layer = LayerMask.NameToLayer("FoodOnPlate"); // Asignar la capa "FoodOnPlate"
                    Destroy(foodInstance, foodLifetime); // Destruir la comida después de foodLifetime segundos
                }
                else
                {
                    Debug.LogError("Invalid food index for current level: " + foodIndex);
                }
            }
        }
        else
        {
            Debug.LogError("No food prefabs assigned in PlayerProgress.");
        }
    }
}