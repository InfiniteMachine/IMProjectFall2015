﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public Vector3 leftFaceing = new Vector3(0,180,0);
	public Vector3 rightFacing = new Vector3(0,0,0);

	public int skillPoints;
	public float maxSpeed;
	public float currentSpeed;
	public float accel;
	bool isGrounded;

	public float jumpPower;
	bool jumped;
	public float liftPower;
	public float glideSpeed; //multiplicative
	public float flightTime;
	public float currentFlightTime;
	public float glideTime;
	public float currentGlideTime;

	[HideInInspector]
	public bool isGroundLeft, isGroundRight;

	public bool leftDown, rightDown;
	[HideInInspector] public bool canFly, canDigDown, canDigLeft, canDigRight;
	GameObject objectToDig;
	Rigidbody2D playerRigidbody;
	SpriteRenderer sprite;
	public Animator playerAni;
	Vector3 cheackPoint;

	// Use this for initialization
	void Start () {
		cheackPoint = transform.position;
		playerAni = GetComponentInChildren<Animator> ();
		playerRigidbody = GetComponent<Rigidbody2D>();
		sprite = GetComponentInChildren<SpriteRenderer>();
	}

	bool gotFirstFood = true;
	void FixedUpdate () {

		if (isGrounded) 
			playerRigidbody.gravityScale = 2;
		else {
			playerRigidbody.gravityScale = 7;
		}

		// left right movement 
		// -----------------------------------------------------------------------------------------
		if (Input.GetKey (KeyCode.A) && !isGroundLeft) {
			playerAni.SetBool("walking", true);

			transform.localEulerAngles = leftFaceing;
			leftDown = true;
			rightDown = false;
			if (currentSpeed >= -maxSpeed)
				currentSpeed -= Time.deltaTime * accel;

			playerRigidbody.velocity = new Vector2(currentSpeed, playerRigidbody.velocity.y);
		} else if (Input.GetKey (KeyCode.D) && !isGroundRight) {
			playerAni.SetBool("walking", true);

			transform.localEulerAngles = rightFacing;
			rightDown = true;
			leftDown = false;
			if (currentSpeed <= maxSpeed)
				currentSpeed += Time.deltaTime * accel;

			playerRigidbody.velocity = new Vector2(currentSpeed, playerRigidbody.velocity.y);
		}
		else  {
			playerAni.SetBool("walking", false);
			playerAni.SetBool("Idle", true);

			if(Mathf.Abs (currentSpeed) > 0.1f){
				currentSpeed = Mathf.Max (0, Mathf.Abs (currentSpeed) - 0.5f) * ((currentSpeed > 0) ? 1 : -1);
			}
			playerRigidbody.velocity = new Vector2(currentSpeed, playerRigidbody.velocity.y);
		}
		//-------------------------------------------------------------------------------------------


		//flight movement
		//-------------------------------------------------------------------------------------------
		if (skillPoints > 1) {
			if (Input.GetKey (KeyCode.Space) && canFly && currentFlightTime > 0f) {
				//playerAni.SetBool("Flying", true);
				//playerAni.SetBool("jumping", false);
				currentFlightTime -= Time.fixedDeltaTime;
				playerRigidbody.velocity += new Vector2(0, liftPower);
				playerRigidbody.gravityScale = 2f;
			}
			//else {playerAni.SetBool("Flying", false);}

			if (Input.GetKey (KeyCode.Space) && currentFlightTime < 0 && currentGlideTime > 0f) {
				currentGlideTime -= Time.fixedDeltaTime;
				if (Input.GetKey (KeyCode.A)) {
					playerRigidbody.velocity = new Vector2 (currentSpeed * glideSpeed, playerRigidbody.velocity.y);
				} else if (Input.GetKey (KeyCode.D)) {
					playerRigidbody.velocity = new Vector2 (currentSpeed * glideSpeed, playerRigidbody.velocity.y);
				}
				playerRigidbody.gravityScale = .55f;
			}
		}
		if (gotFirstFood) {
			if (Input.GetKeyDown (KeyCode.Space) && isGrounded && !jumped) {
				//playerAni.SetBool("jumping", true);
				playerAni.SetBool("walking", false);
				jumped = true;
				Invoke ("StartFly", .15f);
				playerRigidbody.velocity = new Vector2 (playerRigidbody.velocity.x, jumpPower);
			}
		}
		//-------------------------------------------------------------------------------------------

		//Digging Controls
		//-------------------------------------------------------------------------------------------
		if (skillPoints > 2) {
			if (Input.GetKeyDown (KeyCode.S) && canDigDown ) {
				DigDown();
			} else if (Input.GetKeyDown (KeyCode.A) && canDigLeft) {
				DigLeft();
			} else if (Input.GetKeyDown (KeyCode.D) && canDigRight) {
				DigRight();
			}
		}
		//-------------------------------------------------------------------------------------------

	}

	void StartFly () {
		canFly = true;
	}

	void DigDown () {
		Destroy (objectToDig, .25f);
		Invoke ("LeftGround", .26f);
	}
	void DigLeft () {
		Destroy (objectToDig, .25f);
		Invoke ("LeftGround", .26f);
	}
	void DigRight () {
		Destroy (objectToDig, .25f);
		Invoke ("LeftGround", .26f);
	}

	void OnCollisionStay2D (Collision2D col) {
		ContactPoint2D contact = col.contacts [0];
		if (Vector3.Dot(contact.normal, Vector2.up) > .5f) {
			isGrounded = true;
			if (!jumped) {
				canFly = false;
			}
			jumped = false;
			currentFlightTime = flightTime;
			currentGlideTime = glideTime;
//			playerAni.SetBool("jumping", false);
//			playerAni.SetBool("flying", false);
		}

		if (col.transform.tag == "BreakableTerrain") {
			if (Vector3.Dot(contact.normal, Vector2.up) > .5f) {
				canDigDown = true;
			} else if (Vector3.Dot(contact.normal, Vector2.left) > .5f) {
				canDigRight = true;
			} else {
				canDigLeft = true;
			}
			objectToDig = col.gameObject;
		}
	}

	void OnCollisionExit2D (Collision2D col) {
		if (jumped) {
			Invoke ("LeftGround",.15f);
		} else 
			Invoke ("LeftGround",.2f);
		if (col.transform.tag == "BreakableTerrain") {
			canDigDown = false;
			canDigLeft = false; 
			canDigRight = false;
			objectToDig = null;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag == "Food") {
			skillPoints += 1;
			if (!gotFirstFood) {
				maxSpeed += 15;
				gotFirstFood = true;
			}
			Destroy(col.gameObject);
		}

		if (col.tag == "KillBox") {
			transform.position = cheackPoint;
		}
	}

	void LeftGround () {
		isGrounded = false;
		canFly = true;
	}
}