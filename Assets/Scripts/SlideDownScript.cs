using UnityEngine;
using System.Collections;

public class SlideDownScript : MonoBehaviour {
	private Player player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
	}
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.down, ForceMode2D.Impulse);
        }
	}
	
	public Vector2 GetForce(Vector2 f) {
		switch (FindObjectOfType<MapEngine>().RoomOrientation(transform.parent.name))
        {
            case 1:
                return new Vector2(f.y, f.x);
            case 2:
                return new Vector2(-f.x, f.y);
            case 3:
                return new Vector2(-f.y, f.x);
            case 4:
                return new Vector2(f.x, -f.y);
            case 5:
                return new Vector2(f.y, -f.x);
            case 6:
                return new Vector2(-f.x, -f.y);
            case 7:
                return new Vector2(-f.y, -f.x);
        }
		return f;		
	}
}
