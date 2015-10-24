using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

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
	float currentFlightTime;
	public float glideTime;
	float currentGlideTime;

	//[HideInInspector]
	public bool isGroundLeft, isGroundRight, jumping;

	public bool leftDown, rightDown;
	bool canFly, canDigDown, canDigLeft, canDigRight;
	GameObject objectToDig;
	Rigidbody2D playerRigidbody;
	// Use this for initialization
	void Start () {
		playerRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate () {

		if (isGrounded) 
			playerRigidbody.gravityScale = 1;
		else 
			playerRigidbody.gravityScale = 5;

		// left right movement 
		// -----------------------------------------------------------------------------------------
		if (Input.GetKey (KeyCode.A) && !isGroundLeft) {
			leftDown = true;
			rightDown = false;
			if (currentSpeed >= -maxSpeed)
				currentSpeed -= Time.deltaTime * accel;

			playerRigidbody.velocity = new Vector2(currentSpeed, playerRigidbody.velocity.y);
		} else if (Input.GetKey (KeyCode.D) && !isGroundRight) {
			rightDown = true;
			leftDown = false;
			if (currentSpeed <= maxSpeed)
				currentSpeed += Time.deltaTime * accel;

			playerRigidbody.velocity = new Vector2(currentSpeed, playerRigidbody.velocity.y);
		}
		else  {
			if(Mathf.Abs (currentSpeed) > 0.1f){
				currentSpeed = Mathf.Max (0, Mathf.Abs (currentSpeed) - 0.5f) * ((currentSpeed > 0) ? 1 : -1);
			}
			playerRigidbody.velocity = new Vector2(currentSpeed, playerRigidbody.velocity.y);
		}

		//-------------------------------------------------------------------------------------------


		//flight movement
		//-------------------------------------------------------------------------------------------
//		if (Input.GetKey (KeyCode.Space) && canFly && currentFlightTime > 0f) {
//			currentFlightTime -= Time.fixedDeltaTime;
//			playerRigidbody.velocity += new Vector2(0, liftPower);
//			playerRigidbody.gravityScale = 2f;
//		}
//
//		if (Input.GetKey (KeyCode.Space) && currentFlightTime < 0 && currentGlideTime > 0f) {
//			currentGlideTime -= Time.fixedDeltaTime;
//			if (Input.GetKey (KeyCode.A)) {
//				playerRigidbody.velocity = new Vector2 (currentSpeed * glideSpeed, playerRigidbody.velocity.y);
//			} else if (Input.GetKey (KeyCode.D)) {
//				playerRigidbody.velocity = new Vector2 (currentSpeed * glideSpeed, playerRigidbody.velocity.y);
//			}
//			playerRigidbody.gravityScale = .55f;
//		}

		if (Input.GetKeyDown (KeyCode.Space) && isGrounded) {
			Invoke("StartFly", .25f);
			jumped = true;
		}

		if (jumped == true) {
			playerRigidbody.velocity = Vector2.up * jumpPower;
			jumped = false;
		}
		//-------------------------------------------------------------------------------------------

		//Digging Controls
		//-------------------------------------------------------------------------------------------
		if (Input.GetKeyDown (KeyCode.S) && canDigDown ) {
			DigDown();
		} else if (Input.GetKeyDown (KeyCode.A) && canDigLeft) {
			DigLeft();
		} else if (Input.GetKeyDown (KeyCode.D) && canDigRight) {
			DigRight();
		}
		//-------------------------------------------------------------------------------------------

	}

	void StartFly () {
		canFly = true;
		jumped = false;
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
			canFly = false;
			jumped = false;
			currentFlightTime = flightTime;
			currentGlideTime = glideTime;
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
		Invoke ("LeftGround", .25f);
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
			Destroy(col.gameObject);
		}
	}

	void LeftGround () {
		isGrounded = false;
		canFly = true;
	}
}