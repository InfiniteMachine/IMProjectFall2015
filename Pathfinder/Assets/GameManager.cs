using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject localCamera;
	Platformer.ScenePosition checkpoint = new Platformer.ScenePosition ("Nest", new Vector3 (-4.53f, 0.89f, 0f));

	// Use this for initialization
	void Awake () {
		respawn ();
		localCamera.GetComponent<CameraFollow>().newFollow(GameObject.Find ("PlayerCharacter"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void respawn()
	{
		if (checkpoint.scene != Application.loadedLevelName)
			Application.LoadLevel (checkpoint.scene);
		GameObject a = (GameObject) GameObject.Instantiate (playerPrefab, checkpoint.pos, Quaternion.identity);
		localCamera.GetComponent<CameraFollow>().newFollow (a);
	}
}
