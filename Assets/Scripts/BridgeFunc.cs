using UnityEngine;
using System.Collections;

public class BridgeFunc : MonoBehaviour {

    private Switch[] switches;
    private PermaSwitch[] permaSwitches;
	private int bridge3; //keep track of which switch triggered bridge1
	private int bridge1; //keep track of which switch triggered bridge3
	
	// Use this for initialization
	void Start () {
		bridge3 = 0;
		bridge1 = 0;
		transform.parent.GetComponentInChildren<ChestScript>().unlocked = true;		
	}
	
	// Update is called once per frame
	void Update () {
        switches = transform.parent.GetComponentsInChildren<Switch>();
        permaSwitches = transform.parent.GetComponentsInChildren<PermaSwitch>();
		foreach (var s in permaSwitches){
            if (s.tag == "Switch" && s.triggered) {
				BridgeScript bs;
				switch (s.name) {
					case "switchPerma1":
						bs = GameObject.Find ("bridge5").GetComponent<BridgeScript> ();
						bs.activate ();
						break;
					case "switchPerma2":
						bs = GameObject.Find ("bridge2").GetComponent<BridgeScript> ();
						bs.activate ();
						break;
					default :
						break;
				}
	        }
		} 
		foreach (var s in switches){
			if (s.tag == "Switch")
				if (s.triggered) {
					BridgeScript bs;
					switch (s.name) {
						case "switch1":
							bs = GameObject.Find ("bridge3").GetComponent<BridgeScript> ();
							bs.activate ();
							bridge3 = 1;
							break;
						case "switch2":
							bs = GameObject.Find ("bridge1").GetComponent<BridgeScript> ();
							bs.activate ();
							bridge1 = 2;
							break;
						case "switch3":
							bs = GameObject.Find ("bridge3").GetComponent<BridgeScript> ();
							bs.activate ();
							bridge3 = 3;
							break;
						case "switch4":
							bs = GameObject.Find ("bridge1").GetComponent<BridgeScript> ();
							bs.activate ();
							bridge1 = 4;
							break;
						case "switch5":
							bs = GameObject.Find ("bridge6").GetComponent<BridgeScript> ();
							bs.activate ();
							break;
						case "switch6":
							GameObject t = GameObject.Find ("BlockDestroy");
							GameObject.Destroy(t);
							break;
						case "switch7":
							bs = GameObject.Find ("bridge1").GetComponent<BridgeScript> ();
							bs.activate ();
							bridge1 = 7;
							break;
						case "switch8":
							bs = GameObject.Find ("bridge4").GetComponent<BridgeScript> ();
							bs.activate ();
							break;
						default :
							break;
					}
				} else {
					BridgeScript bs;
					switch (s.name) {
						case "switch1":
							if (bridge3 == 1) {
								bs = GameObject.Find ("bridge3").GetComponent<BridgeScript> ();
								bs.deactivate (); 
								bridge3 = 0;
							}
							break;
						case "switch2":
							if (bridge1 == 2) {
								bs = GameObject.Find ("bridge1").GetComponent<BridgeScript> ();
								bs.deactivate ();
								bridge1 = 0;
							}
							break;
						case "switch3":
							if (bridge3 == 3) {
								bs = GameObject.Find ("bridge3").GetComponent<BridgeScript> ();
								bs.deactivate ();
								bridge3 = 0;
							}
							break;
						case "switch4":
							if (bridge1 == 4) {
								bs = GameObject.Find ("bridge1").GetComponent<BridgeScript> ();
								bs.deactivate ();
								bridge1 = 0;
							}
							break;
						case "switch5":
							bs = GameObject.Find ("bridge6").GetComponent<BridgeScript> ();
							bs.deactivate ();
							break;
						case "switch7":
							if (bridge1 == 7) {
								bs = GameObject.Find ("bridge1").GetComponent<BridgeScript> ();
								bs.deactivate ();
								bridge1 = 0;
							}
							break;
						case "switch8":
							bs = GameObject.Find ("bridge4").GetComponent<BridgeScript> ();
							bs.deactivate ();
							break;
						default :
							break;
					}
				}
		}
	
	}
}
