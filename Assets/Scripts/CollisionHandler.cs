using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [Header("Generic Setup Settings")]
    [Tooltip("Player Hits before Destruction")] [SerializeField] int hitPoints = 3;

    void OnCollisionEnter(Collision other) 
    {
        Debug.Log($"{this.name} triggered by {other.gameObject.name}");
    }

    void OnTriggerEnter(Collider other) 
    {
        Debug.Log($"{this.name} triggered by {other.gameObject.name}");
    }
}