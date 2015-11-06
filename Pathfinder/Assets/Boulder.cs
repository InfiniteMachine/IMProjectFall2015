using UnityEngine;
using System.Collections;

public class Boulder : MonoBehaviour {

	bool checkDestroy = false;
	float yBound = -1000f;
	float minimumTimeAlive = 0.8f;
	float rotationSpeed;

	public GameObject[] colliderObjects;

	// Use this for initialization
	void Awake () {
		rotationSpeed = Random.Range (30f, 180f);
	}
	
	// Update is called once per frame
	void Update () {
		minimumTimeAlive += -Time.deltaTime;
		if(minimumTimeAlive<0f && !checkDestroy)
		{
			for(int x=0;x<colliderObjects.Length;x++)
				colliderObjects[x].GetComponent<PolygonCollider2D> ().isTrigger = false;
		}

		transform.eulerAngles += new Vector3(0f, 0f, rotationSpeed*Time.deltaTime);
		if(checkDestroy)
		{
			if(transform.position.y<yBound)
				GameObject.Destroy(gameObject);
		}
	}
	// lol
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag=="Player")
		{
			GameObject.Find ("PlayerCharacter").GetComponent<SphereController>().die ();
		}
	}

	void OnCollisionEnter2D(Collision2D a)
	{
		if(a.collider.gameObject.tag=="Untagged")
		{
			tag = "Dud";
		}
		for(int x=0;x<colliderObjects.Length;x++)
			colliderObjects[x].GetComponent<PolygonCollider2D> ().isTrigger = true;
		checkDestroy = true;
	}
}
