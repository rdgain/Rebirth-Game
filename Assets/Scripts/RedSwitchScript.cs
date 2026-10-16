using UnityEngine;
    /// <summary>Handles activation and deactivation of the red switch.</summary>

public class RedSwitchScript : MonoBehaviour {

	public bool triggered;
	public Sprite norm;
	public Sprite pressed;
	SpriteRenderer spriteRend;
	private GreenSwitchScript switchG;
	private BlueSwitchScript switchB;
	private TrapSwitchT2Script trapSwitch;

	void Start () {
		spriteRend = this.GetComponent<SpriteRenderer> ();
		deactivate();
		switchG = GameObject.Find("switchG").GetComponent<GreenSwitchScript> ();
		switchB = GameObject.Find("switchB").GetComponent<BlueSwitchScript> ();
		trapSwitch = GameObject.Find("trap1").GetComponent<TrapSwitchT2Script> ();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (!triggered) {
			if (!switchG.triggered && !switchB.triggered)
				activate();
			else
				ResetSequence();
		}
	}

	private void ResetSequence() {
		deactivate();
		switchG.deactivate();
		switchB.deactivate();
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
