using UnityEngine;
using System.Collections;

public class Clone1Script : MonoBehaviour {
    private MapEngine mapEngine;
	private GameObject clone;
	private bool canMove;
	
    public float speed = 1f;

	// Use this for initialization
	void Start () {
		clone = transform.parent.Find("Clone").gameObject;
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
					clone = transform.parent.Find("Clone").gameObject;
				}
			}
		} else {
			canMove = false;
            clone.GetComponent<ShadowPlayer>().SetAnimation(new Vector3(0, 0, 0));
        }
		if (canMove) {
			Vector2 temp = clone.transform.position;
			float x = Input.GetAxis("Horizontal");
			float y = Input.GetAxis("Vertical");
			temp.x += x * speed * (Time.deltaTime * 4);
			temp.y += y * speed * (Time.deltaTime * 4);		
			clone.transform.position = temp;
            clone.GetComponent<ShadowPlayer>().SetAnimation(new Vector3(x, y, 0));
		}
	}
}
