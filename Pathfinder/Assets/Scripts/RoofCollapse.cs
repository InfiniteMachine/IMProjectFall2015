using UnityEngine;
using System.Collections;

public class RoofCollapse : MonoBehaviour {

	public GameObject objectToGrow;
	public float scaletoGrow;
	public bool hasGrown;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag == "Player" && !hasGrown) {
			objectToGrow.transform.localScale += new Vector3(0,objectToGrow.transform.localScale.y * scaletoGrow, 0);
			hasGrown = true;
			Destroy(gameObject);
		}
	}
}