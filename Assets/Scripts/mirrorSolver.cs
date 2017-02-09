using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class mirrorSolver : MonoBehaviour {
	public GameObject lightSink = null;
    private ChestScript chest;

    void Start()
    {
        chest = transform.parent.GetComponentInChildren<ChestScript>();
		// set mirror solver lightSink target.
		lightSink = transform.parent.transform.Find ("lightSink").gameObject;

        // apply rules to the room.
        RuleParser.implement(transform.parent.gameObject, FindObjectOfType<MapEngine>().RoomRules(transform.parent.name));
    }

    void Update()
    {
		if (!chest.unlocked) {
			if (lightSink != null && lightSink.GetComponent<LightCheck> ().inToleranceFromSource ()) {
				chest.unlocked = true;

				FindObjectOfType<Overlay>().ChestOpenText();
			}
		}
	}
}
