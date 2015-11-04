using UnityEngine;
using System.Collections;

public class FpsCounter : MonoBehaviour {

	float fps = 60f;
	public float newTimeStr = 100f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		fps += (1f / Time.deltaTime) / newTimeStr;
		fps = (fps / (newTimeStr+1f)) * newTimeStr;
		Debug.Log ("FPS: " + fps);
	}
}
