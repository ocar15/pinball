using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class FlipperController : MonoBehaviour
{
    public Flipper flipperLeft;
    public Flipper flipperRight;

    private List<Flipper> flippersLeft = new List<Flipper>();
    private List<Flipper> flippersRight = new List<Flipper>();

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
            flippersLeft.ForEach(delegate(Flipper flipper)
            {
                flipper.setActive(true);
            });
        } else if (flipperLeftAction.WasCompletedThisFrame())
        {
            flippersLeft.ForEach(delegate(Flipper flipper)
            {
                flipper.setActive(false);
            });
        }

        // Right
        if (flipperRightAction.WasPerformedThisFrame())
        {
            flippersRight.ForEach(delegate(Flipper flipper)
            {
                flipper.setActive(true);
            });
        } else if (flipperRightAction.WasCompletedThisFrame())
        {
            flippersRight.ForEach(delegate(Flipper flipper)
            {
                flipper.setActive(false);
            });
        }
    }

    public void AddFlipper(Flipper flipper)
    {
        if(flipper.GetSide() == Flipper.Side.LEFT) flippersLeft.Add(flipper);
        else flippersRight.Add(flipper);
    }
}
