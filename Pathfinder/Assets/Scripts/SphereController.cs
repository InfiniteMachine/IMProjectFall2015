using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SphereController : MonoBehaviour {

    public bool use2dCasts;

	public Animator animRef;

    public float angleIncrement;
    public float scalar;
    public Vector3 grip;
    public float gravity;
    public LayerMask layers;

    public float walkVelocity = 10f;
    public float flightVelocity = 10f;
    public Platformer.Acceleration acceleration;
    public Platformer.Acceleration airControl;

    public Vector3 velocity = Vector3.zero;
    public Vector3 desiredVelocity;
	public float thegrip;
	public Vector3 startPos;
    int lastSmallestIndex = -1;

    bool applyGravity = true;
    bool grounded1 = false;
    bool grounded2 = false;

    int spacebar = 0;
    int spacebarSet = 7;
    int spacebarDown = 0;
    int spacebarDownSet = 7;

    float hovertime = 0f;

    public Platformer.AnalogStick horStick;
    public Platformer.AnalogStick verStick;
    public Platformer.Jumpset jumpset;
    int nextJumpIndex = 9999;
    Platformer.Jump currentJump;
    bool jumping = false;
    float currentJumpTime;

    float frameTime;
    float fps = 60f;
    public float timeCount = 0f;

    int casts;
    Vector3[] directions;
    float[] distances;
    float[] angles;
    public bool debugBreakOnFall = false;
    public bool active = true;
	Vector3 scale;

	public CameraFollow camRef;
    
	// Use this for initialization
	void Awake() {
		scale = transform.localScale;
		animRef = GetComponentInChildren<Animator> ();

		frameTime = 1f / fps;

		casts = Mathf.RoundToInt(360f / angleIncrement);
		directions = new Vector3[casts];
		distances = new float[casts];
		angles = new float[casts];
		
		float angle;
		
		for(int x=0;x<casts;x++)
		{
			angle = x * angleIncrement;
			angles[x] = angle;
			directions[x] = new Vector3(Mathf.Cos(angle*Mathf.Deg2Rad),
			                            Mathf.Sin(angle*Mathf.Deg2Rad), 0f);
			distances[x] = 0f;
		}
	}

	void Update ()
	{
		if (spacebar > 0)
			spacebar += -1;
		if (spacebarDown > 0)
			spacebarDown += -1;

		if(active)
		{
			if(Input.GetKey(KeyCode.Space))
				spacebar = spacebarSet;
			if(Input.GetKeyDown(KeyCode.Space))
				spacebarDown = spacebarDownSet;
		}

		frameTime = Time.deltaTime;
		if (frameTime > 0.034f)
			frameTime = 0.034f;
		UpdateCall ();
		if (velocity.x > 0.3f)
			transform.localScale = scale;
		else if (velocity.x < -0.3f)
			transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
	}

	// Update is called once per frame
	void UpdateCall () {
		//Debug.Log (velocity.magnitude);
		Vector3 startPosition = transform.position;
		startPos = startPosition;
		desiredVelocity = Vector3.zero;
		float gravityEffect = -gravity * frameTime;
		float currentGrip;

		float lastGrip = findGrip (lastSmallestIndex);

		/*if(lastGrip>0f)
		{
			float hor = horStick.returnThrottle();
			if(hor<0f)
				desiredVelocity = new Vector3(Mathf.Cos((angles[lastSmallestIndex]+90f)*Mathf.Deg2Rad),
				                              Mathf.Sin ((angles[lastSmallestIndex]+90f)*Mathf.Deg2Rad),0f)
					* hor * walkVelocity;
			else if(hor>0f)
				desiredVelocity = new Vector3(Mathf.Cos((angles[lastSmallestIndex]+270f)*Mathf.Deg2Rad),
				                              Mathf.Sin ((angles[lastSmallestIndex]+270f)*Mathf.Deg2Rad),0f)
					* -hor * walkVelocity;
			//if(desiredVelocity!=Vector3.zero)
			//	Debug.Log(desiredVelocity);
		}
		else if(!grounded2)
		{
			// Get desired velocity for flying
			desiredVelocity = new Vector3(horStick.returnThrottle() * 9999f, verStick.returnThrottle() * 9999f, 0f);
		}*/

		//Debug.Log (desiredVelocity);

		//drawLevel (); 
		//drawDistances ();

		if(grounded2)
		{
			gameObject.GetComponent<Renderer>().material.color = Color.blue;

			if(grounded1 && 
			   lastSmallestIndex!=-1)
			{ // Ground Glue
				//if(velocity==Vector3.zero)
				//	transform.position += new vector3
				transform.position += ((velocity+(directions[lastSmallestIndex]*velocity.magnitude) + new Vector3(0f,gravityEffect*frameTime,0f)))*frameTime;
			}
			else
			{
				transform.position += (velocity + new Vector3(0f,-velocity.magnitude + gravityEffect*frameTime,0f))*frameTime;
			}

			// Desired velocity index
			int dVIndex = -1;

			// 2 Check grounded
			checkDistances ();
			int smallestIndex = findSmallestDistance ();
			if(smallestIndex==-1)
				dVIndex = lastSmallestIndex;
			else dVIndex = smallestIndex;
			lastSmallestIndex = smallestIndex;
			currentGrip = findGrip(smallestIndex);
			thegrip = currentGrip;
			transform.position = startPosition;

			float hor = horStick.returnThrottle();
			if(hor<0f && active)
				desiredVelocity = new Vector3(Mathf.Cos((angles[dVIndex]+90f)*Mathf.Deg2Rad),
				                              Mathf.Sin ((angles[dVIndex]+90f)*Mathf.Deg2Rad),0f)
					* hor * walkVelocity;
			else if(hor>0f && active)
				desiredVelocity = new Vector3(Mathf.Cos((angles[dVIndex]+270f)*Mathf.Deg2Rad),
				                              Mathf.Sin ((angles[dVIndex]+270f)*Mathf.Deg2Rad),0f)
					* -hor * walkVelocity;

			if(currentGrip>0f)
			{
				grounded1 = true;
				float deltaAngle = Mathf.Abs(Vector3.Angle(desiredVelocity, velocity));
				if(desiredVelocity!=Vector3.zero && deltaAngle<90f)
					velocity = Vector3.RotateTowards(velocity, desiredVelocity, (currentGrip*deltaAngle*Mathf.Deg2Rad),0f);
				else if(desiredVelocity==Vector3.zero)
				{
					Vector3 expectedVelocity = velocity;
					if(velocity.x>0f)
						expectedVelocity = new Vector3(Mathf.Cos((angles[dVIndex]+90f)*Mathf.Deg2Rad),
						                              Mathf.Sin ((angles[dVIndex]+90f)*Mathf.Deg2Rad),0f) * velocity.magnitude;
					else if(velocity.x<0f)
						expectedVelocity = new Vector3(Mathf.Cos((angles[dVIndex]+270f)*Mathf.Deg2Rad),
						                              Mathf.Sin ((angles[dVIndex]+270f)*Mathf.Deg2Rad),0f) * velocity.magnitude;
					deltaAngle = Mathf.Abs (Vector3.Angle(expectedVelocity, velocity));
					Vector3 oldVel = velocity;
					velocity = Vector3.RotateTowards(velocity, expectedVelocity, (currentGrip*deltaAngle*Mathf.Deg2Rad),0f);
					if(velocity!=Vector3.zero || oldVel!=Vector3.zero)
						Debug.Log (oldVel + " " + velocity);
				}
			}
			else if(grounded1==false)
			{
				grounded2 = false;
			}
			else 
			{	
				grounded1 = false;
				if(velocity.y>0f)
					velocity.y = gravityEffect;
			}

			if(spacebarDown>0 && grounded2 && nextJumpIndex<jumpset.Length())
			{
				// Jumping here
				grounded1 = grounded2 = false;
				jumping = true;
				currentJump = getJump();
				currentJumpTime = currentJump.time;
				// Apply jump here, from a function
				if(currentJump.hover)
					velocity.y = currentJump.power;
				else
					applyFly(desiredVelocity);
				nextJumpIndex++;
				currentGrip = 0f;
				spacebarDown = 0;
				animSet("jump");
			}
			// Apply translation from velocity
			transform.position += velocity * frameTime;

			clip ();
			clipVelocity(startPosition);
			velocity = Vector3.MoveTowards (velocity, desiredVelocity, acceleration.lerpStrength (Vector3.Distance (velocity, desiredVelocity)) * currentGrip * frameTime);
			if(velocity.magnitude>0.2f && grounded2)
				animSet("walk");
			else if (grounded2)
				animSet("idle");
		}
		else // Not grounded // Falling
		{
			if(debugBreakOnFall)
				Debug.Break();
			gameObject.GetComponent<Renderer>().material.color = Color.red;
			if(!(jumping && currentJump.hover))
				velocity.y += gravityEffect;

			desiredVelocity = new Vector3(horStick.returnThrottle() * flightVelocity, verStick.returnThrottle() * flightVelocity, 0f);
			if(!active)
				desiredVelocity = velocity;


			if(jumping)
			{
				if(spacebar>0)
				{
					if(currentJump.applyOverTime)
						applyFly(desiredVelocity);
	
					currentJumpTime += -frameTime;
					if(currentJumpTime<0f)
						jumping = false;
				}
				else jumping = false;
			}
			else 
			{
				desiredVelocity = velocity;
				if(horStick.returnThrottle()!=0f)
					desiredVelocity.x = horStick.returnThrottle() * walkVelocity;
				velocity = Vector3.MoveTowards(velocity, desiredVelocity, airControl.lerpStrength(Vector3.Distance(velocity, desiredVelocity)) * frameTime);

			}

			// 1 Apply X,Y simultaneously
			transform.position += velocity * frameTime;

			// 2 Check distances, see if grounded.
			checkDistances ();
			int smallestIndex = findSmallestDistance ();
			lastSmallestIndex = smallestIndex;
			currentGrip = findGrip(smallestIndex);

			clip(smallestIndex, 1f);

			// 3 If grip > 0f, is grounded again
			if(currentGrip>0f)
			{
				grounded2 = true;
				grounded1 = true;
				nextJumpIndex = 0;
				jumping = false;
				// Apply grip to velocity
			}

			if(!grounded2 && !jumping && spacebarDown>0 && nextJumpIndex<jumpset.Length())
			{
				// Flying here
				currentJump = getJump();
				currentJumpTime = currentJump.time;
				nextJumpIndex++;
				spacebarDown = 0;
				jumping = true;
				animSet("fly");
			}

			clipVelocity(startPosition);
		}
	}

	void applyFly(Vector3 desiredVelocity)
	{
		if(currentJump.applyOverTime)
		{
			desiredVelocity = new Vector3(horStick.returnThrottle() * flightVelocity, verStick.returnThrottle() * flightVelocity, 0f);
			float mod = 1f;
			if(verStick.returnThrottle()!=0f && horStick.returnThrottle()!=0f)
				mod = Mathf.Sqrt(2)/2f;
			velocity = Vector3.MoveTowards (velocity, mod * desiredVelocity, currentJump.timeAcceleration.lerpStrength(Vector3.Distance(velocity, desiredVelocity)) * frameTime);
		}
	}

	void clip(int smallestIndex, float mag = 1f)
	{
		if(smallestIndex!=-1)
			transform.position += -mag * ((scalar-distances[smallestIndex])*directions[smallestIndex]);
	}

	void clip(float mag = 1f)
	{
		checkDistances ();
		clip (findSmallestDistance (), mag);
	}

	void clipVelocity(Vector3 startPosition)
	{
		Vector3 clipPosition = transform.position;
		Vector3 clipVector = (clipPosition - startPosition) / frameTime;
		Vector3 oldVelocityVector = velocity;
		float magnitudeFactor = 1f;
		if(clipVector!=Vector3.zero && oldVelocityVector!=Vector3.zero)
			magnitudeFactor = Mathf.Cos(Vector3.Angle(clipVector, oldVelocityVector)*Mathf.Deg2Rad);
		if(magnitudeFactor<0f)
			magnitudeFactor = 0f;
		magnitudeFactor = magnitudeFactor * clipVector.magnitude;
		clipVector.Normalize ();
		velocity = magnitudeFactor * clipVector;
	}
	
	void checkDistances()
	{
		bool skipped = false;
		if(!use2dCasts)
		{
			distances[0] = returnRayLength(transform.position, directions[0], scalar);

			for(int x=1;x<casts;x++)
			{
				distances[x] = returnRayLength(transform.position, directions[x], scalar);
				if(distances[x]==scalar)
				{
					skipped = true;
					x++;
					if(x<casts)
						distances[x] = scalar;
				}
				else
				{
					if(skipped)
						distances[x-1] = returnRayLength(transform.position, directions[x-1], scalar);
					skipped = false;
				}
			}
		}
		else
		{
			distances[0] = returnRayLength2d(transform.position, directions[0], scalar);
			
			for(int x=1;x<casts;x++)
			{
				distances[x] = returnRayLength2d(transform.position, directions[x], scalar);
				if(distances[x]==scalar)
				{
					skipped = true;
					x++;
					if(x<casts)
						distances[x] = scalar;
				}
				else
				{
					if(skipped)
						distances[x-1] = returnRayLength2d(transform.position, directions[x-1], scalar);
					skipped = false;
				}
			}
		}
		deathCheck ();
	}

	void deathCheck()
	{
		RaycastHit hit;
		Ray ray = new Ray(transform.position, velocity);
		
		//if something under the player
		if (Physics.Raycast(ray, out hit, scalar, layers))
		{
			if(hit.collider.tag=="Death")
				die ();
		}
	}
	
	int findSmallestDistance()
	{
		int smallestIndex = -1;
		float smallestDistance = 1f;
		for(int x=0;x<distances.Length;x++)
		{
			if(distances[x]<smallestDistance)
			{
				smallestDistance = distances[x];
				smallestIndex = x;
			}
		}
		return smallestIndex;
	}
	
	float findGrip(int theIndex)
	{
		float gripAngle;
		if (theIndex == -1)
			return 0f;
		else
			gripAngle = Mathf.Abs(Mathf.DeltaAngle (angles [theIndex], 270f));
		float gripRange = grip.y - grip.x;

		if (gripAngle < grip.x)
			return 1.0f;
		if (gripAngle > grip.y)
			return 0f;
		else return (1f - grip.z) + (grip.z * (gripAngle - grip.x) / gripRange);
	}

	Platformer.Jump getJump()
	{
		return jumpset.jumps [nextJumpIndex];
	}

	public Vector3 returnTangent(int index)
	{
		if (index == -1)
			return Vector3.zero;
		else
			return returnTangent (angles [index]);
	}

	public Vector3 returnTangent(float angle)
	{
		Vector3 toReturn = new Vector3 (Mathf.Cos ((angle + 90f) * Mathf.Deg2Rad), Mathf.Sin ((angle + 90f) * Mathf.Deg2Rad), 0f);
		if (toReturn.x < 0f)
			toReturn = toReturn * -1f;
		return toReturn;
	}

	public void die(string cause = "none")
	{
		// Do death stuff
		GameObject.Find ("GameManager").GetComponent<DelayedRespawn> ().activate ();

		// Despawn
		despawn ();
	}

	public void despawn()
	{
		GameObject.Destroy (gameObject);
	}

	public float returnRayLength(Vector3 position, Vector3 direction, float length = 9999f)
	{
		RaycastHit hit;
		Ray ray = new Ray(position, direction);
		
		//if something under the player
		if (Physics.Raycast(ray, out hit, length, layers))
		{
			hitCondition(hit.collider, 0f);
			return hit.distance;
		}
		return length;
	}

	public float returnRayLength2d(Vector2 position, Vector2 direction, float length = 9999f)
	{
		RaycastHit2D hit = Physics2D.Raycast(position, direction, length, layers);

		if(hit)
		{
			hitCondition2D(hit.collider, 0f);
			return hit.distance;
		}
		return length;
		//return Physics2D.Raycast(position, direction, length, layers).distance;
	}

    void hitCondition2D(Collider2D other, float force = 0f) // Object was hit, here is the collider
    {
        // Jake, here
        // Ignore the force variable
    }

	void hitCondition(Collider other, float force = 0f) // Object was hit, here is the collider
	{
		// Jake, here
		// Ignore the force variable
	}

	void animSet(string command)
	{
		if(command=="walk")
		{
			animRef.SetBool("walking", true);
			animRef.SetBool("idle", false);
			animRef.SetBool("jumping", false);
			animRef.SetBool("flying", false);
		}
		else if(command=="idle")
		{
			animRef.SetBool("walking", false);
			animRef.SetBool("idle", true);
			animRef.SetBool("jumping", false);
			animRef.SetBool("flying", false);
		}
		else if(command=="fly")
		{
			animRef.SetBool("walking", false);
			animRef.SetBool("idle", false);
			animRef.SetBool("jumping", false);
			animRef.SetBool("flying", true);
		}
		else if(command=="jump")
		{
			animRef.SetBool("walking", false);
			animRef.SetBool("idle", false);
			animRef.SetBool("jumping", true);
			animRef.SetBool("flying", false);

		}
	}

	public void drawLevel()
	{
		if (lastSmallestIndex != -1) 
		{
			int newIndex = lastSmallestIndex + directions.Length/2;
			if(newIndex>directions.Length)
				newIndex = newIndex - directions.Length;


			Debug.DrawLine (transform.position + (5f * directions [newIndex]),
			               transform.position + (-5f * directions [newIndex]), Color.blue);
		}
	}

	public void drawDistances()
	{
		for(int x=0;x<distances.Length;x++)
		{
			Debug.DrawLine(transform.position,
			               transform.position + (distances[x] * directions[x]) * 2f,
			               new Color(1f - (distances[x]/scalar), 0f, (distances[x]/scalar)),0f,false);
		}
	}
}
