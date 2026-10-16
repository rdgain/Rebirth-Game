using UnityEngine;
    /// <summary>Handles collisions with the pit and applies the associated game effect.</summary>

public class PitScript : MonoBehaviour
{
    private MapEngine mapEngine;
	AudioSource audio;

    void Awake()
    {
        mapEngine = FindAnyObjectByType<MapEngine>();
		audio = this.GetComponent<AudioSource> ();
    }

	void OnTriggerStay2D(Collider2D other) {
		if (!audio.isPlaying)
			audio.Play();
		if (other.gameObject.tag == "Player")
		{
            Vector3 vec3 = transform.parent.parent.Find(mapEngine.ActiveRoom()).Find("PitSpawn").position;
            other.transform.position = new Vector3(vec3.x, vec3.y, -1.0f);
        }
		else if (other.gameObject.name == "Clone") {
			Vector3 vec3 = transform.parent.parent.Find(mapEngine.ActiveRoom()).Find("PitSpawnClone").position;
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
}
