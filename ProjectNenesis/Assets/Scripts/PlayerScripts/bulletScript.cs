using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    private Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        rb.velocity = transform.forward * 100;
    }
}
