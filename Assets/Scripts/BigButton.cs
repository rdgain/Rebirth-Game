using UnityEngine;
using System.Collections;

public class BigButton : MonoBehaviour {

    private Temple temple;

	void Awake ()
	{
	    temple = FindObjectOfType<Temple>();
	}


	void OnTriggerStay2D (Collider2D other)
    {
		if (other.name.Equals("Player"))
		{
		    Reward[] rewards = FindObjectsOfType<Reward>();
		    if (rewards[0].Gathered() && rewards[1].Gathered() && rewards[2].Gathered() && rewards[3].Gathered())
		    {
                temple.LevelFinished();
            }
		}
	}
}
