using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

	public Vector3 upPos, downPos;

	// Use this for initialization
	void Start () {
	
		upPos = transform.position + Vector3.up;
		downPos = transform.position + Vector3.down;

	}

	public bool moveUp;
	// Update is called once per frame
	void Update () {
		if (moveUp) {
			transform.position = Vector3.MoveTowards(transform.position, upPos, 1 * Time.deltaTime);
		} else {
			transform.position = Vector3.MoveTowards(transform.position, downPos, 1 * Time.deltaTime);
		}

		if (transform.position == upPos)
			moveUp = false;
		else if (transform.position == downPos)
			moveUp = true;
	}
}
