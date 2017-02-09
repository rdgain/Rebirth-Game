using UnityEngine;
using System.Collections;

public class SlideUpScript : MonoBehaviour {
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
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.up, ForceMode2D.Impulse);
        }
	}
}
