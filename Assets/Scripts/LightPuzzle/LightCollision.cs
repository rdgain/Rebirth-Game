using UnityEngine;
using System.Collections;

public class LightCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnParticleCollision(GameObject other) {
		//Debug.Log (other);
		ParticleSystem ps = gameObject.GetComponent<ParticleSystem> ();
		ParticleSystem.Particle[] particals = new ParticleSystem.Particle[ps.particleCount];
		int count = ps.GetParticles(particals);
		Bounds bounds = other.GetComponent<Collider> ().bounds;

		for (int i = 0; i < count; i++) {
			Vector3 pos = transform.TransformPoint(particals[i].position);

			if((other.GetComponentInParent<LightCheck>() == null || other.transform.parent.tag == "LightSink" || other.tag == "LightSink") &&
			   		Vector2.Distance(bounds.ClosestPoint(pos), pos) <= 0.1f)
			{ // if partical is within other's collider.
				particals[i].lifetime = 0;
			}
		}
		ps.SetParticles (particals, count);
	}
}
