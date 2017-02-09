using UnityEngine;
using System.Collections;

public class HookScript : MonoBehaviour {
	private Player player;
	//private Clone clone;
	bool playerHere, cloneHere;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
		//clone = GameObject.Find("Clone").GetComponent<Clone>();
		playerHere = false;
		cloneHere = false;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if (cloneHere) {
				// clone already here, slide player off the tile
				player.GetComponent<Rigidbody2D>().AddForce(transform.right, ForceMode2D.Impulse);
			} else {				
				playerHere = true;
			}
		}
		if (other.gameObject.tag == "Clone") {
			if (playerHere) {
				// player already here, slide clone off the tile
				//clone.GetComponent<Rigidbody2D>().AddForce(transform.right, ForceMode2D.Impulse);
			} else {
				cloneHere = true;
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		//reset velocity when object leaves and flag it appropriately
		if (other.gameObject.tag == "Player") {
			playerHere = false;
			player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
		if (other.gameObject.tag == "Clone") {
			cloneHere = false;
			//clone.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}
}
