using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	public bool close, mid, far;
	float speed;

	public PlayerController player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		if (close)
			speed = 200F;
		else if (mid) 
			speed = 100;
		else 
			speed = 50f;
	}
	
	// Update is called once per frame
	void Update () {

		transform.position += new Vector3 (player.currentSpeed * speed / ( 25 * transform.position.z) * Time.deltaTime, 0,0);

	}

	void FixedUpdate () {


	}
}
