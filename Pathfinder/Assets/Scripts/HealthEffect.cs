using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthEffect : MonoBehaviour {

	public Image healthUI;
	public Material nearDeath;
	public Material fullHealth;
	public float duration = 2.0f;
	public float health;
	public float maxHealth;

	public Vector3 fullHealthScale;
	public Vector3 nearDeathScale;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//healthUI.material.Lerp (nearDeath, fullHealth, health / maxHealth);
	}
}