using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour {
	private AudioSource sound;
	public bool playOnlyOnce = false;
	private bool shouldPlay = true;
	
	void Start ()
	{
		sound = GetComponent<AudioSource>();
		sound.playOnAwake = false;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (shouldPlay)
		{
			sound.Play();
			if (playOnlyOnce) shouldPlay = false;
		}
	}
}
