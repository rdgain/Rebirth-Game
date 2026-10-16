using UnityEngine;
    /// <summary>Checks the room light puzzle and updates its linked switches.</summary>

public class MirrorSolver2 : MonoBehaviour {
	public GameObject lightSink = null;
    private ChestScript chest;
    private Switch[] switches;

	void Start () {
		chest = transform.parent.GetComponentInChildren<ChestScript>();
		lightSink = transform.parent.transform.Find ("sink").gameObject;
		
		// apply rules to the room.
        RuleParser.implement(transform.parent.gameObject, FindAnyObjectByType<MapEngine>().RoomRules(transform.parent.name));
	}
	
	void Update () {

		if (!chest.unlocked) {
            if (lightSink == null || !lightSink.GetComponent<LightCheck>().inToleranceFromSource())
            {
                return;
            }

            switches = transform.parent.GetComponentsInChildren<Switch>();
            foreach (var s in switches)
            {
                if (s.tag == "Switch" && !s.triggered)
                {
                    return;
                }
            }
            chest.unlocked = true;
			FindAnyObjectByType<Overlay>().ChestOpenText();
		}
	}
}
