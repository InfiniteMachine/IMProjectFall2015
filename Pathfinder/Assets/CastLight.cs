using UnityEngine;
using System.Collections;

public class CastLight : MonoBehaviour {

	public float colorChangeSpeed;
	public float prevDist;
	public Color colorToShade;
	public float maxDistance;
	// Use this for initialization
	public Renderer[] colRenderers;
	GameObject player;
	void Start () {
		maxDistance = GetComponent<CircleCollider2D> ().radius;
		prevDist = maxDistance;
	}
	
	// Update is called once per frame
	void Update () {
		//print("update  " + Vector3.Distance (transform.position, player.transform.position));
	}

	void OnTriggerStay2D (Collider2D col) {
		if (col.tag == "Player") {
			float distance = Vector3.Distance (transform.position, col.transform.position);
			if (col.GetComponent<SpriteRenderer> () == null)
				colRenderers = col.gameObject.GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer colRenderer in colRenderers) {
				colRenderer.material.color = Color.Lerp (colorToShade, Color.white, distance/maxDistance);
			}
			prevDist = distance;

		}
	}
}