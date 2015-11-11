using UnityEngine;
using System.Collections;

public class DayNightEnemies : MonoBehaviour {
    GameObject Player;

    bool enemyEngaged = false;
    bool enemyFacingRight = false;

    bool facingRight;
    bool playerFacingEnemy;

    public int maxDistance;
    public int distanceToEngage;

    public float enemyBaseSpeed;
    float enemyAcceleration;
    float enemySpeed;

	// Use this for initialization
	void Awake () {
        Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
    void Update()
    {
        var distance = Vector3.Distance(Player.transform.position, transform.position);
        
        if (enemyEngaged == false)
        {
            //change sprite to show enemy is engaged or not
            //Enemy.sprite = ;
            if (distance < distanceToEngage)
            {
                enemyEngaged = true;
            }
        }

        if (enemyEngaged == true)
        {
            //change sprite to show enemy is engaged or not
            //Enemy.sprite = ;
            //Track

            enemyAcceleration = 10 / (4 * distance);
            enemySpeed = enemyBaseSpeed + enemyAcceleration;
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, enemySpeed * Time.deltaTime);

            //check if enemy still engaged
            //distance too great, enemyEngaged=false
            if (distance > maxDistance)
            {
                enemyEngaged = false;
            }

            //flip enemy
            if (Player.transform.position.x > transform.position.x)
            {
                if (enemyFacingRight == false)
                {
                    enemyFacingRight = true;
                    Flip();
                }
            }
            if (Player.transform.position.x < transform.position.x)
            {
                if (enemyFacingRight == true)
                {
                    enemyFacingRight = false;
                    Flip();
                }
            }
        }
    }

      void Flip()
    {
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }
}
