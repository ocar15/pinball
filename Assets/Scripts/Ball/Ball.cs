using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private BallData ballData;

    private bool frozen;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        
        ballData = new BallData();
        ballData.name = "bart";
    }

    // Update is called once per frame
    void Update()
    {
        // Freeze
        rb.isKinematic = frozen;
    }

    public BallData GetBallData()
    {
        return new BallData
        {
            name = ballData.name
        };
    }

    public void Freeze()
    {
        frozen = true;
    }

    public void Unfreeze()
    {
        frozen = false;
    }
}
