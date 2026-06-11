using UnityEngine;
using System.Collections.Generic;

public class BallManager : MonoBehaviour
{
    private List<BallData> balls = new List<BallData>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectBall(GameObject ballObject)
    {
        Ball ball = ballObject.GetComponent<Ball>();
        BallData ballData = ball.GetBallData();
        Debug.Log(ballData);

        AddBall(ballData);
        Destroy(ballObject);
    }

    public void AddBall(BallData ballData)
    {
        balls.Add(ballData);
    }
}
