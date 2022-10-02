using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Ability ability;

    public float runSpeed = 40;
    public float runFaster = 70;

    float horizontalMove = 0;
    bool jump = false;
    bool crouch = false;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(ability.isAktive("Run")){
            horizontalMove = Input.GetAxisRaw("Horizontal") * runFaster;
        }else{
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
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
