using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour {

    GameObject Player;
    GameObject RespawnPt;
    int minY = -15;

	// Use this for initialization
	void Awake () {
	//use the ridiculous death box in the future assumably, but for now, this'll do

        Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

        if (Player.transform.position.y < minY)
        {
            RespawnPt = GameObject.FindGameObjectWithTag("Respawn");
            transform.position = RespawnPt.transform.position;
        }
	}
}
