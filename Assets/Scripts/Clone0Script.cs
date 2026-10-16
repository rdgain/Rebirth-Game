using UnityEngine;
    /// <summary>Applies direct movement input to clone zero.</summary>

public class Clone0Script : MonoBehaviour {
	private Player player;
	private CameraControl cam;
    private MapEngine mapEngine;
	private ShadowPlayer clone;
	private bool canMove;

    public float speed = 1f;

	// Use this for initialization
	void Start () {
		player = FindAnyObjectByType<Player>();
		cam = FindAnyObjectByType<CameraControl>();
	    clone = transform.parent.GetComponentInChildren<ShadowPlayer>();
		mapEngine = FindAnyObjectByType<MapEngine>();
		canMove = false;
	}

	void FixedUpdate () {
		//move only when player is in the room
		if (mapEngine.ActiveRoom() == transform.parent.gameObject.name) {
			if (GameplayInput.ActionReleasedThisFrame) { //toggle can move by action button
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
			Vector2 movement = GameplayInput.Movement;
			float x = movement.x;
			float y = movement.y;
			temp.x += x * speed * (Time.deltaTime * 4);
			temp.y += y * speed * (Time.deltaTime * 4);
			clone.transform.position = temp;
            clone.SetAnimation(new Vector3(x, y, 0));
		} else {
			player.SetPlayerControl(true);
		}
	}
}
