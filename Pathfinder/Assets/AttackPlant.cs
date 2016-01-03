using UnityEngine;
using System.Collections;

public class AttackPlant : MonoBehaviour {
	public Transform player;

	public float delay;
	bool inRange;

	public float distance;
	float timer;

	GameObject plant;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		timer = delay;
	}
	
	// Update is called once per frame
	void Update () {
		distance = Vector2.Distance (transform.position, player.position);
		if (distance < 10) {
			inRange = true;
		} else {
			inRange = false;
		}

		if (timer > 0 && inRange)
			timer -= Time.deltaTime;

		if (timer < 0) {
			//animation
			if (distance < 2) {
				player.GetComponent<HealthEffect> ().ReduceHealth ();
				timer = delay;
			}
		}
	}
}