using UnityEngine;
    /// <summary>Places the player at the room spawn position during initialization.</summary>

public class Spawn : MonoBehaviour {

	// Use this for initialization
	void Awake ()
	{
	    transform.position = new Vector3(transform.position.x, transform.position.y, -1.0f);

	    Player player = FindAnyObjectByType<Player>();

	    if (player.PositionInitialized())
	    {
	        player.transform.position = transform.position;
	    }
	}

}
