using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Plunger : MonoBehaviour
{
    [SerializeField] private DebugManager debugManager;

    private Vector3 scale;

    private int steps = 5;
    private Vector3 startPos;
    private Vector3 currentPos;
    private float pullStopPercentage = 0.7f;

    private float pullSpeed = .2f;
    private float launchSpeed = 1.5f;
    private float speed;

    private bool ready;
    private bool pulling;
    private bool primed;
    private float currentTime;

    private float pullValueRaw;
    private float pullValue;
    private float percentPulled;

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
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePull();
        HandleMove();
    }

    private void HandlePull()
    {
        // Read input
        pullValueRaw = -1 * pullAction.ReadValue<Vector2>().y;
        if(pullValueRaw < 0) pullValueRaw = 0;

        // Pull value = raw input mapped to # of steps
        pullValue = (int) Math.Ceiling((decimal) (pullValueRaw * steps));
        
        // Pulling logic
        if(pullValueRaw > 0 && !pulling) pulling = true;
        else pulling = false;

        // Timer logic
        if (pulling)  currentTime += Time.deltaTime;
        else currentTime = 0;

        // Launching
        if(primed && pullValueRaw == 0) Launch();
    }

    private void HandleMove()
    {
        currentPos = transform.localPosition;

        // Primed position
        percentPulled = pullValue/steps;
        float maxDistance = boxCollider.size.z * gameObject.transform.localScale.z * pullStopPercentage;
        float primedPos = startPos.x + (percentPulled * maxDistance);

        // Determine if ready to pull or primed (ready to launch)
        if(currentPos.x == startPos.x && !pulling) ready = true;
        if(currentPos.x >= primedPos && pulling) primed = true;

        // Set plunger move speed
        if(ready) speed = pullSpeed/scale.z * percentPulled;

        // Move
        transform.localPosition = Vector3.MoveTowards(currentPos, new Vector3(primedPos, 1, currentPos.z), speed);
    }

    private void Launch()
    {
        primed = false;
        ready = false;
        speed = launchSpeed/scale.z * percentPulled;
        Debug.Log("Launched!");
    }

    // Getters
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

    public float GetCurrentPos()
    {
        return currentPos.x;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
