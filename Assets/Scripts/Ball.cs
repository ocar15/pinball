using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        // rb.maxAngularVelocity = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
