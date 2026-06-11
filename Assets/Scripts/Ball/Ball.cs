using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private BallData ballData;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballData = new BallData();
        ballData.name = "bart";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BallData GetBallData()
    {
        return new BallData
        {
            name = ballData.name

        };
    }
}
