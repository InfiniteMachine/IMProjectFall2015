﻿using UnityEngine;
using System.Collections.Generic;

public class SphereActions : MonoBehaviour {

	SphereController controlRef;
	FoodTracker trackerRef;
	public float eatDuration = 0f;
	public float eatRange = 1f;
	float sleepRange = 2.5f;
	float eatTime = 0f;
	int currentEatingIndex = -1;


    private bool canBite = false;
    public List<string> biteableTags = new List<string>();
    private Collider2D bitable;
	// Should change transform.position to current world position of mouth

	// Use this for initialization
	void Awake () {
		if (GameObject.Find ("FoodTracker"))
			GameObject.Find ("FoodTracker").name = "FoodTracker" + Application.loadedLevelName;
		trackerRef = GameObject.Find ("FoodTracker" + Application.loadedLevelName).GetComponent<FoodTracker> ();
		controlRef = GetComponent<SphereController> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (controlRef.active && controlRef.velocity == Vector3.zero)
		{
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Play Biting Animation
                if(bitable != null)
                {
                    bitable.GetComponent<BitableObject>().Bite();
                }
            }
			if(Input.GetKey(KeyCode.E)) // eat
			{
				// Look for foods
				int newEatingIndex = trackerRef.nearFood(transform.position, eatRange);

				//Debug.Log (eatTime + " " + newEatingIndex);

				if(newEatingIndex!=-1)
				{
					if(currentEatingIndex==newEatingIndex)
					{
						eatTime += Time.deltaTime;
						if(eatTime>eatDuration)
						{
							trackerRef.eat(currentEatingIndex);
							currentEatingIndex = -1;
						}
					}
					else eatTime = 0f;
					currentEatingIndex = newEatingIndex;
				}
			}
			else 
			{
				eatTime = 0f;
			}

			if(Input.GetKey(KeyCode.R) && eatTime == 0f)
			{

			}
			else if(Input.GetKeyDown(KeyCode.E) && trackerRef.specialCase() && Vector3.Distance(transform.position, GameObject.Find("Nest").transform.position)<sleepRange) // Also, food must be at least >=1!!!!
			{
				// Go to sleep
				controlRef.active = false;
				// Start sleep animation
				GameObject.Find ("GameManager").GetComponent<DelayedEndDay>().activate();
			}
		}
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (biteableTags.Contains(col.tag))
        {
            //if((facingRight && col.transform.position.x > transform.position.x) || (facingLeft && col.transform.position.x < transform.position.x))
            //Need to change bitable to null when switch directions? maybe
            bitable = col;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(bitable = col)
        {
            bitable = null;
        }
    }
}