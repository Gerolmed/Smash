using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    public int damage;
    public float baseMultiplier;
    public int lives = 1;

    public void applyDamage(Vector3 origin, float damageVal)
    {
        Debug.Log("Something happened here");
        //TODO: get Damage
    }
}
