using UnityEngine;
using System.Collections;

public class BoulderGenerator : MonoBehaviour {

	public float intervalX, intervalY, delay;
	public GameObject[] boulderSelection;
	public GameObject particleEffect;
	float currentTime;
	bool readyToSpawn = false;

	// Use this for initialization
	void Awake () {
		currentTime = Random.Range (intervalX, intervalY);
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += -Time.deltaTime;
		if(currentTime < 0f && readyToSpawn)
		{
			GameObject.Instantiate (boulderSelection [Random.Range (0, boulderSelection.Length)], transform.position, Quaternion.identity);
			currentTime = Random.Range (intervalX - delay, intervalY - delay);
			readyToSpawn = false;
		}
		else if (currentTime < 0f)
		{
			readyToSpawn = true;
			currentTime = delay;
			particleEffect.GetComponent<ParticleSystem>().Play();
		}

	}
}
