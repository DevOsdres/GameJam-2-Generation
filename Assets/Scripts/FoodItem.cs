using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    private bool isDelivered = false;
    private float lifetime;

    public void SetAsDelivered(float lifetime)
    {
        isDelivered = true;
        this.lifetime = lifetime;
        StartCoroutine(DestroyAfterLifetime());
    }

    public bool IsDelivered()
    {
        return isDelivered;
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isDelivered && other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}