using UnityEngine;
using System.Collections;

public class FloatingVine : MonoBehaviour {

	BoxCollider2D vineCollider;
	Renderer vineRend;
	Vector3 startPos;
	public FloatUpPlatform floatingPlatform;
	public float respawnTime;
	Vector3 origScale;
	bool growUp = false;
	bool bringDown = false;
	public float growSpeed;
	public float downSpeed;

	// Use this for initialization
	void Start () {
		origScale = transform.localScale;
		vineCollider = GetComponent<BoxCollider2D>();
		vineRend = GetComponent<Renderer>();
		startPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (growUp) {
			transform.localScale += new Vector3(0,growSpeed * Time.deltaTime,0);
			transform.position += new Vector3(0,(growSpeed/2) * Time.deltaTime,0);
		}
		if (bringDown && floatingPlatform.vines == floatingPlatform.numVines) {
			transform.localScale -= new Vector3(0,downSpeed * Time.deltaTime,0);
			transform.position -= new Vector3(0,(downSpeed/2) * Time.deltaTime,0);
		}

		if (floatingPlatform.transform.position == floatingPlatform.startPos) {
			bringDown = false;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag == "Player") {
			vineCollider.enabled = false;
			vineRend.enabled = false;
			floatingPlatform.vines--;
			transform.localPosition = startPos;
			Invoke("RespawnVine", respawnTime);
		}
		if (col.gameObject == floatingPlatform.gameObject && growUp) {
			growUp = false;
			bringDown = true;
			floatingPlatform.vines++;
		}
	}

	void RespawnVine () {
		transform.localPosition = new Vector3 (startPos.x, startPos.y - (transform.localScale.y / 2), startPos.z);
		transform.localScale = new Vector3 (origScale.x, 0, origScale.z);
		vineCollider.enabled = true;
		vineRend.enabled = true;
		growUp = true;
	}
}