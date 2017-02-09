using System;
using UnityEngine;
using System.Collections;

public class Altar1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddItem(GameObject go)
    {
        float tx = transform.position.x;
        float ty = transform.position.y;

        float cx = go.transform.position.x;
        float cy = go.transform.position.y;

        if (Math.Abs(ty-cy) < Math.Abs(tx-cx))
        {
            if (tx < cx)
            {
                transform.FindChild("right_carpet").GetComponent<Reward>().Found();
            }
            else
            {
                transform.FindChild("left_carpet").GetComponent<Reward>().Found();
            }
        }
        else
        {
            if (ty < cy)
            {
                transform.FindChild("top_carpet").GetComponent<Reward>().Found();
            }
            else
            {
                transform.FindChild("bottom_carpet").GetComponent<Reward>().Found();
            }
        }
    }
}
