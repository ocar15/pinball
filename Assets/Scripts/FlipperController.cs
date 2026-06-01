using UnityEngine;
using UnityEngine.InputSystem;

public class FlipperController : MonoBehaviour
{
    public Flipper flipperLeft;
    public Flipper flipperRight;

    InputAction flipperLeftAction;
    InputAction flipperRightAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flipperLeftAction = InputSystem.actions.FindAction("Flipper Left");
        flipperRightAction = InputSystem.actions.FindAction("Flipper Right");
    }

    // Update is called once per frame
    void Update()
    {
        // Left
        if (flipperLeftAction.WasPerformedThisFrame())
        {
            flipperLeft.setActive(true);
        } else if (flipperLeftAction.WasCompletedThisFrame())
        {
            flipperLeft.setActive(false);
        }

        // Right
        if (flipperRightAction.WasPerformedThisFrame())
        {
            flipperRight.setActive(true);
        } else if (flipperRightAction.WasCompletedThisFrame())
        {
            flipperRight.setActive(false);
        }
    }
}
