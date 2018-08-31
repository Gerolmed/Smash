using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    [Header("Connections")]
    public InputReader.Controller controller;
    public Rigidbody2D rigidbody;
    public float floatHeight;

    [Space(5)]
    [Header("Properties")]
    public float alpha = 3;
    public float speed = 3;
    public float airSpeed = 2;
    public float jumpSpeed = 4;
    public float doubleJumpSpeed = 4;
    public float maxFallSpeed = -20;
    public float rayLength = 1;
    public float rayLengthExtended = 1.05f;
    public float gravUp = 12;
    public float gravDown = 36;
    public float airDamp = 3;
    public float groundDamp = 3;
    public int maxJumps = 2;
    public int saveDistance = 6;
    public float size = 2;

    private bool cooldownJumpInput;
    private float timerJumpInput;

    private bool aRelease;
    private bool blocked;

    private int jumpCounter;



	
	// Update is called once per frame
	void Update () {

        Vector2 vel = rigidbody.velocity;
        float value = vel.y > 0 ? gravUp : gravDown;

        if (isNearGround())
        {
            jumpCounter = 0;
            blocked = false;
        }

        vel.Set(vel.x, isGrounded() ? 0 : vel.y - value * Time.deltaTime);
        if (vel.y < maxFallSpeed)
            vel.Set(vel.x, maxFallSpeed);
        rigidbody.velocity = vel;

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
                    if (InputReader.getInput(controller, InputReader.ControlType.VERTICAL) == 1)
                    {
                        if(!isNearGround() && !blocked)
                            save();
                    }
                    if (InputReader.getInput(controller, InputReader.ControlType.VERTICAL) == -1)
                    {
                        if (isNearGround())
                            down();
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
        if (Math.Abs(rigidbody.velocity.x) > 0)
        {
            float newX = rigidbody.velocity.x;

            newX = Mathf.Lerp(newX, 0, alpha * Time.deltaTime);

            rigidbody.velocity = new Vector2(newX, rigidbody.velocity.y);
        }
        //Debug.Log("Slow");
    }

    public virtual void moveRight()
    {
        if (isGrounded())
            rigidbody.velocity = new Vector2(Mathf.Lerp(rigidbody.velocity.x, speed, groundDamp * Time.deltaTime), rigidbody.velocity.y);
        else
            rigidbody.velocity = new Vector2(Mathf.Lerp(rigidbody.velocity.x, speed, airDamp * Time.deltaTime), rigidbody.velocity.y);
    }

    public virtual void moveLeft()
    {
        if (isGrounded())
            rigidbody.velocity = new Vector2(Mathf.Lerp(rigidbody.velocity.x, -speed, groundDamp * Time.deltaTime), rigidbody.velocity.y);
        else
            rigidbody.velocity = new Vector2(Mathf.Lerp(rigidbody.velocity.x, -speed, airDamp * Time.deltaTime), rigidbody.velocity.y);
    }

    public virtual void jump()
    {
        if (isNearGround())
        {
            jumpCounter++;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
        }
        else if (jumpCounter < maxJumps && !blocked)
        {
            jumpCounter++;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, doubleJumpSpeed);
        }
        //Debug.Log("A");
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
        Vector2 vector = findFreeLocation(this.transform.position, saveDistance);
        this.transform.position = this.transform.position = new Vector3(vector.x, vector.y, 0);
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
        blocked = true;
        Debug.Log("V");
    }

    public virtual void down()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, rayLength, 1 << LayerHelper.getLayer(LayerHelper.Layer.GOUND_NO_COLLISION));
        if (hit.collider != null)
        {
            OneWayPlatform oneWayPlatForm = hit.collider.gameObject.GetComponent<OneWayPlatform>();
            oneWayPlatForm.unblock(this.name, 0.1f);
        }
    }

    public bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, rayLength, 1 << LayerHelper.getLayer(LayerHelper.Layer.GROUND));
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    public bool isNearGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, rayLengthExtended, LayerHelper.getLayers(new LayerHelper.Layer[] { LayerHelper.Layer.GROUND, LayerHelper.Layer.GOUND_NO_COLLISION }));
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    public Vector2 findFreeLocation(Vector2 position, float length)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.up, length, 1 << LayerHelper.getLayer(LayerHelper.Layer.GROUND));
        if (hit.collider != null)
        {
            return new Vector2(position.x, hit.point.y-size/2);
        }
        return new Vector2(this.transform.position.x, this.transform.position.y + length);
    }
}
