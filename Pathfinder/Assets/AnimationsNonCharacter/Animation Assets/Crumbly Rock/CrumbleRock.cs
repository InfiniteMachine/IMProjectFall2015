using UnityEngine;
using System.Collections;

public class CrumbleRock : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        GameObject CrumblyRock = Instantiate(Resources.Load("Crumbly Rock")) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
