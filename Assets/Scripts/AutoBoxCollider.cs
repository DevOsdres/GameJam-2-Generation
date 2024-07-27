using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AutoBoxCollider : MonoBehaviour
{
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        AdjustCollider();
    }

    void AdjustCollider()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            boxCollider.size = meshRenderer.bounds.size;
            boxCollider.center = meshRenderer.bounds.center - transform.position;
        }
        else
        {
            Debug.LogWarning("AutoBoxCollider: No MeshRenderer found on the object.");
        }
    }
}
