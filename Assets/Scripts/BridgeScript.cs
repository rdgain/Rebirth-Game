using UnityEngine;
using System.Collections;

public class BridgeScript : MonoBehaviour
{
	public string name;
    private MapEngine mapEngine;
	AudioSource audio;
	public GameObject bridge;
	private bool activated, deactivated;

    void Awake()
    {
        mapEngine = FindObjectOfType<MapEngine>();
		audio = this.GetComponent<AudioSource> ();
		name = gameObject.name;
		activated = false;
		deactivated = false;
    }

	void OnTriggerStay2D(Collider2D other) {
		if (!audio.isPlaying)
			audio.Play();
		if (other.gameObject.tag == "Player")
		{
            Vector3 vec3 = transform.parent.parent.FindChild(mapEngine.ActiveRoom()).FindChild("PitSpawn").position;
            other.transform.position = new Vector3(vec3.x, vec3.y, -1.0f);
        }
		else if (other.gameObject.name == "Clone") {
			Vector3 vec3 = transform.parent.parent.FindChild(mapEngine.ActiveRoom()).FindChild("PitSpawnClone").position;
            other.transform.position = new Vector3(vec3.x, vec3.y, -1.0f);		
		}
		else if (other.gameObject.name.StartsWith("move"))
        {
			BoxCollider2D[] cols  = other.gameObject.GetComponents<BoxCollider2D> ();
			for (int i = 0; i < cols.Length; i++)
				cols[i].enabled = false;
			this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		}
	}
	
	public void activate() {
		//create block at this position
		if (!activated) {
			GameObject br = (GameObject) Instantiate(bridge, transform.position, transform.rotation);
			br.transform.parent = gameObject.transform;
			br.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			activated = true;
			deactivated = false;
		}
	}
	
	public void deactivate() {
		//destroy block and reactivate pit
		if (!deactivated) {
			foreach (Transform child in transform) {
				GameObject.Destroy(child.gameObject);
			}
			this.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
			deactivated = true;
			activated = false;
		}
	}
}
