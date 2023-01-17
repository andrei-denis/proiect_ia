using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    public delegate void DyingEventHandler();
    public delegate void TeleportEventHandler();

    public event Action<GameObject> OnFruitCathced;
    public event DyingEventHandler OnDying;
    public event TeleportEventHandler OnTeleport;


    [SerializeField]
    public Camera cam;
    private Animator animator;
    private float speed = 4.5f;
    public int hearts = 3;

    private void Start()
    {
        // this.cam = Camera.main;
        this.animator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        // KeyboardControl();
        CheckScreenLimits();
    }

    private void CheckScreenLimits()
	{
		//get the screen position
		Vector3 screen_pos = this.cam.WorldToScreenPoint(transform.position);
	    //Check if we are too far left
		if(screen_pos.x < 0){
			transform.position = this.cam.ScreenToWorldPoint (new Vector3 (Screen.width - 1, screen_pos.y, screen_pos.z));
            OnTeleport?.Invoke();
		}
			//check if we are too far right
		if (screen_pos.x > Screen.width){
            transform.position = this.cam.ScreenToWorldPoint (new Vector3 (1, screen_pos.y, screen_pos.z));
            OnTeleport?.Invoke();
        }
	}

    private void KeyboardControl()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Move(horizontalInput);
    }

    public void Move(float horizontalInput)
    {
        const float ignoredInput = .15f;
        
        if(horizontalInput < -ignoredInput){
            animator?.SetBool ("left", true);
            animator?.SetBool("right", false);
        }
        else if(horizontalInput > ignoredInput){
            animator?.SetBool ("left", false);
            animator?.SetBool("right", true);
        }
        else{
            StopAnimation();
        }

        transform.Translate(new Vector3(horizontalInput, 0, 0) * this.speed * Time.deltaTime);
    }

    public void StopAnimation()
    {
        animator?.SetBool ("left", false);
        animator?.SetBool("right", false);
    }

    public void TakeDamage(int damage = 1)
    {
        hearts -= damage;
        if(hearts <= 0){
            OnDying?.Invoke();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Fruit"){
            OnFruitCathced?.Invoke(other.gameObject);
        }
    }

}
