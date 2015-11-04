using UnityEngine;
using System.Collections;

public class ParallaxScott : MonoBehaviour {
	
	public float para_percent = 1f;
	float origin;
	GameObject theplayer;
	// Use this for initialization
	void Start () {
		theplayer = GameObject.FindGameObjectWithTag ("MainCamera");
		origin = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float difference;
		difference = theplayer.transform.position.x - origin;
		if((difference>0)||(difference<0))
		{
			transform.position = new Vector3(theplayer.transform.position.x
			                                 - difference * para_percent,
			                                 transform.position.y,
			                                 transform.position.z);
		}
		else
			transform.position = new Vector3(origin, transform.position.y, transform.position.z);
	}
}