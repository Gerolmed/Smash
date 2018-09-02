using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour {
    public static StartUp Instance;

    public Vector3[] spawns = new Vector3[4];

	void Awake () {
        Instance = this;

        if (GameData.Instance == null) {
            Debug.LogError("No GameData set!");
            return;
        }
        GameData data = GameData.Instance;
        if (data.player1 != null) {
            Vector3 spawn = spawns[0];

            GameObject player = GameObject.Instantiate(data.player1.ingameObject);
            player.name = "Player1";
            player.transform.position = spawn;
        }

	}
}
