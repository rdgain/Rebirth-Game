using UnityEngine;
using System.Collections;

public class MovingBlock : MonoBehaviour {


	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if (Input.GetButtonUp ("Action")) {

				double x = getDirection(other.gameObject);
				int modifier;

				if (x == 2) {
					if (Input.GetAxis ("Horizontal") > 0)
						modifier = 1;
					else
						modifier = -1;
					if (checkClear ('h', modifier)) {
						Vector3 temp = transform.position;
						temp.x = temp.x + modifier;					
						transform.position = temp;
					}
				} 
				else if (x == 1) {
					if (Input.GetAxis ("Vertical") > 0)
						modifier = 1;
					else
						modifier = -1;
					if (checkClear ('v',modifier)) {
						Vector3 temp = transform.position;
						temp.y = temp.y + modifier;					
						transform.position = temp;
					}
				}
				AudioSource audio = gameObject.GetComponent<AudioSource> ();
				if (!audio.isPlaying) {
					audio.Play();
				}
			}
		}
	}

	bool checkClear(char d, int dir) {
		float radius = 0.3f;

		if (d.Equals ('h')) {
			Collider2D temp = Physics2D.OverlapCircle (new Vector2 (transform.position.x + dir, transform.position.y), radius);
			if (temp != null && temp.tag.Equals("Unpassable"))
				return false;
			else 
				return true;
				

		} else {
			Collider2D temp = Physics2D.OverlapCircle (new Vector2 (transform.position.x, transform.position.y + dir), radius);
			if (temp != null && temp.tag.Equals("Unpassable"))
				return false;
			else 
				return true;
		}

	}

	double getDirection(GameObject other) {

		if (other.transform.position.x > transform.position.x - 0.5
			&& other.transform.position.x < transform.position.x + 0.5) {
			return 1;
		}
		else if (other.transform.position.y > transform.position.y - 0.5
			&& other.transform.position.y < transform.position.y + 0.5) {
			return 2;
		}
		return 0;
	}

}
