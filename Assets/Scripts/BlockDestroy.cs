using UnityEngine;
    /// <summary>Removes this block after another game object activates it.</summary>

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
