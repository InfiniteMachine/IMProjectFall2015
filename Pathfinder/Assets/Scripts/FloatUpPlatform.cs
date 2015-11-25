using UnityEngine;
using System.Collections;

public class FloatUpPlatform : MonoBehaviour {

	[HideInInspector]
	public Vector3 startPos;
	Vector3 maxPos;
	[HideInInspector]
	public int vines;
	public float riseSpeed;
	public float lowerSpeed;
	public int numVines;


	// Use this for initialization
	void Start () {
		startPos = transform.position;
		Invoke ("SetMax", .5f);
	}

	// Update is called once per frame
	void Update () {
		if (vines == 0) {
			transform.position = Vector3.MoveTowards (transform.position, maxPos, riseSpeed * Time.deltaTime);
		} else if (vines == numVines) {
			transform.position = Vector3.MoveTowards (transform.position, startPos, lowerSpeed * Time.deltaTime);
		}
	}

	void SetMax () {
		maxPos = startPos + new Vector3(0,numVines,0);
		vines = numVines;
	}
}