using UnityEngine;
using System.Collections;

public class PermaSwitchSolver : MonoBehaviour
{

    private ChestScript chest;
    private PermaSwitch[] switches;

    void Start ()
	{
	    chest = transform.parent.GetComponentInChildren<ChestScript>();
    }
	
	// Update is called once per frame
	void Update ()
	{
		if (!chest.unlocked) {
			switches = transform.parent.GetComponentsInChildren<PermaSwitch> ();

			foreach (var s in switches) {
				if (s.tag == "Switch" && !s.triggered) {
					return;
				}
			}

			chest.unlocked = true;
			FindObjectOfType<Overlay>().ChestOpenText();
		}
	}
}
