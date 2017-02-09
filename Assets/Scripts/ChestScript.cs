using UnityEngine;
using System.Collections;

public class ChestScript : MonoBehaviour {

	public Sprite open;
	public bool unlocked = false;
    public bool openned = false;
	//GameObject[] stuff;
	//int capacity;

    private Altar1 altar;

	// Use this for initialization
	void Start ()
	{
	    altar = FindObjectOfType<Altar1>();
		//capacity = 1;
		//stuff = new GameObject[capacity];
		//for (int i = 0; i<capacity; i++) {
		//	stuff [i] = GameObject.FindGameObjectWithTag ("Reward");
		//}
	}
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if (Input.GetButtonUp ("Action")) {

				//if (!unlocked && te.getReward[te.playerRoom()]) {

				//	//transfer contents from chest to player inventory
				//	te.getInventory()[0] = stuff[0];
				//	/*
				//	if (capacity != 0) {
				//		for (int i = 0; i < capacity; i++) {
				//			te.inventory[te.inventory.Length + i] = stuff[i];
				//			stuff[i] = null;
				//		}
				//		capacity = 0;
				//	}*/

				//	unlocked = true;
				//	this.gameObject.GetComponent<SpriteRenderer>().sprite = open;	

				//}

			    if ( unlocked )
			    {
                    gameObject.GetComponent<SpriteRenderer>().sprite = open;
			        if (altar != null)
			        {
			            altar.AddItem(gameObject);
			        }
			        openned = true;
			    }
            }			
		}
	}
}
