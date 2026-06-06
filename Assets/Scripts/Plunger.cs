using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Plunger : MonoBehaviour
{
    [SerializeField] private DebugManager debugManager;

    private Vector3 scale;

    private int steps = 5;
    private float startPos;
    private float currentPos;
    private float pullStopPercentage = 0.9f;

    public float pullSpeed = 1f;
    public float launchSpeed = .5f;
    private float speed;

    private bool ready;
    private bool pulling;
    private bool primed;
    private float currentTime;

    private float pullValueRaw;
    private float pullValue;
    private float targetDistance;

    private BoxCollider boxCollider;
    private Rigidbody rb;
    private ConfigurableJoint configJoint;

    InputAction pullAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        boxCollider = gameObject.GetComponent<BoxCollider>();
        rb = gameObject.GetComponent<Rigidbody>();
        configJoint = gameObject.GetComponent<ConfigurableJoint>();

        pullAction = InputSystem.actions.FindAction("Pull Plunger");

        scale = gameObject.transform.localScale;

        currentTime = 0;
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePull();
        HandleMove();
    }

    private void HandlePull()
    {
        pullValueRaw = -1 * pullAction.ReadValue<Vector2>().y;

        if(pullValueRaw < 0) pullValueRaw = 0;

        // Determine max distance (for now, just the length of plunger)
        float maxDistance = boxCollider.size.z * gameObject.transform.localScale.z * pullStopPercentage;

        // pullValue = mapping pullValueRaw to # of steps
        pullValue = (int) Math.Ceiling((decimal) (pullValueRaw * steps));

        // targetDistance = mapping pullPower to actual maxDistance
        targetDistance = pullValue/steps * maxDistance;
        
        // Pulling logic
        if(pullValueRaw > 0 && !pulling)
        {
            pulling = true;
        } else if (pullValueRaw <= 0){
            pulling = false;
        }

        // Timer logic
        if (pulling)
        {
            currentTime += Time.deltaTime;
        } else
        {
            currentTime = 0;
        }

        // When the joystick is let go and the plunger is primed, the ball is launched
        if(primed && pullValueRaw == 0)
        {
            Launch();
        }
    }

    private void HandleMove()
    {
        currentPos = transform.position.x;

        // Plunger is ready after launching and returning to position 0 (for now)
        if(currentPos == startPos && !pulling) ready = true;

        // Calculate "primed" position
        float targetPos = startPos + targetDistance;

        // Plunger is primed when it reaches the target position
        if(pulling && transform.position.x >= targetPos)
        {
            primed = true;
        }

        // Pull speed scale
        if(ready) speed = pullSpeed/scale.z;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos, 1, transform.position.z), speed);
    }

    private void Launch()
    {
        primed = false;
        ready = false;
        speed = launchSpeed/scale.z;
        Debug.Log("Launched!");
    }



    public bool IsReady()
    {
        return ready;
    }

    public bool IsPulling()
    {
        return pulling;
    }

    public bool IsPrimed()
    {
        return primed;
    }

    public float GetTimer()
    {
        return currentTime;
    }

    public float GetPullValueRaw()
    {
        return pullValueRaw;
    }

    public float GetPullValue()
    {
        return pullValue;
    }

    public float GetTargetDistance()
    {
        return targetDistance;
    }

    public float GetCurrentPos()
    {
        return currentPos;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
