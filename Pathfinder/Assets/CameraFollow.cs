using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Platformer.Acceleration lerp;
	public GameObject followingObject;
	public Vector3 followingPosition;
	public bool useObject = false;
	public bool usePosition = false;

	// Use this for initialization
	void Awake() {

	}
	
	// Update is called once per frame
	void LateUpdate () {
		float distance;
		if(useObject)
		{
			distance = Vector3.Distance(transform.position, followingObject.transform.position);
			transform.position = 
				Vector3.MoveTowards(transform.position, followingObject.transform.position,
				                    lerp.lerpStrength(distance)*Time.deltaTime);
		}
		else if(usePosition)
		{
			distance = Vector3.Distance(transform.position, followingPosition);
			transform.position = 
				Vector3.MoveTowards(transform.position, followingPosition,
				                    lerp.lerpStrength( distance)*Time.deltaTime);
		}
	}

	public void newFollow(GameObject a)
	{
		followingObject = a;
		useObject = true;
		usePosition = false;
	}

	public void newFollow(Vector3 a)
	{
		followingPosition = a;
		useObject = false;
		usePosition = true;
	}
}
