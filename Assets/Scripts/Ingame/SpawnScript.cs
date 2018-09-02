using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    public GameObject laserBeam;
    public static SpawnScript Instance;
    public Transform[] positions;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        spawnObject(GameObject.Find("System").GetComponent<PlayerReference>().player1, false);
        spawnObject(GameObject.Find("System").GetComponent<PlayerReference>().player2, false);
        spawnObject(GameObject.Find("System").GetComponent<PlayerReference>().player3, false);
        spawnObject(GameObject.Find("System").GetComponent<PlayerReference>().player4, false);
    }

    public void spawnObject(GameObject gameObject, bool rndPos) {
        if (gameObject == null) {
            Debug.Log("No player found!");
            return;
        }
        StartCoroutine(spawn(gameObject, rndPos));
    }

    private IEnumerator spawn(GameObject gameObject, bool rndPos) {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(4);
        GameObject beam = GameObject.Instantiate(laserBeam);

        Vector3 vector;

        if (rndPos)
            vector = getRndPos();
        else
            vector = gameObject.transform.position;

        gameObject.transform.position = vector;
        beam.transform.position = vector;
        yield return new WaitForSeconds(0.5f);
        gameObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        GameObject.Destroy(beam); 
    }

    private Vector3 getRndPos()
    {
        return positions[0].position;
    }
}
