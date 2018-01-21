using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    /** 
     * these handle the rocket thrust and movement
     * also handles a level load delay
     * @see StartLevelCompleteSequence() and StartDeathSequence()
     * @see Thrust() and Rotate()
     */
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float MainThrust = 100f;
    [SerializeField] float LevelLoadDelay = 2f;

    /** 
     * these handle the sound
     * @see StartLevelCompleteSequence() and StartDeathSequence()
     */
    [SerializeField] AudioClip MainEngine;
    [SerializeField] AudioClip LevelComplete;
    [SerializeField] AudioClip DeathSound;      

    /**
     * Particle systems the rocket will have upon 
     * either death or completion
     * "MainEngine" refers to when the character is playing
     */
    [SerializeField] ParticleSystem MainEngineParticles;
    [SerializeField] ParticleSystem LevelCompleteParticles;
    [SerializeField] ParticleSystem DeathParticles;    

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State
    {
        Alive,

        Dying,

        Transending
    }

    State state = State.Alive;

    bool CollisionsDisabled = false;

	// Use this for initialization
	void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update()
    {
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        }

        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
	}

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            CollisionsDisabled = !CollisionsDisabled;
        }
    }

    /*A function to handle all the collision*/
    private void OnCollisionEnter(Collision collision)
    {
        // ignore collision when dead
        if(state != State.Alive || CollisionsDisabled)
        {
            return;
        }

        switch(collision.gameObject.tag)
        {        
            case "Friendly":               
                break;
            case "Finish":
                StartLevelCompleteSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
        
    }

    /*A Method for handling the level completion sequence*/
    private void StartLevelCompleteSequence()
    {
        state = State.Transending;
        audioSource.Stop();
        audioSource.PlayOneShot(LevelComplete);
        LevelCompleteParticles.Play();
        Invoke("LoadNextLevel", LevelLoadDelay);
    }

    /*A Method for handling the level death sequence*/
    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(DeathSound);
        DeathParticles.Play();
        Invoke("LoadFirstLevel", LevelLoadDelay);
    }

    /*A function for loading the level on completion*/
    private void LoadNextLevel()
    {
        int CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int NextSceneIndex = CurrentSceneIndex + 1;

        if(NextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            NextSceneIndex = 0; // loops back to start
        }

        SceneManager.LoadScene(NextSceneIndex);       
    }

    /*A function for loading the first level on death*/
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);        
    }
  
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            MainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * MainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(MainEngine);
        }
        MainEngineParticles.Play();
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
