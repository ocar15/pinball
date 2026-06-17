using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BallManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject respawnPosition;
    public Plunger plunger;

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
        // Get components
        Ball ball = ballObject.GetComponent<Ball>();
        BallData ballData = ball.GetBallData();

        // Return ball to list
        AddBall(ballData);

        // Destroy physical ball
        Destroy(ballObject);

        // Send next ball
        SpawnBall();
    }

    public void AddBall(BallData ballData)
    {
        balls.Add(ballData);
    }

    public void SpawnBall()
    {
        GameObject ballObject = Instantiate(ballPrefab, respawnPosition.transform.position, Quaternion.identity);
    }
}
