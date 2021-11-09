using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [Header("Generic Setup Settings")]
    [Tooltip("Player Hits before Destruction")] [SerializeField] int hitPoints = 3;

    void OnTriggerEnter(Collider other) 
    {
        Debug.Log($"{this.name} triggered by {other.gameObject.name}");
        
        hitPoints--;
        Debug.Log($"{this.name} has {hitPoints} lives remaining");
        if(hitPoints == 0)
        {
            Destroy(gameObject);
            Debug.Log($"{this.name} destroyed by {other.gameObject.name}");
        }
    }
}