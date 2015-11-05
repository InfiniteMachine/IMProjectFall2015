using UnityEngine;
using System.Collections;

public class FoodTracker : MonoBehaviour {

	public GameObject[] allFoods;
	public bool[] consumed;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (gameObject);
		name = "FoodTracker" + Application.loadedLevelName;
		bool shouldDestroy = false;
		GameObject[] allFoodTrackers = GameObject.FindGameObjectsWithTag("FoodTracker");
		int count = 0;
		foreach(GameObject foodTracker in allFoodTrackers)
		{
			if(foodTracker.name==name)
				count++;
		}


		if(count>1)
			GameObject.Destroy(gameObject);

		consumed = new bool[allFoods.Length];

		// Load consumed bools from a save file
		for (int x=0; x<consumed.Length; x++)
			consumed [x] = false;

		// Activate/Disable correct foods, apply correct art, etc.
		// Should have function that can deal with each of the foods
		// art objects one at a time
	}

	public int nearFood(Vector3 pos, float range)
	{
		for(int x=0;x<allFoods.Length;x++)
		{
			if(consumed[x])
				continue;

			Vector3 relativity = Vector3.zero;
			if(allFoods[x].GetComponent<CircleCollider2D>())
			{
				relativity = allFoods[x].GetComponent<CircleCollider2D>().offset;
			}
			else if(allFoods[x].GetComponent<SphereCollider>())
			{
				relativity = allFoods[x].GetComponent<SphereCollider>().center;
			}

			if(Vector3.Distance(pos, relativity+allFoods[x].transform.position)<range)
				return x;
		}
		return -1;
	}

	public void eat(int index)
	{
		consumed[index] = true;

		// Update art
		Debug.Log ("Eated food" + index);
	}
}