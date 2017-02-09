using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

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

    public void SetVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
        SetAnimation(velocity);
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }
	
	public void ReverseVelocity() {
		Vector3 vel = velocity;
		Vector3 temp = new Vector3 (-vel.x, -vel.y, vel.z);
		this.velocity = temp;
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
