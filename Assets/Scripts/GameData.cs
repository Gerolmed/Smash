using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData {

    public static GameData Instance;

    public PlayerData player1, player2, player3, player4;
    public Map nextMap;

    public GameData() {
        Instance = this;
        
    }

}
