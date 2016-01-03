﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthEffect : MonoBehaviour {

	public Image oneHealth;
	public Image twoHealth;
	public float duration = 1.0f;
	int health;
	Color fullAlpha = new Color(0,0,0,245f);

	float t;

	int prevHealth;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//health decreasing
		if (health == 1 && prevHealth == 2 && t < 1) {
			oneHealth.color = Color.Lerp (Color.clear, Color.white, t);
			twoHealth.color = Color.Lerp (Color.white, Color.black, 1);
		} else if (health == 2 && prevHealth == 3 && t < 1) {
			twoHealth.color = Color.Lerp (Color.clear, Color.white, t);
		}

		if (health == 2 && prevHealth == 1 && t < 1) {
			oneHealth.color = Color.Lerp (Color.white, Color.clear, t);
			twoHealth.color = Color.Lerp (Color.black, Color.white, t);
		} else if (health == 3 && prevHealth == 2 && t < 1) {
			twoHealth.color = Color.Lerp (Color.white, Color.clear, t);
		}
			

		if (t < 1) {
			t += Time.deltaTime / duration;
		}
	}


	public void UpdateHealth (int curHealth) {
		prevHealth = health;
		health = curHealth;
		t = 0;
	}
}