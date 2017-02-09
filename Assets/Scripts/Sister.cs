using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Sister : MonoBehaviour
{
    private Temple temple;


    // Use this for initialization
    void Awake()
    {
		temple = FindObjectOfType<Temple>();	
    }

    // Update is called once per frame
    void Update()
    {
    }
	
	void OnTriggerStay2D (Collider2D other) {
		if (other.name == "Player")
			temple.LevelFinished();
	}


    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

}
