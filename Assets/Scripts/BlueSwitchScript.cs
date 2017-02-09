using UnityEngine;
using System.Collections;

public class BlueSwitchScript : MonoBehaviour {

	public string name;
	public bool triggered;
	public Sprite norm;
	public Sprite pressed;
	SpriteRenderer spriteRend;
	private GreenSwitchScript switchG;
	private RedSwitchScript switchR;
	private TrapSwitchT2Script trapSwitch;

	void Start () {
		spriteRend = this.GetComponent<SpriteRenderer> ();
		deactivate();
		name = gameObject.name;
		switchG = GameObject.Find("switchG").GetComponent<GreenSwitchScript> ();
		switchR = GameObject.Find("switchR").GetComponent<RedSwitchScript> ();
		trapSwitch = GameObject.Find("trap1").GetComponent<TrapSwitchT2Script> ();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (!triggered) {
			activate();
			if (switchR.triggered && !switchG.triggered) switchR.deactivate();
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
