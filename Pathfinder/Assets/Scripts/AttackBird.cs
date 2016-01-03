using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackBird : MonoBehaviour {
	public Transform player;
	public GameObject leftNest, rightNest, bird;
	public float speed;
	public float delay;
	float timer;
	bool isLeft;
	bool inRange;

	public float distance;

	Vector3 leftNestPos, rightNestPos;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		leftNestPos = leftNest.transform.localPosition;
		leftNestPos.y += 1;
		rightNestPos = rightNest.transform.localPosition;
		rightNestPos.y += 1;
	}
	
	// Update is called once per frame
	void Update () {
		distance = Vector2.Distance (transform.position, player.position);
		if (distance < 10) {
			inRange = true;
		} else {
			inRange = false;
		}

		if (delay > 0)
			delay -= Time.deltaTime;

		if (inRange && delay <= 0) {
			if (isLeft) {
				bird.transform.localPosition = Vector3.MoveTowards(bird.transform.localPosition, rightNestPos, speed * Time.deltaTime);
				if (bird.transform.localPosition == rightNestPos) {
					isLeft = false;
					timer = delay;
				}
			} else {
				bird.transform.localPosition = Vector3.MoveTowards(bird.transform.localPosition, leftNestPos, speed * Time.deltaTime);
				if (bird.transform.localPosition == leftNestPos) {
					isLeft = true;
					timer = delay;
				}
			}
		}
	}
}