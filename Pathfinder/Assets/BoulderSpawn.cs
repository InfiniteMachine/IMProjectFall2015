using UnityEngine;
using System.Collections;

public class BoulderSpawn : MonoBehaviour {

	public float growSpeed;
	Vector3 growRate;
	Vector3 scale;

	// Use this for initialization
	void Awake () {
		scale = transform.localScale;
		growRate = scale;
		growRate.Normalize ();
		growRate = growRate * growSpeed;
		transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {

		transform.localScale += growRate;

		if(transform.localScale.x>scale.x)
		{
			transform.localScale = scale;
			GetComponent<Boulder>().enabled = true;
			Destroy(this);
		}
	}
}
