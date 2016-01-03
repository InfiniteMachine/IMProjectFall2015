using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackBird : MonoBehaviour {
	public GameObject leftNest, rightNest;
	public float speed;
	public float delay;
	float timer;
	bool isLeft;
	bool inRange;

	Vector3 leftNestPos, rightNestPos;
	// Use this for initialization
	void Start () {
		leftNestPos = leftNest.transform.localPosition;
		leftNestPos.y += 1;
		rightNestPos = rightNest.transform.localPosition;
		rightNestPos.y += 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (delay > 0)
			delay -= Time.deltaTime;

		if (inRange && delay <= 0) {
			if (isLeft) {
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, rightNestPos, speed * Time.deltaTime);
				if (transform.localPosition == rightNestPos) {
					isLeft = false;
					timer = delay;
				}
			}
			else {
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, leftNestPos, speed * Time.deltaTime);
				if (transform.localPosition == leftNestPos) {
					isLeft = true;
					timer = delay;
				}
			}
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag == "Player")
			inRange = true;
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.tag == "Player")
			inRange = false;
	}
}