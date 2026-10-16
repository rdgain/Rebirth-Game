using UnityEngine;
    /// <summary>Activates permanently when a player or clone presses the switch.</summary>

public class PermaSwitch : MonoBehaviour {

	public string switchName;
	public bool triggered;
	public Sprite norm;
	public Sprite pressed;
	SpriteRenderer spriteRend;

	void Start () {
		triggered = false;
		spriteRend = this.GetComponent<SpriteRenderer> ();
		spriteRend.sprite = norm;
		switchName = gameObject.name;
	}

	void OnTriggerStay2D(Collider2D other) {
		triggered = true;
		spriteRend.sprite = pressed;
	}
}
