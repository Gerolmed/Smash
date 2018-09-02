using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour {
    public static StartUp Instance;

    public Transform[] spawns = new Transform[4];

	void Awake () {
        Instance = this;

        if (GameData.Instance == null) {
            Debug.LogError("No GameData set!");
            return;
        }
        GameData data = GameData.Instance;
        //player 0
        if (data.player1 != null) {
            Vector3 spawn = spawns[0].position;

            GameObject player = GameObject.Instantiate(data.player1.ingameObject);
            player.name = "Player1";
            player.transform.position = spawn;
            player.GetComponent<Holder>().character.controller = InputReader.Controller.ONE;
            GetComponent<PlayerReference>().player1 = player;
        }
        //player 1
        if (data.player2 != null)
        {
            Vector3 spawn = spawns[1].position;

            GameObject player = GameObject.Instantiate(data.player2.ingameObject);
            player.name = "Player2";
            player.transform.position = spawn;
            player.GetComponent<Holder>().character.controller = InputReader.Controller.TWO;
            GetComponent<PlayerReference>().player2 = player;
        }
        //player2
        if (data.player3 != null)
        {
            Vector3 spawn = spawns[2].position;

            GameObject player = GameObject.Instantiate(data.player3.ingameObject);
            player.name = "Player3";
            player.transform.position = spawn;
            player.GetComponent<Holder>().character.controller = InputReader.Controller.THREE;
            GetComponent<PlayerReference>().player3 = player;
        }
        //player3
        if (data.player4 != null)
        {
            Vector3 spawn = spawns[3].position;

            GameObject player = GameObject.Instantiate(data.player4.ingameObject);
            player.name = "Player4";
            player.transform.position = spawn;
            player.GetComponent<Holder>().character.controller = InputReader.Controller.FOUR;
            GetComponent<PlayerReference>().player4 = player;
        }

    }
}
