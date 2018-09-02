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
            rigidbody.velocity = new Vector2(rigidbody.velocity.x + pushBack, Mathf.Lerp(rigidbody.velocity.y, slideSpeed, slideAlpha));
            jumpCounter = 0;
        }
	}

    public override void save()
    {
        jump();
    }

    public bool isNearWall(float Width, float rotation)
    {
        if (rotation == 180)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, rayLengthExtended, LayerHelper.getLayers(new LayerHelper.Layer[] { LayerHelper.Layer.GROUND, LayerHelper.Layer.GOUND_NO_COLLISION }));
            if (hit.collider != null)
            {
                return true;
            }
        }
        else if (rotation == 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, rayLengthExtended, LayerHelper.getLayers(new LayerHelper.Layer[] { LayerHelper.Layer.GROUND, LayerHelper.Layer.GOUND_NO_COLLISION }));
            if (hit.collider != null)
            {
                return true;
            }
            pushBack = -pushBack;
        }
        return false;
    }
}
