using UnityEngine;
    /// <summary>Collects and coordinates the Switch components beneath this object.</summary>

public class allSwitches : MonoBehaviour
{

    private ChestScript chest;
    private Switch[] switches;

    void Start ()
	{
	    chest = transform.parent.GetComponentInChildren<ChestScript>();
    }
	
	// Update is called once per frame
	void Update ()
	{
		if (!chest.unlocked) {
			switches = transform.parent.GetComponentsInChildren<Switch> ();

			foreach (var s in switches) {
				if (s.tag == "Switch" && !s.triggered) {
					return;
				}
			}

			FindAnyObjectByType<Overlay>().ChestOpenText();
			chest.unlocked = true;
		}
	}
}
