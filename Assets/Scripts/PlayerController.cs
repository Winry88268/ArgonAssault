using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveMultiplier = 30f;

    [SerializeField] float xRange = 14f;
    [SerializeField] float yRange = 16f;


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
        float xThrow = Input.GetAxis("Horizontal");
        float yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * moveMultiplier;
        float xNewLocal = transform.localPosition.x + xOffset;
        float xClamp = Mathf.Clamp(xNewLocal, -xRange, xRange);

        float yOffset = yThrow * Time.deltaTime * moveMultiplier;
        float yNewLocal = transform.localPosition.y + yOffset;
        float yClamp = Mathf.Clamp(yNewLocal, 0.93f, yRange);

        transform.localPosition = new Vector3(xClamp, yClamp, transform.localPosition.z);
    }

    void ProcessRotation()
    {
        transform.localRotation = Quaternion.Euler()
    }
}