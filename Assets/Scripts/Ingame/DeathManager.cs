using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour {

    private GameObject player1, player2, player3, player4;
    public Vector3 mid;
    public float maxX = 50;
    public float maxY = 50;

    // Update is called once per frame
    void Update () {
        if (player1 == null)
            player1 = GameObject.Find("Player1");
        if (player2 == null)
            player2 = GameObject.Find("Player2");
        if (player3 == null)
            player3 = GameObject.Find("Player3");
        if (player4 == null)
            player4 = GameObject.Find("Player4");

        if (player1 != null && player1.activeSelf) {
            checkForDeath(player1);
        }
        if (player2 != null && player2.activeSelf)
        {
            checkForDeath(player2);
        }
        if (player3 != null && player3.activeSelf)
        {
            checkForDeath(player3);
        }
        if (player4 != null && player4.activeSelf)
        {
            checkForDeath(player4);
        }
    }

    private void checkForDeath(GameObject player)
    {
        Vector3 position = player.transform.position;
        Vector3 dist = position - mid;
        if (Math.Abs(dist.y) >= maxY || Math.Abs(dist.x) >= maxX) {
            player.GetComponent<HealthManager>().die();
        }
    }
}
