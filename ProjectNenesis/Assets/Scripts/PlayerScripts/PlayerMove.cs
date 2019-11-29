using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController characterController;

    //Set movementspeed in Unity editor
    [SerializeField] private float movementSpeed;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Screen.SetResolution(1680, 1050, true);
    }

    // Update is called once per frame
    void Update(){
        playerMovement();
    }

    void playerMovement()
    {
        //Set horizontal inputs and movement speed
        float horizontalInput = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;

        //Set vertical inputs and movement speed
        float verticalInput = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

        //Set vertical movement vector
        Vector3 forwardMovement = this.transform.forward * verticalInput;

        //Set horizontal movement vector
        Vector3 rightMovement = this.transform.right * horizontalInput;

        //Move the player character
        characterController.SimpleMove(forwardMovement + rightMovement);
    }
}
