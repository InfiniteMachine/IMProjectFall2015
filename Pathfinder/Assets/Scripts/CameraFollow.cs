﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Platformer.Acceleration lerp;
	public GameObject followingObject;
	public Vector3 followingPosition;
	public bool useObject = false;
	public bool useReportedPosition = false;
	public bool usePosition = false;
	public Vector3 velocity;
	float frameTime;

	// Use this for initialization
	void Awake() {

	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 startPos = transform.position;
		float distance;
		if(useObject)
		{
			if(followingObject==null)
			{
				useObject = false;
			}
			else if(useReportedPosition)
			{
				distance = Vector3.Distance(transform.position, followingPosition);
				transform.position = 
					Vector3.MoveTowards(transform.position, followingPosition,
					                    lerp.lerpStrength( distance)*frameTime);
				velocity = transform.position - startPos;
			}
			else
			{
				distance = Vector3.Distance(transform.position, followingObject.transform.position);
				transform.position = 
					Vector3.MoveTowards(transform.position, followingObject.transform.position,
					                    lerp.lerpStrength(distance)*Time.deltaTime);
				velocity = transform.position - startPos;
			}
		}
		else if(usePosition)
		{
			distance = Vector3.Distance(transform.position, followingPosition);
			transform.position = 
				Vector3.MoveTowards(transform.position, followingPosition,
				                    lerp.lerpStrength( distance)*Time.deltaTime);
			velocity = transform.position - startPos;
		}
		else if(velocity!=Vector3.zero)
		{
			transform.position += velocity;
			velocity = Vector3.MoveTowards(velocity, Vector3.zero,
			                               lerp.lerpStrength(velocity.magnitude) * Time.deltaTime * 0.5f);
		}
	}

	public void newFollow(GameObject a)
	{
		followingObject = a;
		useObject = true;
		usePosition = false;
		useReportedPosition = false;
		/*if(a.GetComponent<SphereController>())
		{
			a.GetComponent<SphereController>().camRef = this;
			useReportedPosition = true;
		}*/
	}

	public void newFollow(Vector3 a)
	{
		followingPosition = a;
		useObject = false;
		usePosition = true;
		useReportedPosition = false;
	}

	public void reportPosition(Vector3 a, float deltaTime)
	{
		followingPosition = a;
		frameTime = deltaTime;
	}
}
