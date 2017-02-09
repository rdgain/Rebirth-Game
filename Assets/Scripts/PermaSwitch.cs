using UnityEngine;
using System.Collections;

public class PermaSwitch : MonoBehaviour {

	public string name;
	public bool triggered;
	public Sprite norm;
	public Sprite pressed;
	SpriteRenderer spriteRend;

	void Start () {
		triggered = false;
		spriteRend = this.GetComponent<SpriteRenderer> ();
		spriteRend.sprite = norm;
		name = gameObject.name;
	}

	void OnTriggerStay2D(Collider2D other) {
		triggered = true;
		spriteRend.sprite = pressed;
	}
}
