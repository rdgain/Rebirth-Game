using UnityEngine;
    /// <summary>Applies movement, action, and tether controls to clone four.</summary>

public class Clone4Script : MonoBehaviour {
	private Player player;
	private CameraControl cam;
    private MapEngine mapEngine;
	private GameObject clone;
    public float speed = 1f;

	// Use this for initialization
	void Start () {
		player = FindAnyObjectByType<Player>();
		cam = FindAnyObjectByType<CameraControl>();
		clone = transform.parent.Find("Clone").gameObject;
		mapEngine = FindAnyObjectByType<MapEngine>();
	}

	void FixedUpdate () {/*
		//move only when player is in the room
		if (mapEngine.ActiveRoom() == transform.parent.gameObject.name) {
			if (GameplayInput.ActionReleasedThisFrame) { //toggle can move by action button E
				if (canMove) canMove = false;
				else canMove = true;
			}
			if (GameplayInput.TetherReleasedThisFrame) { //toggle tether by tether button T
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
			Vector2 movement = GameplayInput.Movement;
			float x = movement.x;
			float y = movement.y;
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
