using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	Player player;
	GameObject clone;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player>();
		clone = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetPlayerControl())	
			transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, -10);
		else 
			if (clone != null)
				transform.position = new Vector3 (clone.transform.position.x, clone.transform.position.y, -10);
	}
	
	public void SetClone(GameObject c) {
		clone = c;
	}
}
