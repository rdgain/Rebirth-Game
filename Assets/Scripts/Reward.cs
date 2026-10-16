using UnityEngine;
    /// <summary>Tracks whether this reward has been collected.</summary>

public class Reward : MonoBehaviour
{

    private bool gathered;

	// Use this for initialization
	void Awake () {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
	    gathered = false;
	}
	
	// Update is called once per frame
	public void Found()
    {
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            gathered = true;
    }

    public bool Gathered()
    {
        return gathered;
    }
}
