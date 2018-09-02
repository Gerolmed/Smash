using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour {
    public static StartUp Instance;

	void Awake () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
