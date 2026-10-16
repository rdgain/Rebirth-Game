using UnityEngine;
    /// <summary>Responds to a completed trigger condition and advances the game state.</summary>

public class BigButton : MonoBehaviour {

    private Temple temple;

	void Awake ()
	{
	    temple = FindAnyObjectByType<Temple>();
	}


	void OnTriggerStay2D (Collider2D other)
    {
		if (other.name.Equals("Player"))
		{
		    Reward[] rewards = FindObjectsByType<Reward>();
		    if (rewards[0].Gathered() && rewards[1].Gathered() && rewards[2].Gathered() && rewards[3].Gathered())
		    {
                temple.LevelFinished();
            }
		}
	}
}
