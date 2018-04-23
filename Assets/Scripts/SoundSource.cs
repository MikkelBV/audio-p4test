using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour {
	private AudioSource sound;
	public bool playOnlyOnce = false;
	private bool shouldPlay = true;
    private bool isSrcVisible;
	
	void Start ()
	{
		sound = GetComponent<AudioSource>();
		sound.playOnAwake = false;
	}

    private void OnBecameVisible()
    {
        isSrcVisible = true;
    }

    private void OnBecameInvisible()
    {
        isSrcVisible = false;
    }

    void OnTriggerEnter(Collider other)
	{

        if (isSrcVisible)
        { 
            if (shouldPlay)
            {
                sound.Play();
                if (playOnlyOnce) shouldPlay = false;
            }
        }
	}
}
