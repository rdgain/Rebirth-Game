using UnityEngine;
using System.Collections;

public class Clone0Script : MonoBehaviour {
	private Player player;
	private CameraControl cam;
    private MapEngine mapEngine;
	private ShadowPlayer clone;
	private bool canMove;
	
    public float speed = 1f;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player>();	
		cam = FindObjectOfType<CameraControl>();
	    clone = transform.parent.GetComponentInChildren<ShadowPlayer>();
		mapEngine = FindObjectOfType<MapEngine>();
		canMove = false;
	}
	
	void FixedUpdate () {
		//move only when player is in the room
		if (mapEngine.ActiveRoom() == transform.parent.gameObject.name) {
			if (Input.GetButtonUp ("Action")) { //toggle can move by action button
				if (canMove) canMove = false;
				else {
					canMove = true;					
					clone = transform.parent.GetComponentInChildren<ShadowPlayer>();
				}
			}
		} else {
			canMove = false;
		}
		if (canMove) {
			player.SetPlayerControl(false);
			cam.SetClone(clone.gameObject);
			Vector3 temp = clone.transform.position;
			float x = Input.GetAxis("Horizontal");
			float y = Input.GetAxis("Vertical");
			temp.x += x * speed * (Time.deltaTime * 4);
			temp.y += y * speed * (Time.deltaTime * 4);
			clone.transform.position = temp;
            clone.SetAnimation(new Vector3(x, y, 0));
		} else {
			player.SetPlayerControl(true);
		}
	}
}
