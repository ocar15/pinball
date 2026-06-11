using UnityEngine;

public class Flipper : MonoBehaviour
{
    public enum Side {LEFT, RIGHT}
    
    public Side side;
    public float torque = 25f;

    private bool isActive = false;

    private Rigidbody rb;
    public FlipperController flipperController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        flipperController.AddFlipper(this);
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
        float direction = 1;

        if(side == Side.RIGHT)
        {
            direction *= -1;
        }

        if (isActive)
        {
            direction *= -1;
        }

        Vector3 currentTorque = new Vector3(0, torque * direction, 0);

        rb.angularVelocity = currentTorque;
    }

    public Side GetSide()
    {
        return side;
    }
}
