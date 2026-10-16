using UnityEngine;
    /// <summary>Implements clone movement, animation, and position control.</summary>

public class ShadowPlayer : MonoBehaviour
{

    private Animator anim_controller;

    public float speed = 1;
    private Vector3 velocity;
    private bool frozen;

    // Use this for initialization
    void Awake()
    {
        anim_controller = GetComponent<Animator>();
        frozen = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        if (FindObjectOfType<MapEngine>().ActiveRoom() == transform.parent.gameObject.name)
        {
            if ( frozen )
            {
                SetAnimation(velocity);
                frozen = false;
            }
            transform.position += speed * velocity;
        }
        else
        {
            SetAnimation(new Vector3(0, 0, 0));
            frozen = true;
        }
    }

    public void SetAnimation(Vector3 velocity)
    {
        if (velocity.x > 0)
        {
            anim_controller.SetBool("right", true);
            anim_controller.SetBool("left", false);
            anim_controller.SetBool("up", false);
            anim_controller.SetBool("down", false);
        }
        else if (velocity.x < 0)
        {
            anim_controller.SetBool("right", false);
            anim_controller.SetBool("left", true);
            anim_controller.SetBool("up", false);
            anim_controller.SetBool("down", false);
        }
        else if (velocity.y > 0)
        {
            anim_controller.SetBool("right", false);
            anim_controller.SetBool("left", false);
            anim_controller.SetBool("up", true);
            anim_controller.SetBool("down", false);
        }
        else if (velocity.y < 0)
        {
            anim_controller.SetBool("right", false);
            anim_controller.SetBool("left", false);
            anim_controller.SetBool("up", false);
            anim_controller.SetBool("down", true);

        }
        else
        {
            anim_controller.SetBool("right", false);
            anim_controller.SetBool("left", false);
            anim_controller.SetBool("up", false);
            anim_controller.SetBool("down", false);
        }
    }

    /// <summary>Sets the clone's movement vector and updates its directional animation.</summary>
    public void SetVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
        SetAnimation(velocity);
    }

    /// <summary>Returns the clone's current movement vector.</summary>
    public Vector3 GetVelocity()
    {
        return velocity;
    }
	
	/// <summary>Reverses the clone's horizontal and vertical movement.</summary>
	public void ReverseVelocity() {
		Vector3 vel = velocity;
		Vector3 temp = new Vector3 (-vel.x, -vel.y, vel.z);
		this.velocity = temp;
	}

    /// <summary>Moves the clone to a world-space position.</summary>
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    /// <summary>Returns the clone's current world-space position.</summary>
    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
