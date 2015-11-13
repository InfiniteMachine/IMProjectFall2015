using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SphereActions : MonoBehaviour {

	SphereController controlRef;
	FoodTracker trackerRef;
	public float eatDuration = 0f;
	public float eatRange = 1f;
	float sleepRange = 2.5f;
	float eatTime = 0f;
	int currentEatingIndex = -1;


    private bool canBite = false;
    public List<string> biteableTags = new List<string>();
    private Collider2D bitable = null;

    public float digDuration = 1f;
    public float maxDigDistance = 3f;
    public float biteDuration = 1f;
    private Coroutine dig;
    private Coroutine bite;
	// Should change transform.position to current world position of mouth

	// Use this for initialization
	void Awake () {
		if (GameObject.Find ("FoodTracker"))
			GameObject.Find ("FoodTracker").name = "FoodTracker" + Application.loadedLevelName;
		trackerRef = GameObject.Find ("FoodTracker" + Application.loadedLevelName).GetComponent<FoodTracker> ();
		controlRef = GetComponent<SphereController> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (controlRef.active && controlRef.velocity == Vector3.zero)
		{
            if (Input.GetKeyDown(KeyCode.F) && dig == null)
            {
                RaycastHit2D hit;
                Vector2 castDirection = Vector2.zero;
                if (Input.GetKey(KeyCode.A))
                {
                    //Dig Left
                    castDirection = Vector2.left;
                }else if (Input.GetKey(KeyCode.D))
                {
                    //Dig Right
                    castDirection = Vector2.right;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    castDirection = Vector2.down;
                }
                hit = Physics2D.Raycast(transform.position, castDirection, maxDigDistance);
                if (hit)
                {
                    if(hit.collider.tag == "DiggableTerrain")
                    {
                        dig = StartCoroutine(DestroyLater(hit.collider.gameObject, digDuration));
                    }
                }
            }
			if(Input.GetKeyDown(KeyCode.E)) // eat
			{
                if (bitable != null)
                {
                    //PlayBitingAnimation
                    bite = StartCoroutine(BiteLater(bitable.gameObject, biteDuration));
                }
                else
                {
                    // Look for foods
                    int newEatingIndex = trackerRef.nearFood(transform.position, eatRange);

                    //Debug.Log (eatTime + " " + newEatingIndex);

                    if (newEatingIndex != -1)
                    {
                        //if (currentEatingIndex == newEatingIndex)
                        //{
                        //    eatTime += Time.deltaTime;
                        //    if (eatTime > eatDuration)
                        //    {
                        //        trackerRef.eat(currentEatingIndex);
                        //        currentEatingIndex = -1;
                        //    }
                        //}
                        //else eatTime = 0f;
                        //currentEatingIndex = newEatingIndex;
                        bite = StartCoroutine(BiteLater(newEatingIndex, biteDuration));
                    }
                }
			}
			else 
			{
				eatTime = 0f;
			}

			if(Input.GetKey(KeyCode.R) && eatTime == 0f)
			{

			}
			else if(Input.GetKeyDown(KeyCode.E) && Vector3.Distance(transform.position, GameObject.Find("Nest").transform.position)<sleepRange)// && trackerRef.specialCase()) // Also, food must be at least >=1!!!!
			{
				// Go to sleep
				controlRef.active = false;
				// Start sleep animation
				GameObject.Find ("GameManager").GetComponent<DelayedEndDay>().activate();
			}
		}
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (biteableTags.Contains(col.tag))
        {
            if((controlRef.transform.localScale.x > 0 && col.transform.position.x > transform.position.x) || (controlRef.transform.localScale.x < 0 && col.transform.position.x < transform.position.x))
                bitable = col;
        }
    }

    public void StopDig()
    {
        if (dig != null)
            StopCoroutine(dig);
        dig = null;
    }

    IEnumerator DestroyLater(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(go);
    }

    public void StopBite()
    {
        if(bite != null)
            StopCoroutine(bite);
        bite = null;
    }

    IEnumerator BiteLater(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        go.GetComponent<BitableObject>().Bite();
    }

    IEnumerator BiteLater(int foodIndex, float time)
    {
        yield return new WaitForSeconds(time);
        trackerRef.eat(foodIndex);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(bitable == col)
        {
            bitable = null;
        }
    }
}