using UnityEngine;
using System.Collections;

public class pitTrapScript : MonoBehaviour {

	public Sprite pit;
	public bool triggered;
	public GameObject sw;
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
	        sw = GameObject.Find(name.Substring(0, 5));
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



	void OnTriggerStay2D (Collider2D other) {
		if(!audio.isPlaying)
		audio.Play ();
		if (other.gameObject.tag == "Player") {
			other.transform.position = transform.parent.parent.Find("Puzzle0").Find("Spawn").position;				
		}
	}
}
