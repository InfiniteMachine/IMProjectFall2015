using UnityEngine;
using System.Collections;

public class StayGrounded : MonoBehaviour {

    public bool stayDown = true;

    public float distance = 3f;
    [Range(0f, 1f)]
    public float errorPercentage = 0.8f;
    public Rigidbody2D rBody;
    public CircleCollider2D colider;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void FixedUpdate()
    {
        //if (stayDown && rBody.velocity.y > 0)
           // rBody.velocity.Set(rBody.velocity.y, -1);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            if (stayDown)
            {
                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + colider.offset, Vector2.down, distance);
                if (hit) {
                    Vector2 normal = -hit.normal;
                    RaycastHit2D hit2 = Physics2D.Raycast((Vector2)transform.position + colider.offset, normal, distance);
                    if (hit2)
                    {
                        Vector2 vec = hit2.point - ((Vector2)transform.position + colider.offset);
                        vec = vec.normalized * (vec.magnitude - colider.radius) * errorPercentage;
                        vec.x = 0;
                        rBody.MovePosition(transform.position + (Vector3)vec);
                    }
                    rBody.velocity = new Vector2(rBody.velocity.x, -5f);
                }
            }
        }
    }
}
