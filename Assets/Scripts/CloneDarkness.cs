using UnityEngine;
using System.Collections;

public class CloneDarkness : MonoBehaviour
{

    GameObject onSwitch, offSwitch;
    Light MCC, MCP;
    private bool state;

    // Use this for initialization
    void Start()
    {

        offSwitch = GameObject.Find("Lswitch2");
        onSwitch = GameObject.Find("Lswitch1");
        MCC = transform.parent.Find("Clone").GetComponentInChildren<Light>();
        MCP = GameObject.Find("Player").GetComponentInChildren<Light>();
        state = true;
    }

    public void Trigger(bool state)
    {
        this.state = state;
    }

    // Update is called once per frame
    void Update()
    {
        if (offSwitch.GetComponent<Switch>().triggered)
        {
            state = false;
        }
        if (onSwitch.GetComponent<Switch>().triggered)
        {
            state = true;
        }

        if (!state)
        {
            MCC.enabled = true;
            MCP.enabled = false;
        }
        else
        {
            MCC.enabled = false;
            MCP.enabled = true;
        }
    }
}
