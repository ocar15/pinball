using UnityEngine;

public class Flipper : MonoBehaviour
{
    public enum Side {LEFT, RIGHT}
    
    public Side side;
    private float restAngle = -115f;
    private float angleDiff = 60f;
    private float speed = 20f;
    private bool isActive = false;

    private float targetAngle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.targetAngle = restAngle;
    }

    // Update is called once per frame
    void Update()
    {
        HandleFlip();
    }

    public void setActive(bool isActive)
    {
        this.isActive = isActive;
    }

    private void HandleFlip()
    {
        if (isActive)
        {
            targetAngle = restAngle + angleDiff;
        } else
        {
            targetAngle = restAngle;
        }

        float sideModifier = 1f;
        if(side == Side.LEFT)
        {
            sideModifier = -1f;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetAngle * sideModifier, 0), speed * Time.deltaTime);
    }
}
