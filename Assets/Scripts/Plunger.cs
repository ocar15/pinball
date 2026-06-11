    using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Plunger : MonoBehaviour
{
    


    InputAction pullAction;
    
    private BoxCollider boxCollider;
    private Rigidbody rb;
    private ConfigurableJoint configJoint;
    [SerializeField] private DebugManager debugManager;

    private Vector3 scale;
    private Vector3 startPosition;

    private int steps = 6;
    private bool ready;
    private bool pulling;
    private bool primed;
    private float currentTime;

    private float pullValueRaw;
    private float pullValue;
    private float prevPullValue;
    private Vector3 currentPosition;
    private float percentPulled;

    public float resistance = -20;
    public float baseForce = 1;
    private float pullForce;
    private float returnForce;
    private float displacement;
    private float maxDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        boxCollider = gameObject.GetComponent<BoxCollider>();
        rb = gameObject.GetComponent<Rigidbody>();
        configJoint = gameObject.GetComponent<ConfigurableJoint>();

        pullAction = InputSystem.actions.FindAction("Pull Plunger");

        scale = gameObject.transform.localScale;

        currentTime = 0;
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePull();
    }

    void FixedUpdate()
    {
        HandlePhysics();
    }

    private void HandlePull()
    {
        // Read input
        pullValueRaw = -1 * pullAction.ReadValue<Vector2>().y;
        if(pullValueRaw < 0) pullValueRaw = 0;

        // Pull value = raw input mapped to # of steps
        pullValue = (int) Math.Ceiling((decimal) (pullValueRaw * steps));
        percentPulled = pullValue/steps;
        
        // Pulling logic
        pulling = pullValue > 0;

        // Timer logic
        if (pulling)  currentTime += Time.deltaTime;
        else currentTime = 0;

        // Launching
        if(primed && pullValue == 0)
        {
            ready = false;
            primed = false;
            pulling = false;
            Launch();
        }

        // Store pullValue
        prevPullValue = pullValue;
    }

    private void HandlePhysics()
    {
        // Get max distance
        maxDistance = boxCollider.size.z;

        // Get displacement
        currentPosition = transform.localPosition;
        displacement = currentPosition.z - startPosition.z;

        // Calculate forces
        pullForce = pullValue * baseForce;
        returnForce = resistance * displacement;

        // Clamp position
        if(displacement > maxDistance && pulling)
        {
            rb.linearVelocity = Vector3.zero;
            pullForce = 0;
            returnForce = 0;

            if(pulling && ready) primed = true;
        } else if(displacement <= 0)
        {
            rb.linearVelocity = Vector3.zero;
            transform.localPosition = startPosition;

            if(!pulling) ready = true;
        }

        // Apply forces
        rb.AddRelativeForce(Vector3.forward * pullForce);
        rb.AddRelativeForce(Vector3.forward * returnForce);
    }

    private void Launch()
    {
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
}
