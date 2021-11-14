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
    [Tooltip("Maximum Laser Power")] [SerializeField] float laserPower = 10000f;
    [Tooltip("Laser Power Regen per Update")] [SerializeField] float laserRegen = 5f;
    [Tooltip("Laser Power Drain per Update")] [SerializeField] float laserDrain = 10f;

    [Header("Screen Position Tuning")]
    [SerializeField] float pitchFactor = -2f;
    [SerializeField] float yawFactor = 2.5f;
     
    [Header("Player Input Tuning")]
    [SerializeField] float pitchMultiplier = -10f;
    [SerializeField] float rollMultiplier = -30f;

    GameManager gm;

    float xThrow, yThrow, power;
    bool isFiring = false;

    void Start() 
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(!gm.isPaused)
        {
            laserUpdate();
            ProcessTranslation();
            ProcessRotation();
            ProcessFire();
        }  
    }

    // Laser Power Modification
    void laserUpdate()
    {
        if(isFiring && laserPower >= laserDrain)
        {
            laserPower = Mathf.Clamp((laserPower - laserDrain), 0f, 10000f);
        }
        else
        {
            laserPower = Mathf.Clamp((laserPower + laserRegen), 0f, 10000f);
        }

        power = laserPower/10000f;
        gm.powerUpdate(power);
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
        if (Input.GetButton("Fire1") && laserPower > laserDrain)
        {
            LaserFire(true);
            isFiring = true;
        }
        else
        {
            LaserFire(false);
            isFiring = false;
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