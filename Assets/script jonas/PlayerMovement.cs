using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Ability ability;

    public float runSpeed = 40;
    public float runFaster = 70;

    float cooldown = 2f;
    float nextActivatetime = 0f;
    float activatetime = 0f;
    public float dashTime = 0.1f;
    public float cooldownDash = 5f;
    public float dash = 10;

    float horizontalMove = 0;
    bool jump = false;
    bool crouch = false;



    void Update()
    {
        if(ability.isAktive("Run")){
            horizontalMove = Input.GetAxisRaw("Horizontal") * runFaster;
        }else{
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        }

        if(ability.isAktive("Dash")){
			cooldown = cooldownDash;
        }

        if(ability.isAktive("Dash")){
            if(Time.time >= nextActivatetime){
                if(Input.GetButtonDown("Fire1")){
                    print("2");
                    horizontalMove *= dash;
                    nextActivatetime = Time.time + cooldown;
                    activatetime = Time.time + dashTime;
                }
            }else if(Time.time <= activatetime){
                horizontalMove *= dash;
            }
        }

        if(Input.GetButtonDown("Jump")){
            jump = true;
        }
        if(Input.GetButtonDown("Crouch")){
            crouch = true;
        }else if (Input.GetButtonUp("Crouch")){
            crouch = false;
        }
    }

    void FixedUpdate(){
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;

    }
}
