using UnityEngine;
using System.Collections;

public class DelayedEndDay : MonoBehaviour {

	public Platformer.Timer timer;
	bool activated = false;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(activated)
		{
			if(timer.tick ())
			{
				activated = false;
				// Do what when activated
				GameObject.Find ("GameManager").GetComponent<GameManager>().setCheckpoint();
				Application.LoadLevel("TechTree");
			}
		}
	}
	
	public void activate()
	{
		activated = true;
		timer.set ();
	}
}