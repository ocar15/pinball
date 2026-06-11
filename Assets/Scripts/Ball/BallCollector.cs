using UnityEngine;

public class BallCollector : MonoBehaviour
{
    public BallManager ballManager;

    void OnTriggerEnter(Collider other)
    {
        Ball ball = other.gameObject.GetComponent<Ball>();

        if (ball)
        {
            ballManager.CollectBall(other.gameObject);
        }
    }
}
