using UnityEngine;
using System.Collections;

public class BushAI : MonoBehaviour {
    private Animation am;
    private DayNNight dn;
    private Transform player;
    public float distanceShake = 25;
    public float distanceKill = 1;
    // Use this for initialization
	void Start () {
        am = GetComponent<Animation>();
        dn = GameObject.FindGameObjectWithTag("DayNightCanvas").GetComponent<DayNNight>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (dn.GetState() == DayNNight.CycleState.Night)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance < distanceShake)
            {
                am.Play();
            }
            else
            {
                am.Stop();
            }
            if (distance < distanceKill)
            {
                //KillPlayer
            }
        }
	}
}
