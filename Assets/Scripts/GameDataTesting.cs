using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataTesting : MonoBehaviour {

    public PlayerData[] playerData;
    private void Awake()
    {
        GameData gameData = new GameData();

        gameData.player1 = playerData[0];
        gameData.player2 = playerData[1];
        gameData.player3 = playerData[2];
        gameData.player4 = playerData[3];
    }
}
