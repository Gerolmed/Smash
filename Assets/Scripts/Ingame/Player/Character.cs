using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public float speed = 3;

    public InputReader.Controller controller;
    public Rigidbody2D rigidbody;

    private bool cooldownJumpInput;
    private float timerJumpInput;

    private bool aRelease;

	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Horizontal: "+InputReader.getInput(controller, InputReader.ControlType.HORIZONTAL));
        //Debug.Log("Vertical  : " + InputReader.getInput(controller, InputReader.ControlType.VERTICAL));
        //Debug.Log("A         : " + InputReader.getInput(controller, InputReader.ControlType.A_BTN));
        //Debug.Log("B         : " + InputReader.getInput(controller, InputReader.ControlType.B_BTN));
        //Debug.Log("X         : " + InputReader.getInput(controller, InputReader.ControlType.X_BTN));
        //Debug.Log("Y         : " + InputReader.getInput(controller, InputReader.ControlType.Y_BTN));

        if (InputReader.getInput(controller, InputReader.ControlType.B_BTN) == 1)
        {
            secondary();
            return;
        }

        if (InputReader.getInput(controller, InputReader.ControlType.HORIZONTAL) == 1)
            moveRight();
        else if (InputReader.getInput(controller, InputReader.ControlType.HORIZONTAL) == -1)
            moveLeft();
        else
            slow();

        handleButtons();

    }

    private void handleButtons()
    {
        if (!aRelease && InputReader.getInput(controller, InputReader.ControlType.A_BTN) == 0)
            aRelease = true;

        if (timerJumpInput == 0)
        {
            if (cooldownJumpInput)
            {
                if (InputReader.getInput(controller, InputReader.ControlType.A_BTN) == 1)
                {
                    //TODO: asadssa
                    if (InputReader.getInput(controller, InputReader.ControlType.VERTICAL) == 1)
                    {
                        save();
                    }
                    else
                    {
                        jump();
                    }
                    timerJumpInput = 0.1f;
                    cooldownJumpInput = false;
                    aRelease = false;
                }
                else {
                    cooldownJumpInput = false;
                }
            }
            else if(aRelease && InputReader.getInput(controller, InputReader.ControlType.A_BTN) == 1)
            {
                timerJumpInput = 0.03f;
                cooldownJumpInput = true;
            }
        }
        else
        {
            timerJumpInput -= Time.deltaTime;
            if (timerJumpInput < 0)
                timerJumpInput = 0;
        }
        
    }

    public virtual void slow()
    {
        //Debug.Log("Slow");
    }

    public virtual void moveRight()
    {
        Debug.Log("Right");
    }

    public virtual void moveLeft()
    {
        Debug.Log("Left");
    }

    public virtual void jump()
    {
        Debug.Log("A");
    }

    public virtual void primary()
    {
        Debug.Log("X");
    }

    public virtual void secondary()
    {
        Debug.Log("B");
    }

    public virtual void save()
    {
        Debug.Log("V");
    }
}
