using UnityEngine;
    /// <summary>Implements main-player movement, animation, and control locking.</summary>

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
        Vector2 movement = GameplayInput.Movement;

        if (playerControl && canMove)
        {
            temp.x += speed * movement.x;
            temp.y += speed * movement.y;
			transform.position = temp;
						
			if (movement.x > 0) {
				anim_controller.SetBool("right", true);
				anim_controller.SetBool("left", false);
				anim_controller.SetBool("up", false);
				anim_controller.SetBool("down", false);
			}
            else if (movement.x < 0) {
				anim_controller.SetBool("right", false);
				anim_controller.SetBool("left", true);
				anim_controller.SetBool("up", false);
				anim_controller.SetBool("down", false);
			}
            else if (movement.y > 0) {
				anim_controller.SetBool("right", false);
				anim_controller.SetBool("left", false);
				anim_controller.SetBool("up", true);
				anim_controller.SetBool("down", false);
			}
            else if (movement.y < 0) {
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

    /// <summary>Enables or disables keyboard movement for the player.</summary>
    public void SetPlayerControl(bool toggle)
    {
        playerControl = toggle;
    }
	
	/// <summary>Reports whether keyboard movement is currently enabled.</summary>
	public bool GetPlayerControl()
    {
        return playerControl;
    }

    /// <summary>Reports whether the player's spawn position has been assigned.</summary>
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
	
	/// <summary>Moves the player to a world-space position.</summary>
	public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

	/// <summary>Returns the player's current world-space position.</summary>
	public Vector3 GetPosition()
	{
		return transform.position;
	}
}
