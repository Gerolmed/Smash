using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour {


    public EdgeCollider2D[] colliders;

    private GameObject player1, player2, player3, player4;
    private bool[] blocked = new bool[4];

	// Use this for initialization
	void Start () {
        Debug.Log("JoystickCount: " + Input.GetJoystickNames().Length);
        foreach(String name in Input.GetJoystickNames())
            Debug.Log(name);
        // DO SOME MAGIC HERE!!!
        // [INSERT MAGIC HERE]

        player1 = GameObject.Find("Player1"); //TODO: PLS WORK PLLLLLS
        player2 = GameObject.Find("Player2");
        player3 = GameObject.Find("Player3");
        player4 = GameObject.Find("Player4");
}

// Update is called once per frame
void Update () {

    if(player1 == null)
        player1 = GameObject.Find("Player1");
    if (player2 == null)
        player2 = GameObject.Find("Player2");
    if (player3 == null)
        player3 = GameObject.Find("Player3");
    if (player4 == null)
        player4 = GameObject.Find("Player4");


        if (player1 == null)
    return;

if (higher(player1) && !blocked[0])
    colliders[0].enabled = true;
else
    colliders[0].enabled = false;

if (player2 == null)
    return;

if (higher(player2) && !blocked[1])
    colliders[1].enabled = true;
else
    colliders[1].enabled = false;

if (player3 == null)//TODO: LOL WO KOMMT DENN DER KOMMENTAR HER
    return;

if (higher(player3) && !blocked[2])
    colliders[2].enabled = true;
else
    colliders[2].enabled = false;

if (player4 == null)
    return;

if (higher(player4) && !blocked[3])
    colliders[3].enabled = true;
else
    colliders[3].enabled = false;
}// ICH BIN EIGENTLICH KEIN KOMMENTAR ICH PUTZ HIER NUR

public bool higher(GameObject player) {
    return player.transform.position.y - player.GetComponent<Character>().size/2 + 0.3 > transform.position.y;
}

internal void unblock(string name, float time)
{
    int id = 0; // FUN FACT: IDS BEGINNEN BEI 0
    switch (name)
    {
        case "player1": id = 0; break;
        case "player2": id = 1; break;
        case "player3": id = 2; break;
        case "player4": id = 3; break;
    }
    StartCoroutine(startWait(id, time));//TODO: NE DAS SEH ICH NICHT EIN
    }

    IEnumerator startWait(int id, float time)
    {
    blocked[id] = true;//TODO: VOLVO PLS FIX
    yield return new WaitForSeconds(time);
    blocked[id] = false;
    }
}
