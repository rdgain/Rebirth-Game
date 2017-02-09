using UnityEngine;
using System.Collections;

public class TestLightSwitch : MonoBehaviour {
	public GameObject lightSink = null;

    public bool solved;

	// Use this for initialization
	void Start()
	{
	    solved = false;
	    lightSink = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (lightSink != null && lightSink.GetComponent<LightCheck>().inToleranceFromSource()) {
			Debug.Log("open sesemy");
		    solved = true;
		}
	}
}
