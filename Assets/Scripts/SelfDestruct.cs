using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [Header("System Generic Settings")]
    [Tooltip("Delay before automatic Clean-up of Unnecessary Entities")] [SerializeField] float cleanupDelay = 2f;

    void Start()
    {
        Destroy(gameObject, cleanupDelay);
    }
}