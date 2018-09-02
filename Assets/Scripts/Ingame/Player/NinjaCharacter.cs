﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaCharacter : Character {
	
    public float slideSpeed = -3;
    public float slideAlpha = 3;
    public float pushBack = 5;

	// Update is called once per frame
	protected override void Update () {
        base.Update();
        if (isNearWall(width / 2, animator.gameObject.transform.rotation.y))
        {
            jumpCounter = 0;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, Mathf.Lerp(rigidbody.velocity.y, slideSpeed, slideAlpha));
        }
    }

    public override void save()
    {
        jump();
    }

    public override void jump()
    {
        base.jump();
        /*if (isNearWall(width / 2, animator.gameObject.transform.rotation.y))
        {
            rigidbody.velocity = new Vector2(Mathf.Lerp(rigidbody.velocity.y, slideSpeed, slideAlpha), rigidbody.velocity.y);
        }*/
    }
   
    public bool isNearWall(float Width, float rotation)
    {
        if (rotation == 180)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, -rayLengthExtended, LayerHelper.getLayers(new LayerHelper.Layer[] { LayerHelper.Layer.GROUND, LayerHelper.Layer.GOUND_NO_COLLISION }));
            if (hit.collider != null)
            {
                return true;
            }
            pushBack = Math.Abs(pushBack);
        }
        else if (rotation == 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, rayLengthExtended, LayerHelper.getLayers(new LayerHelper.Layer[] { LayerHelper.Layer.GROUND, LayerHelper.Layer.GOUND_NO_COLLISION }));
            if (hit.collider != null)
            {
                return true;
            }
            pushBack = -Math.Abs(pushBack);
        }
        return false;
    }
}
