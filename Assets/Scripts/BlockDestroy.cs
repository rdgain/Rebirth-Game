using UnityEngine;
using System.Collections;

public class BlockDestroy : MonoBehaviour {
	private bool activated;
	
	void Awake() { activated = false; }
	
	void Start () {}

	void Update () {}
	
	public void activate() {
		AudioSource audio = gameObject.GetComponent<AudioSource> ();
		if (!activated) {
			if (!audio.isPlaying) {
				audio.Play();
			}
			GameObject.Destroy(gameObject);
			activated = true;
		}
	}
}
