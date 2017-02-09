using UnityEngine;
using System.Collections;

public class DarknessScript : MonoBehaviour {

	GameObject onSwitch, offSwitch;
	Light MC;
    bool state;

	// Use this for initialization
	void Start () {

		offSwitch = GameObject.Find("Lswitch2");
		onSwitch = GameObject.Find("Lswitch1");
		MC = GameObject.Find("Player").GetComponentInChildren<Light>();
	    state = true;
	}

    public void Trigger(bool state)
    {
        this.state = state;
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (offSwitch.GetComponent<Switch>().triggered)
        {
            state = false;
        }
        if (onSwitch.GetComponent<Switch>().triggered)
        {
            state = true;
        }

        if (!state)
        {
			MC.spotAngle = 16;
		}
        else
        {
			MC.spotAngle = 100;
		}
	}
}
