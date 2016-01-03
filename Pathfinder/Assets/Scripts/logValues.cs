using UnityEngine;
using System.Collections;

public class logValues : MonoBehaviour {

	public float value = 0f;
	public float highest=0f;
	public float lowest=5f;

	public float newTimeStr = 100f;
	public bool reset;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void addValue(float a)
	{
		value += (1f / a) / newTimeStr;
		value = (value / (newTimeStr+1f)) * newTimeStr;
		if(reset)
		{
			highest = -99999f;
			lowest = 99999f;
			reset = false;
		}
		if (a > highest)
			highest = a;
		if (a < lowest)
			lowest = a;
	}
}
