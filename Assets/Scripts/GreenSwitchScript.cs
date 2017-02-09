using UnityEngine;
using System.Collections;

public class GreenSwitchScript : MonoBehaviour {

	public string name;
	public bool triggered;
	public Sprite norm;
	public Sprite pressed;
	SpriteRenderer spriteRend;
	private RedSwitchScript switchR;
	private BlueSwitchScript switchB;
	private TrapSwitchT2Script trapSwitch;

	void Start () {
		spriteRend = this.GetComponent<SpriteRenderer> ();
		deactivate();
		name = gameObject.name;
		switchR	= GameObject.Find("switchR").GetComponent<RedSwitchScript> ();
		switchB = GameObject.Find("switchB").GetComponent<BlueSwitchScript> ();
		trapSwitch = GameObject.Find("trap1").GetComponent<TrapSwitchT2Script> ();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (!other.name.Equals ("Clone")) {
			if (!triggered) {
				activate ();
				if (switchB.triggered)
					switchB.deactivate ();
			}
		}
	}
	
	public void activate() {		
		triggered = true;
		spriteRend.sprite = pressed;
	}
	
	public void deactivate() {		
		triggered = false;
		spriteRend.sprite = norm;
	}
}
