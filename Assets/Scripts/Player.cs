using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : MonoBehaviour
{

	private Animator anim_controller;

    public float speed = 1;
    private bool playerControl;
    private bool posInit;
	
	public bool canMove;

	// Use this for initialization
	void Awake ()
	{
		canMove = true;
		anim_controller = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate()
    {

        Vector3 temp = transform.position;

        if (playerControl && canMove)
        {
            temp.x += (speed * Input.GetAxis("Horizontal"));
            temp.y += (speed * Input.GetAxis("Vertical"));
			transform.position = temp;
						
			if (Input.GetAxis("Horizontal") > 0) {
				anim_controller.SetBool("right", true);
				anim_controller.SetBool("left", false);
				anim_controller.SetBool("up", false);
				anim_controller.SetBool("down", false);
			}
            else if (Input.GetAxis("Horizontal") < 0) {
				anim_controller.SetBool("right", false);
				anim_controller.SetBool("left", true);
				anim_controller.SetBool("up", false);
				anim_controller.SetBool("down", false);
			}
            else if (Input.GetAxis("Vertical") > 0) {
				anim_controller.SetBool("right", false);
				anim_controller.SetBool("left", false);
				anim_controller.SetBool("up", true);
				anim_controller.SetBool("down", false);
			}
            else if (Input.GetAxis("Vertical") < 0) {
				anim_controller.SetBool("right", false);
				anim_controller.SetBool("left", false);
				anim_controller.SetBool("up", false);
				anim_controller.SetBool("down", true);
			} else {
				anim_controller.SetBool("right", false);
				anim_controller.SetBool("left", false);
				anim_controller.SetBool("up", false);
				anim_controller.SetBool("down", false);
			}
        }		
	}

    public void SetPlayerControl(bool toggle)
    {
        playerControl = toggle;
    }
	
	public bool GetPlayerControl()
    {
        return playerControl;
    }

    public bool PositionInitialized()
    {
        if (posInit)
        {
            return false;
        }
        else
        {
            posInit = true;
            return posInit;
        }
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
