using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    public int damage;
    public float baseMultiplier;
    public float startboost=20;
    public int lives = 1;

    private Rigidbody2D rigidbody;

    [HideInInspector]
    public bool shieldActivated;
    public int stunned;

    public void applyDamage(Vector3 origin, int damageVal)
    {
        if (shieldActivated)
            return;
        Vector3 dir = (this.transform.position - origin).normalized;
        rigidbody.velocity = rigidbody.velocity + new Vector2(dir.x, dir.y) * getMultiplier();
        damage += damageVal;
    }

    public void applyDamageFixedY(Vector3 origin, int damageVal, float fixedY)
    {
        if (shieldActivated)
            return;
        Vector3 dir = (this.transform.position - origin).normalized;
        rigidbody.velocity = rigidbody.velocity + new Vector2(dir.x, dir.y) * getMultiplier();
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, fixedY);
        damage += damageVal;
    }

    public IEnumerator stun(float time)
    {
        yield return new WaitForSeconds(time);
    }

    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
    }

    private float getMultiplier()
    {
        return baseMultiplier * (damage + startboost);
    }

    internal void die()
    {
        this.gameObject.SetActive(false);
        lives--;
        damage = 0;
        if (lives <= 0) {
            defeat();
            return;
        }
        SpawnScript.Instance.spawnObject(this.gameObject, true);
        Debug.Log("Respawn");

    }

    private void defeat()
    {
        Debug.Log("Defeat");
    }
}
