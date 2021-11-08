using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveMultiplier = 30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xThrow =  Input.GetAxis("Horizontal");
        float yThrow =  Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * moveMultiplier;
        float xNewLocal = transform.localPosition.x + xOffset;

        float yOffset = yThrow * Time.deltaTime * moveMultiplier;
        float yNewLocal = transform.localPosition.y + yOffset;

        transform.localPosition = new Vector3(xNewLocal, yNewLocal, zNewLocal);
    }
}