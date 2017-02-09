using UnityEngine;
using System.Collections;

public class Clone4Script : MonoBehaviour {
	private Player player;
	private CameraControl cam;
    private MapEngine mapEngine;
	private GameObject clone;
	private bool canMove, tether, tetherStop;
	private float maxDist;
	
	
    public float speed = 1f;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player>();	
		cam = FindObjectOfType<CameraControl>();
		clone = transform.parent.Find("Clone").gameObject;
		mapEngine = FindObjectOfType<MapEngine>();
		canMove = false;
		tether = false;
		tetherStop = false;
		maxDist = 12;
	}
	
	void FixedUpdate () {/*
		//move only when player is in the room
		if (mapEngine.ActiveRoom() == transform.parent.gameObject.name) {
			if (Input.GetButtonUp ("Action")) { //toggle can move by action button E
				if (canMove) canMove = false;
				else canMove = true;
			}
			if (Input.GetButtonUp ("Tether")) { //toggle tether by tether button T
				if (tether) tether = false;
				else tether = true;
			}
			
			if (tether) {			
				//restrict movement if player and clone too far apart
				float dist = Vector2.Distance(player.transform.position, transform.position);
				print(dist);
				if (dist > maxDist) {
					tetherStop = true;
					player.canMove = false;
				} else {
					tetherStop = false;
					player.canMove = true;
				}
			}
		} else {
			canMove = false;
		}
		if (canMove) {
			player.SetPlayerControl(false);
			cam.SetClone(clone);
			Vector2 temp = clone.transform.position;
			float x = Input.GetAxis("Horizontal");
			float y = Input.GetAxis("Vertical");
			if (!tetherStop) {
				temp.x += x * speed * (Time.deltaTime * 4);
				temp.y += y * speed * (Time.deltaTime * 4);
			} else {
				temp.x -= x * speed * (Time.deltaTime * 4);
				temp.y -= y * speed * (Time.deltaTime * 4);
			}					
			clone.transform.position = temp;
		} else {
			player.SetPlayerControl(true);
		}*/
	}
}
