using UnityEngine;
using System.Collections;

public class TempleSolver : MonoBehaviour {

	private GameObject clone;
    private bool triggered;
	public GameObject sister;
	
	// Use this for initialization
	void Start () {
		clone = transform.parent.Find("Clone").gameObject;
	    triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (!triggered)
	    {
	        ChestScript[] rewards = FindObjectsOfType<ChestScript>();
	        if (rewards[0].openned && rewards[1].openned && rewards[2].openned && rewards[3].openned)
	        {
                Debug.Log("Temple 2 Solved");
	            triggered = true;
	            //destroy clone, create sister
	            Destroy(clone);
	            Vector3 vec3 = transform.parent.FindChild("PitSpawnClone").position;
	            GameObject s = (GameObject) Instantiate(sister, new Vector3(vec3.x, vec3.y, -1.0f), transform.rotation);
	            s.transform.parent = gameObject.transform;
	        }
	    }
	}
}
