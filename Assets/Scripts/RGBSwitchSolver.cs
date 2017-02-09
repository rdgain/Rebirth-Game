using UnityEngine;
using System.Collections;

public class RGBSwitchSolver : MonoBehaviour {
	private RedSwitchScript switchR;
	private GreenSwitchScript switchG;
	private BlueSwitchScript switchB;
    private ChestScript chest;

	// Use this for initialization
	void Start () {
		switchR = GameObject.Find("switchR").GetComponent<RedSwitchScript> ();	
		switchG = GameObject.Find("switchG").GetComponent<GreenSwitchScript> ();	
		switchB = GameObject.Find("switchB").GetComponent<BlueSwitchScript> ();	
        chest = transform.parent.GetComponentInChildren<ChestScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!chest.unlocked )
		{
			if (switchB.triggered && switchR.triggered && switchB.triggered) {
				chest.unlocked = true;
				FindObjectOfType<Overlay>().ChestOpenText();
			}
		}
	
	}
}
