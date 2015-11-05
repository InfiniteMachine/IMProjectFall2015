using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject localCamera;
	Platformer.ScenePosition checkpoint = new Platformer.ScenePosition ("Nest", new Vector3 (-4.53f, 0.89f, 0f));

	Platformer.ScenePosition beginPoint = new Platformer.ScenePosition ("Nest", new Vector3 (-4.53f, 0.89f, 0f));

	public string[] sceneNames;

	int number = -1;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (gameObject);
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

	public int freshID()
	{
		number++;
		return number;
	}

	public void setCheckpoint()
	{
		checkpoint.scene = Application.loadedLevelName;
		checkpoint.pos = transform.position;
	}
}
