using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {
	
	public bool isNightTime;
	public Vector3 playerPos;
	
	
	public GameObject[] enemies;
	public Vector3 spawnPos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!isNightTime) {
			transform.Rotate(Vector3.up * Time.deltaTime);
		} else {
			transform.Rotate(Vector3.up * 3 * Time.deltaTime);
		}
		
		//when the sun sets
		if (transform.eulerAngles.y > 80 && transform.eulerAngles.y < 81) {
			if (!isNightTime)
				OnDusk();
		}
		//when the sun rises
		if (transform.eulerAngles.y > 280 && transform.eulerAngles.y < 283) {
			if (isNightTime)
				OnDawn();
		}
	}
	
	
	void OnDawn () {
		isNightTime = false;
	}
	
	void OnDusk () {
		isNightTime = true;
		InvokeRepeating("SpawnEnemies", 1f, 10f);
	}
	
	public int minDist;
	int spawnsPerTick = 1;
	void SpawnEnemies () {
		
		//playerPos = Vector3.zero;
		for (int i = 0; i < spawnsPerTick; i++) {
			Vector3 randPos = Random.insideUnitCircle;
			spawnPos = randPos * minDist + playerPos;
			Instantiate(enemies[0], spawnPos, Quaternion.identity);
		}
	}
	
	void StopEnemySpawns () {
		CancelInvoke();
	}
}