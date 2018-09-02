using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Smash/Player", order = 1)]
public class PlayerData : ScriptableObject {
    public string name;
    public Sprite preview;
    public GameObject ingameObject;

}
