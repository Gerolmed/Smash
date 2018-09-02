using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    [Header("Connections")]
    public InputReader.Controller controller;
    public Rigidbody2D rigidbody;
    public float floatHeight;
    public Animator animator;
    public HealthManager healthManager;

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
    public float attackUpdateCooldown = 0.5f;
    public float attackRange = 3;

    private float attackUpdate;
    private bool cooldownJumpInput;
    private float timerJumpInput;
    private float blockCooldown;

    private bool aRelease;
    private bool blocked;

    private int jumpCounter;

    //Animation Data
    private bool moving, move_right, grounded, jumping, double_jumping, prim, prim_air, sec, rescue_move, fly;

	
	// Update is called once per frame
	void Update () {

        resetAnim();

        attackUpdate -= Time.deltaTime;
        if (attackUpdate < 0)
            attackUpdate = 0;

        blockCooldown -= Time.deltaTime;
        if (blockCooldown < 0)
            blockCooldown = 0;


        if (GetComponent<HealthManager>().shieldActivated)
            return;

        if (InputReader.getInput(controller, InputReader.ControlType.B_BTN) == 1 && isNearGround() && secondary())
        {
            manageAnimation();
            rigidbody.velocity = new Vector2();
            return;
        }


        if (InputReader.getInput(controller, InputReader.ControlType.X_BTN) == 1)
        {
            if (isNearGround())
            {
                a_ground();
            }
            else
            {
                a_air();
            }
        }

        grounded = isNearGround();
        Vector2 vel = rigidbody.velocity;
        float value = vel.y > 0 ? gravUp : gravDown;
        fly = vel.y > 0;

        if (isNearGround())
        {
            jumpCounter = 0;
            blocked = false;
        }

        vel.Set(vel.x, isGrounded() ? 0 : vel.y - value * Time.deltaTime);
        if (vel.y < maxFallSpeed)
            vel.Set(vel.x, maxFallSpeed);
        rigidbody.velocity = vel;

        

        if (InputReader.getInput(controller, InputReader.ControlType.HORIZONTAL) == 1)
            moveRight();
        else if (InputReader.getInput(controller, InputReader.ControlType.HORIZONTAL) == -1)
            moveLeft();
        else
            slow();

        handleButtons();

        if (move_right)
            animator.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        else
            animator.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);

        manageAnimation();
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
        move_right = true;
        moving = true;
        if (isGrounded())
            rigidbody.velocity = new Vector2(Mathf.Lerp(rigidbody.velocity.x, speed, groundDamp * Time.deltaTime), rigidbody.velocity.y);
        else
            rigidbody.velocity = new Vector2(Mathf.Lerp(rigidbody.velocity.x, speed, airDamp * Time.deltaTime), rigidbody.velocity.y);
    }

    public virtual void moveLeft()
    {
        move_right = false;
        moving = true;
        if (isGrounded())
            rigidbody.velocity = new Vector2(Mathf.Lerp(rigidbody.velocity.x, -speed, groundDamp * Time.deltaTime), rigidbody.velocity.y);
        else
            rigidbody.velocity = new Vector2(Mathf.Lerp(rigidbody.velocity.x, -speed, airDamp * Time.deltaTime), rigidbody.velocity.y);
    }

    public virtual void jump()
    {
        if (isNearGround())
        {
            jumping = true;
            jumpCounter++;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
        }
        else if (jumpCounter < maxJumps && !blocked)
        {
            double_jumping = true;
            jumpCounter++;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, doubleJumpSpeed);
            manageAnimation();
        }
        //Debug.Log("A");
    }

    public virtual bool a_ground()
    {
        if (attackUpdate == 0)
        {
            prim = true;
            attackUpdate = attackUpdateCooldown;
            Vector2 dir = new Vector2(animator.gameObject.transform.rotation.y == 0 ? 1 : -1, 0);
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dir, attackRange, 1 << LayerHelper.getLayer(LayerHelper.Layer.HITABLE));
            for (int i = 0; i < hit.Length; i++)
            {
                GameObject gameObject = hit[i].collider.gameObject;
                if (gameObject.transform.parent.gameObject != this.gameObject && gameObject.transform.parent.name.StartsWith("Player"))
                {
                    gameObject.transform.GetComponentInParent<HealthManager>().applyDamageFixedY(this.transform.position + new Vector3(0, -0.5f, 0), 2, 20);
                    gameObject.transform.GetComponentInParent<HealthManager>().stun(0.5f);
                }
            }
            return true;
      
        }
        return false;
    }

    public virtual bool a_air()
    {
        if (attackUpdate == 0 && !blocked)
        {
            prim_air = true;
            attackUpdate = attackUpdateCooldown;
            Vector2 dir = new Vector2(animator.gameObject.transform.rotation.y == 0 ? 1 : -1, 0);
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dir, attackRange, 1 << LayerHelper.getLayer(LayerHelper.Layer.HITABLE));
            for (int i = 0; i < hit.Length; i++)
            {
                GameObject gameObject = hit[i].collider.gameObject;
                if (gameObject.transform.parent.gameObject != this.gameObject && gameObject.transform.parent.name.StartsWith("Player"))
                {
                    gameObject.transform.GetComponentInParent<HealthManager>().applyDamageFixedY(this.transform.position + new Vector3(0, -0.5f, 0), 2, 20);
                    gameObject.transform.GetComponentInParent<HealthManager>().stun(0.5f);
                }
            }
            return true;

        }
        return false;
    }

    public virtual bool secondary()
    {
        if (blockCooldown == 0) {
            sec = true;
            blockCooldown = 4;
            StartCoroutine(StartBlock());
            return true;
        }
        return false;
    }

    public IEnumerator StartBlock()
    {
        GetComponent<HealthManager>().shieldActivated = true;
        yield return new WaitForSeconds(1);
        GetComponent<HealthManager>().shieldActivated = false;
    }

    public virtual void save()
    {
        Vector2 vector = findFreeLocation(this.transform.position, saveDistance);
        this.transform.position = this.transform.position = new Vector3(vector.x, vector.y, 0);
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
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

    public void resetAnim() {
        moving = false;
        grounded = false;
        jumping = false;
        double_jumping = false;
        prim = false;
        prim_air = false;
        sec = false;
        rescue_move = false;
        fly = false;
    }
    public void manageAnimation() {
        //Debug.Log(double_jumping);
        //Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        string clipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if (double_jumping)
        {
            animator.Play("DoubleJump");
            return;
        }
        if (jumping)
        {
            animator.Play("Jump");
            return;
        }

        {
            if (clipName.Equals("Attack"))
                return;
            if (clipName.Equals("AttackAir"))
                return;
            if (clipName.Equals("DoubleJump"))
                return;
            if (clipName.Equals("Jump"))
                return;
            if (clipName.Equals("Save"))
                return;
            if (clipName.Equals("Shield") && sec)
                return;
        }
        if (sec)
        {
            animator.Play("Shield");
            return;
        }
        if (rescue_move)
        {
            animator.Play("Save");
            return;
        }
        if (prim_air)
        {
            animator.Play("AttackAir");
            return;
        }
        if (prim) {
            animator.Play("Attack");
            return;
        }
        if (grounded) {
            if (moving && animator.GetComponent<Animation>() == null)
            {
                animator.Play("Run");
                return;
            }
            if (!moving) {
                animator.Play("Idle");
                return;
            }
        }
        if (!grounded)
        {
            if (fly && !clipName.Equals("Fly"))
            {
                animator.Play("Fly");
                return;
            }
            if (!fly && !clipName.Equals("Fall"))
            {
                animator.Play("Fall");
                return;
            }
        }
    }
}
