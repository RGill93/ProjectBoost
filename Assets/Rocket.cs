using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{   

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float MainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource audioSource;

	// Use this for initialization
	void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update()
    {
        Thrust();
        Rotate();
	}

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                Console.WriteLine("okay");
                break;            
            default:
                Console.WriteLine("Dead");
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * MainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    /*
     * A function to handle the rocket input movement
     */
    private void Rotate()
    {

        // Takes manual control of the rotation
        rigidBody.freezeRotation = true;       
        float RotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {           
            transform.Rotate(Vector3.forward * RotationThisFrame);
        }
        else if(Input.GetKey(KeyCode.D))
        {            
            transform.Rotate(-Vector3.forward * RotationThisFrame);
        }

        // Resume physics control of rotation
        rigidBody.freezeRotation = false;
    }
} 
