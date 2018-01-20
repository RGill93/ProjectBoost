using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 MovementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;
        
    float MovementFactor; // 0 for not moved, 1 for fully moved    
    Vector3 StartingPosition; // stored for absolute movement

    // Use this for initialization
    void Start ()
    {
        StartingPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {   
        if(period <= Mathf.Epsilon)
        {
            return;
        }

        float cycles = Time.time / period; // grows continually from 0

        const float tau = Mathf.PI * 2; // about 6.2
        float RawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to +1

        MovementFactor = RawSinWave / 2f + 0.5f;
        Vector3 offset = MovementVector * MovementFactor;
        transform.position = StartingPosition + offset;
	}
}
