using UnityEngine;
using System.Collections;

public class Clone3Script : MonoBehaviour {
	//private Animator anim_controller;
	public float speed = 1f;
	public float chaseSpeed = 2f;
	public int mass = 1;
	
    private MapEngine mapEngine;
	private Temple temple;
	private bool canMove, seenPlayer;
	private Vector2 temp;
	private float radius1, radius2;
	private int counter, maxCount;
	private GameObject player;
	
    private Vector2 currentVelocity, steering, desiredVelocity;
	
	private GameObject clone;

	void Start () {
		mapEngine = FindObjectOfType<MapEngine>();
		temple = FindObjectOfType<Temple>();
		clone = GameObject.Find("Clone");
		player = GameObject.Find("Player");
		canMove = false;
		seenPlayer = false;
		radius1 = 100;
		radius2 = 30;
		maxCount = 30;
		counter = maxCount;
		currentVelocity = clone.transform.GetComponent<Rigidbody2D>().velocity;
	}
	
	void FixedUpdate ()
    {
		clone = GameObject.Find("Clone");
	    if (clone != null)
	    {
	        if (!seenPlayer)
	        {
				if (mapEngine.ActiveRoom() == transform.parent.gameObject.name) { //if player in the room
					//reward player + time
					if (temple.SecondsLeft() < temple.InitialTime)
						temple.Buff(Time.deltaTime * 2f);
				}
				
	            //wander behaviour
	            if (counter == maxCount)
	            {
	                counter = 0;
	                clone.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	                Vector2 init = Vector2.zero;

	                // get point on first big circle
	                float c1x = Random.Range(radius1*(-1), radius1);
	                int g = Random.Range(-1, 2);
	                float c1y = g*Mathf.Sqrt(radius1*radius1 - c1x*c1x);
	                init.x += c1x;
	                if (float.IsNaN(c1y)) c1y = 0;
	                init.y += c1y;

	                //get point on second small circle
	                float c2x = Random.Range(radius1*(-1), radius1);
	                g = Random.Range(-1, 2);
	                float c2y = g*Mathf.Sqrt(radius2*radius2 - c2x*c2x);
	                init.x += c2x;
	                if (float.IsNaN(c2y)) c2y = 0;
	                init.y += c2y;

	                init.Normalize();

	                clone.GetComponent<ShadowPlayer>().SetVelocity(new Vector3(init.x, init.y, transform.position.z));
	            }
	            else
	            {
	                counter ++;
	            }
	        }
	        else
	        {
	            //penalise player 
	            temple.Penalise(0.6f);

	            //chase player behaviour
	            seek();

	            // Add steering component to the current velocity and desired speed
	            currentVelocity = currentVelocity + steering/this.mass;
	            currentVelocity.Normalize();
	            currentVelocity *= speed;

	            // Set the velocity to the rigidbody ’s velocity .
	            clone.GetComponent<ShadowPlayer>().SetVelocity(currentVelocity);
	        }

	        //raycast to check for players
	        RaycastHit2D hit1 = Physics2D.Raycast(clone.transform.position, Vector2.down);
	        RaycastHit2D hit2 = Physics2D.Raycast(clone.transform.position, Vector2.up);
	        RaycastHit2D hit3 = Physics2D.Raycast(clone.transform.position, Vector2.left);
	        RaycastHit2D hit4 = Physics2D.Raycast(clone.transform.position, Vector2.right);

	        if (hit1.collider != null && hit1.transform.name == "Player" ||
	            hit2.collider != null && hit2.transform.name == "Player" ||
	            hit3.collider != null && hit3.transform.name == "Player" ||
	            hit4.collider != null && hit4.transform.name == "Player")
	        {
	            seenPlayer = true;
	        }
	        else
	        {
	            seenPlayer = false;
	        }
	    }
	}
	
	void seek () {
		// Desired velocity is the vector to the target .
		Vector2 desiredVelocity = ( player.transform.position - clone.transform.position );

		// ... normalized and with the correct speed
		desiredVelocity.Normalize();
		desiredVelocity *= speed;

		// The steering component to steer towards the target :
		steering = desiredVelocity - currentVelocity;
	}
}


