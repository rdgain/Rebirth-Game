using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	// Use this for initialization
	void Awake ()
	{
	    transform.position = new Vector3(transform.position.x, transform.position.y, -1.0f);

	    Player player = FindObjectOfType<Player>();

	    if (player.PositionInitialized())
	    {
	        player.transform.position = transform.position;
	    }
	}

}
