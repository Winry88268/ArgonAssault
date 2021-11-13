using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Generic Setup Settings")]
    [Tooltip("Horitontal and Vertical move speed")] [SerializeField] float moveMultiplier = 30f;
    [Tooltip("Bounding for Horizontal movement")] [SerializeField] float xRange = 14f;
    [Tooltip("Bounding for Vertical movement")] [SerializeField] float yRange = 7f;

    [Header("Laser Array")]    
    [Tooltip("Grouping of Player Laser weaponry")] [SerializeField] GameObject[] lasers;
    
    [Header("Screen Position Tuning")]
    [SerializeField] float pitchFactor = -2f;
    [SerializeField] float yawFactor = 2.5f;
     
    [Header("Player Input Tuning")]
    [SerializeField] float pitchMultiplier = -10f;
    [SerializeField] float rollMultiplier = -30f;

    float xThrow, yThrow;

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFire();
    }

    // Horizontal and Vertical Translation of the Player
    void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * moveMultiplier;
        float xNewLocal = transform.localPosition.x + xOffset;
        float xClamp = Mathf.Clamp(xNewLocal, -xRange, xRange);

        float yOffset = yThrow * Time.deltaTime * moveMultiplier;
        float yNewLocal = transform.localPosition.y + yOffset;
        float yClamp = Mathf.Clamp(yNewLocal, -yRange, yRange);

        transform.localPosition = new Vector3(xClamp, yClamp, transform.localPosition.z);
    }

    // Horizontal and Vertical Rotation of the Player
    void ProcessRotation()
    {
        float positionalPitch = transform.localPosition.y * pitchFactor;
        float controllerPitch = yThrow * pitchMultiplier;
        
        float pitch =  positionalPitch + controllerPitch;
        float yaw = transform.localPosition.x * yawFactor;
        float roll = xThrow * rollMultiplier;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    // if(shoot is pressed) > shoot, else do not shoot
    void ProcessFire()
    {
        if (Input.GetButton("Fire1"))
        {
            LaserFire(true);
        }
        else
        {
            LaserFire(false);
        }
    }

    // De/Activate Laser Particle Emitter
    public void LaserFire(bool activity)
    {
        foreach(GameObject i in lasers)
        {
            var j = i.GetComponent<ParticleSystem>().emission;
            j.enabled = activity;
        }
    }
}