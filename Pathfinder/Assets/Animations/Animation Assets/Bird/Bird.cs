using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {
	// Use this for initialization
	void Awake () {
        GameObject Bird = Instantiate(Resources.Load("Bird")) as GameObject;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
