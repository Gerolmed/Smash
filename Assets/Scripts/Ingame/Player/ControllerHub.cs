using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ControllerHub : MonoBehaviour {

    public static ControllerHub Instance;
    public ArrayList a = new ArrayList();

    void Awake()
    {
        Instance = this;
    }

    void verifyControllers()
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerIndex testPlayerIndex = (PlayerIndex)i;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            if (testState.IsConnected)
            {
                if (!a.Contains(testPlayerIndex))
                    a.Add(testPlayerIndex);
            }
            else if (a.Contains(testPlayerIndex))
                a.Remove(testPlayerIndex);
        }
    }

	// Use this for initialization
	void Start () {
        verifyControllers();
	}
	
	// Update is called once per frame
	void Update () {
        verifyControllers();
    }

    public PlayerIndex[] GetControls()
    {
        PlayerIndex[] array = new PlayerIndex[4];
        for (int i = 0; i < 4; i++)
        {
            if (a.Count > i)
            {
                if (a[i] != null)
                {
                    array[i] = (PlayerIndex) a[i];
                }
            }
        }
        return array;
    }
}
