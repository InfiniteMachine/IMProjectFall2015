using UnityEngine;
using System.Collections;

public class SphereActions : MonoBehaviour {

	SphereController controlRef;
	FoodTracker trackerRef;
	public float eatDuration = 0f;
	public float eatRange = 1f;
	float eatTime = 0f;
	int currentEatingIndex = -1;

	// Should change transform.position to current world position of mouth

	// Use this for initialization
	void Awake () {
		trackerRef = GameObject.Find ("FoodTracker" + Application.loadedLevelName).GetComponent<FoodTracker> ();
		controlRef = GetComponent<SphereController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(controlRef.active && controlRef.velocity == Vector3.zero)
		{
			if(Input.GetKey(KeyCode.E)) // eat
			{
				// Look for foods
				int newEatingIndex = trackerRef.nearFood(transform.position, eatRange);

				Debug.Log (eatTime + " " + newEatingIndex);

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
		}
	}
}
