    using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Plunger : MonoBehaviour
{
    public enum PlungerState {READY, PULLING, RELEASED, PRIMED, LAUNCHING}

    InputAction pullAction;
    
    private BoxCollider boxCollider;
    private Rigidbody rb;
    public Collider plungerWall;
    public DebugManager debugManager;

    private Vector3 startPosition;
    private Vector3 currentPosition;

    private PlungerState state;
    private int steps = 6;

    private float pullValueRaw;
    private float pullValue;
    private float pullValueMax;
    private float percentPulled;

    private float displacement;
    private float targetDisplacement;
    private float stopPercentage = 0.8f;

    private float speed;
    public float pullSpeed;
    public float returnSpeed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        // Get components & actions
        boxCollider = gameObject.GetComponent<BoxCollider>();
        rb = gameObject.GetComponent<Rigidbody>();
        pullAction = InputSystem.actions.FindAction("Pull Plunger");

        // Initialize values
        state = PlungerState.READY;
        startPosition = transform.position;
        Debug.Log(startPosition);
        pullValueMax = 0;

        // Ignore plunger wall collision
        Physics.IgnoreCollision(plungerWall, GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        HandlePull();
        HandleClamping();
    }

    void FixedUpdate()
    {
        HandleMove();
    }

    private void HandlePull()
    {
        // Read input
        pullValueRaw = -1 * pullAction.ReadValue<Vector2>().y;
        if(pullValueRaw < 0) pullValueRaw = 0;

        // Pull value = raw input mapped to # of steps
        pullValue = (int) Math.Ceiling((decimal) (pullValueRaw * steps));
        percentPulled = pullValue/steps;

        if(pullValue > pullValueMax)
        {
            pullValueMax = pullValue;
        }
    }

    private void HandleMove()
    {
        currentPosition = transform.position;
        displacement = currentPosition.z - startPosition.z;
        targetDisplacement = boxCollider.size.z * percentPulled * stopPercentage;

        switch (state) {
            case PlungerState.READY:
                speed = 0;
                if(pullValue > 0) state = PlungerState.PULLING;
                break;
            case PlungerState.PULLING:
                speed = pullSpeed;
                if(pullValue < pullValueMax) state = PlungerState.RELEASED;
                if(displacement >= targetDisplacement && percentPulled > 0.2) state = PlungerState.PRIMED;
                break;
            case PlungerState.RELEASED:
                speed = returnSpeed;
                if(pullValueRaw > 0)
                {
                    pullValueMax = pullValue;
                    state = PlungerState.PULLING;
                }
                if(displacement <= 0) state = PlungerState.READY;
                break;
            case PlungerState.PRIMED:
                speed = 0;
                if(pullValue < pullValueMax) state = PlungerState.LAUNCHING;
                break;
            case PlungerState.LAUNCHING:
                speed = returnSpeed;
                if(displacement <= 0) state = PlungerState.READY;
                break;
        }

        rb.MovePosition(rb.position + (transform.forward * speed * Time.deltaTime));
    }

    private void HandleClamping()
    {
        
    }

    // Getters
    public PlungerState GetState()
    {
        return state;
    }

    public float GetPullValueRaw()
    {
        return pullValueRaw;
    }

    public float GetPullValue()
    {
        return pullValue;
    }

    public float GetPercentPulled()
    {
        return percentPulled;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
