using UnityEngine;
    /// <summary>Coordinates the permanent switches that solve its room puzzle.</summary>

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
			FindAnyObjectByType<Overlay>().ChestOpenText();
		}
	}
}
