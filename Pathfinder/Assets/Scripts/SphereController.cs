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

	public float debugHorThrottle;

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
	const float groundBufferFactor = 1.005f;
	public bool debugBreakOnFall = false;
	public bool active = true;
	Vector3 scale;
	Vector3 spawnPosition;

	public float globalGrip = 0f;

	public CameraFollow camRef;
	
	int frameNumber = 0;
	int count = 0;

	public int gripIndexMin;
	public int gripIndexMax;

	// Use this for initialization
	void Awake() {
		spawnPosition = transform.position;
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

		for(int x=0;x<casts;x++)
		{
			if(findGrip(x)>0f)
			{
				gripIndexMin = x-1;
				for(int y=x; y<casts;y++)
				{
					if(findGrip(y)==0f)
					{
						gripIndexMax = y;
						break;
					}
				}
				break;
			}
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
		count = 0;
		frameNumber += 1;
		//Debug.Log (velocity.magnitude);
		Vector3 startPosition = transform.position;
		startPos = startPosition;
		desiredVelocity = Vector3.zero;
		float gravityEffect = -gravity * frameTime;
		float currentGrip;

		globalGrip = 0f;
		
		//float lastGrip = findGrip (lastSmallestIndex);
		
		//Debug.Log (desiredVelocity);
		
		//drawLevel (); 
		//drawDistances ();
		
		if(grounded2)
		{
			// Bug starts here
			gameObject.GetComponent<Renderer>().material.color = Color.blue;

			transform.position += velocity * frameTime;
			checkDistances(true);
			int smallestIndex = findSmallestDistance(true);
			currentGrip = findGrip(smallestIndex);
			if(smallestIndex==-1)
			{
				currentGrip = 0f;
				float groundContactBonus = Mathf.Abs(gravityEffect);
				if(velocity.y>0f)
					groundContactBonus += velocity.y*frameTime;
				if(moveIntoGroundContact(velocity.magnitude*frameTime + groundContactBonus))
				{
					checkDistances(true);
					smallestIndex = findSmallestDistance(true);
					currentGrip = findGrip(smallestIndex);
					if(currentGrip>0f)
					{
						
					}
					else
					{
						grounded2 = false;
						transform.position = startPosition + (velocity * frameTime);
					}
				}
				else
				{
					grounded2 = false;
				}
			}
			
			if(currentGrip==0f && smallestIndex!=-1)
			{
				// Mightve hit a wall, try to find ground?
				clip (smallestIndex, 1f);
				Vector3 storedPosition = transform.position;

				float groundContactBonus = Mathf.Abs(gravityEffect);
				if(velocity.y>0f)
					groundContactBonus += velocity.y*frameTime;
				if(moveIntoGroundContact(velocity.magnitude*groundBufferFactor + groundContactBonus))
				{
					checkDistances(true);
					smallestIndex = findSmallestDistance(true);
					currentGrip = findGrip(smallestIndex, true);
					if(currentGrip>0f)
					{
						
					}
					else
					{
						grounded2 = false;
						transform.position = storedPosition;
					}
				}
			}
			
			if(currentGrip>0f)
			{
				float horThrottle = horStick.returnThrottle();
				if(horThrottle==0f)
					desiredVelocity = Vector3.zero;
				else
					desiredVelocity = returnTangent(smallestIndex) * horThrottle * walkVelocity;
				Vector3 a,b;
				if(desiredVelocity!=Vector3.zero)
				{
					a = velocity.normalized;
					b = desiredVelocity.normalized;
					velocity = Vector3.Dot(a,b) * b * velocity.magnitude;
				}
				velocity = Vector3.MoveTowards (velocity, desiredVelocity, acceleration.lerpStrength (Vector3.Distance (velocity, desiredVelocity)) * currentGrip * frameTime);
			}
			else grounded2 = false;

			clip ();
			
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

			//clipVelocity(startPosition, currentGrip);
			if(velocity.magnitude>0.2f && grounded2)
				animSet("walk");
			else if (grounded2)
				animSet("idle");
			if(GetComponent<logValues>())
				GetComponent<logValues>().addValue(Vector3.Distance(transform.position,startPos)/frameTime);
			// Bug ends here
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
				//Debug.Log (2 + " " + velocity);
				velocity = Vector3.MoveTowards(velocity, desiredVelocity, airControl.lerpStrength(Vector3.Distance(velocity, desiredVelocity)) * frameTime);
				//Debug.Log (2 + " " + velocity);
			}
			
			// 1 Apply X,Y simultaneously
			transform.position += velocity * frameTime;
			
			// 2 Check distances, see if grounded.
			checkDistances ();
			int smallestIndex = findSmallestDistance ();
			lastSmallestIndex = smallestIndex;
			currentGrip = findGrip (smallestIndex);
			// This might be an issue, clipping before grip is found & applied
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
			//Debug.Log (3 + " " + velocity);
			clipVelocity(startPosition, currentGrip);
			//Debug.Log (3 + " " + velocity);
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
		{
			transform.position += -mag * ((scalar-distances[smallestIndex])*directions[smallestIndex]);
			/*if(grounded2 && findGrip(smallestIndex)>0f)
			{
				if(desiredVelocity==Vector3.zero && velocity.magnitude<(Time.deltaTime*gravity))
				{

				}
			}*/
		}
	}
	
	void clip(float mag = 1f)
	{
		checkDistances ();
		clip (findSmallestDistance (), mag);
	}
	
	void clipVelocity(Vector3 startPosition, float currentGrip)
	{
		Vector3 clipPosition = transform.position;
		Vector3 clipVector = (clipPosition - startPosition) / frameTime;
		Vector3 oldVelocityVector = velocity;
		float magnitudeFactor = 1f;
		if (currentGrip <= 0f) 
		{
			if (clipVector != Vector3.zero && oldVelocityVector != Vector3.zero)
				magnitudeFactor = Mathf.Cos (Vector3.Angle (clipVector, oldVelocityVector) * Mathf.Deg2Rad);
		}
		else
		{
			Vector3 groundVector = returnTangent(lastSmallestIndex);
			if (clipVector != Vector3.zero && oldVelocityVector != Vector3.zero)
			{
				float tangentAngle = Vector3.Angle (oldVelocityVector, groundVector);
				if(Mathf.Abs(tangentAngle)>90f)
				{
					groundVector = -groundVector;
					tangentAngle = Vector3.Angle(oldVelocityVector, groundVector);
				}
				magnitudeFactor = oldVelocityVector.magnitude * Mathf.Cos(tangentAngle * Mathf.Deg2Rad) / clipVector.magnitude;
			}
		}
		if(magnitudeFactor<0f)
			magnitudeFactor = 0f;
		//magnitudeFactor = magnitudeFactor * clipVector.magnitude;
		//clipVector.Normalize ();
		velocity = magnitudeFactor * clipVector;
	}
	
	void checkDistances(bool addBuffer = false)
	{
		bool skipped = false;
		float checkLength = scalar;
		if (addBuffer)
			checkLength = checkLength * groundBufferFactor;
		
		if(!use2dCasts)
		{
			distances[0] = returnRayLength(transform.position, directions[0], checkLength);
			
			for(int x=1;x<casts;x++)
			{
				distances[x] = returnRayLength(transform.position, directions[x], checkLength);
				if(distances[x]==checkLength)
				{
					skipped = true;
					x++;
					if(x<casts)
						distances[x] = checkLength;
				}
				else
				{
					if(skipped)
						distances[x-1] = returnRayLength(transform.position, directions[x-1], checkLength);
					skipped = false;
				}
			}
		}
		else
		{
			distances[0] = returnRayLength2d(transform.position, directions[0], checkLength);
			
			for(int x=1;x<casts;x++)
			{
				distances[x] = returnRayLength2d(transform.position, directions[x], checkLength);
				if(distances[x]==checkLength)
				{
					skipped = true;
					x++;
					if(x<casts)
						distances[x] = checkLength;
				}
				else
				{
					if(skipped)
						distances[x-1] = returnRayLength2d(transform.position, directions[x-1], checkLength);
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
	
	int findSmallestDistance(bool addBuffer = false)
	{
		int smallestIndex = -1;
		float smallestDistance = scalar;
		if (addBuffer)
			smallestDistance = smallestDistance * groundBufferFactor;
		float maximum = smallestDistance;
		globalGrip = 0f;
		float a;
		for(int x=0;x<distances.Length;x++)
		{
			if((x>gripIndexMin && x<gripIndexMax) && distances[x]<maximum)
			{
				a = findGrip(x);
				if(a>globalGrip)
				{
					globalGrip = a;
				}
			}

			if(distances[x]>smallestDistance)
			{}
			else if(distances[x]<smallestDistance)
			{
				smallestDistance = distances[x];
				smallestIndex = x;
			}
			else if(smallestIndex!=-1)
			{
				if(findGrip(x)>findGrip(smallestIndex))
				{
					smallestDistance = distances[x];
					smallestIndex = x;
				}
			}
		}
		return smallestIndex;
	}
	
	float findGrip(int theIndex, bool mayUseGlobalGrip = false)
	{
		if(mayUseGlobalGrip && theIndex>0 && theIndex<(casts/2))
		{
			return globalGrip;
		}

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
	
	bool moveIntoGroundContact(float largestDistance = 999f)
	{
		// Move down the smallest amount
		// Distance = returnRayLength - sin(iCos(
		const int castAmount = 15;
		float[] arrayOfLengths = new float[castAmount];
		float distance, factor, circleChord;
		Vector3 pos = transform.position-new Vector3(scalar,0f,0f);
		if(!use2dCasts)
		{
			for(int x=0;x<castAmount;x++)
			{
				factor = (x*1f)/((castAmount-1)*1f);
				circleChord = scalar*Mathf.Sin (Mathf.Acos((2f*factor)-1f));
				distance = returnRayLength(pos+new Vector3(2f*scalar*factor,0f,0f),Vector3.down,largestDistance+circleChord);
				if(distance==0f)
				{
					arrayOfLengths[x] = largestDistance;
					continue;
				}
				distance = distance - circleChord;
				arrayOfLengths[x] = distance;
			}
		}
		else
		{
			for(int x=0;x<castAmount;x++)
			{
				factor = (x*1f)/((castAmount-1)*1f);
				circleChord = scalar*Mathf.Sin (Mathf.Acos((2f*factor)-1f));
				distance = returnRayLength2d(pos+new Vector3(2f*scalar*factor,0f,0f),Vector3.down,largestDistance+circleChord);
				if(distance==0f)
				{
					arrayOfLengths[x] = largestDistance;
					continue;
				}
				distance = distance - circleChord;
				arrayOfLengths[x] = distance;
			}
		}
		// Find lowest distance
		distance = arrayOfLengths [0];
		for(int x=0;x<castAmount;x++)
		{
			if(arrayOfLengths[x]<distance)
				distance = arrayOfLengths[x];
		}
		if(distance<largestDistance)
		{
			transform.position += new Vector3(0f, -distance, 0f);
			return true;
		}
		return false;
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
		return Vector3.right;
		Vector3 toReturn = new Vector3 (Mathf.Cos ((angle + 90f) * Mathf.Deg2Rad), Mathf.Sin ((angle + 90f) * Mathf.Deg2Rad), 0f);
		if (toReturn.x < 0f)
			toReturn = toReturn * -1f;
		return toReturn;
	}
	
	public void die(string cause = "none")
	{
		// Do death stuff
		if(GameObject.Find("GameManager"))
		{
			GameObject.Find ("GameManager").GetComponent<DelayedRespawn> ().activate ();
			// Despawn
			despawn ();
		}
		else transform.position = spawnPosition;
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
			animRef.SetBool("jumping", false);
			animRef.SetBool("flying", false);
		}
		else if(command=="idle")
		{
			animRef.SetBool("walking", false);
			animRef.SetBool("jumping", false);
			animRef.SetBool("flying", false);
		}
		else if(command=="fly")
		{
			animRef.SetBool("walking", false);
			animRef.SetBool("jumping", false);
			animRef.SetBool("flying", true);
		}
		else if(command=="jump")
		{
			animRef.SetBool("walking", false);
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

	void logVel()
	{
		count++;
		Debug.Log (count + " " + velocity);
	}
}
