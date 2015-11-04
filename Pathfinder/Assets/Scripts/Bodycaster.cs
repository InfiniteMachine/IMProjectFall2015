using UnityEngine;
using System.Collections;

public class Bodycaster : MonoBehaviour {

	public GameObject bodyCapsules;
	public LayerMask layers;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*public float returnRayLength(Vector3 position, Vector3 direction, float length = 9999f)
	{
		Physics.caps
		RaycastHit hit;
		Ray ray = new Ray(position, direction);
		
		//if something under the player
		if (Physics.Raycast(ray, out hit, length, layers))
		{
			Debug.Log(hit.collider.gameObject.name);
			return hit.distance;
		}
		return length;
	}*/
}
