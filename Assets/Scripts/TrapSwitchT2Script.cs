using UnityEngine;
using System.Collections;

public class TrapSwitchT2Script : MonoBehaviour {

	public Sprite pit;
	public bool triggered;
	private GameObject sw;
	AudioSource audio;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<BoxCollider2D> ().enabled = false;	
		triggered = false;
		audio = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

	    if (!triggered)
	    {
	        sw = GameObject.Find("trap1");
	        if (sw != null)
	        {
	            if (sw.GetComponent<Switch>().triggered)
	            {
	                triggered = true;
	                GetComponent<SpriteRenderer>().sprite = pit;
	                gameObject.GetComponent<BoxCollider2D>().enabled = true;
	            }
	        }
	    }
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (!audio.isPlaying)
        {
            audio.Play();
        }

        if (other.gameObject.tag == "Player")
        {
			other.transform.position = transform.parent.parent.Find("Puzzle3").Find("Spawn").position;
            transform.parent.GetComponentInChildren<CloneDarkness>().Trigger(true);		
		}
	}
}
