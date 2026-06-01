using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Plunger : MonoBehaviour
{
    [SerializeField] private DebugManager debugManager;

    private float maxDistance = 10f;
    private float pullTime = 1.5f;

    private bool pulling;
    private bool primed;
    private float currentTime;
    private float targetDistance;
    public float launchPower;

    InputAction pullAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pullAction = InputSystem.actions.FindAction("Pull Plunger");

        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePull();
    }

    private void HandlePull()
    {
        float pullValue = -1 * pullAction.ReadValue<Vector2>().y;

        if(pullValue < 0) pullValue = 0;

        // Joystick distance determines target pull 
        targetDistance = maxDistance * pullValue;

        // Power of launch determined by max pull distance
        launchPower = (int) Math.Ceiling((decimal) targetDistance);
        
        // Pulling logic
        if(pullValue > 0 && !pulling)
        {
            pulling = true;
        } else if (pullValue <= 0){
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

        // Plunger is primed when timer reaches max time
        if(currentTime >= pullTime && pulling)
        {
            primed = true;
            pulling = false;
        }

        // When the joystick is let go and the plunger is primed, the ball is launched
        if(primed && pullValue == 0)
        {
            Launch();
        }
    }

    private void Launch()
    {
        primed = false;
        Debug.Log("Launched!");
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

    public float GetTargetDistance()
    {
        return targetDistance;
    }

    public float GetLaunchPower()
    {
        return launchPower;
    }
}
