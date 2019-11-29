using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] Slider mouseSens;

    private float xAxisClamp;
    [SerializeField] private Transform playerBody;

    private void Awake(){
        xAxisClamp = 0.0f;
    }
    private void FixedUpdate(){
        CameraRotation();
    }

    private void CameraRotation(){
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSens.value * 100;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSens.value * 100;

        xAxisClamp += mouseY;
        if(xAxisClamp > 90.0f){
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            clampXAxisRotationToValue(270.0f);
        }
        else if(xAxisClamp < -90.0f){
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            clampXAxisRotationToValue(90.0f);
        }

        this.transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void clampXAxisRotationToValue(float value){
        Vector3 eulerRotation = this.transform.eulerAngles;
        eulerRotation.x = value;
        this.transform.eulerAngles = eulerRotation;
    }
}
