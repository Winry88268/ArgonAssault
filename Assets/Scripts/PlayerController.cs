using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveMultiplier = 30f;
    [SerializeField] float xRange = 14f;
    [SerializeField] float yRange = 7f;
    
    [SerializeField] float pitchFactor = -2f;
    [SerializeField] float pitchMultiplier = -10f;
    [SerializeField] float yawFactor = 2.5f;
    [SerializeField] float rollMultiplier = -30f;

    float xThrow, yThrow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
    }

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

    void ProcessRotation()
    {
        float positionalPitch = transform.localPosition.y * pitchFactor;
        float controllerPitch = yThrow * pitchMultiplier;
        
        float pitch =  positionalPitch + controllerPitch;
        float yaw = transform.localPosition.x * yawFactor;
        float roll = xThrow * rollMultiplier;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }
}