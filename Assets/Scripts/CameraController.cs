using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");

	}
	
	// Update is called once per frame
	void Update () {

		Vector3 temp = player.transform.position;
		temp.z = transform.position.z;

		transform.position = temp;
	
	}
	
}
