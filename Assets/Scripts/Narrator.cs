using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narrator : MonoBehaviour {
	private AudioSource audioSource;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Narrator") 
		{
			Debug.Log("Narrator soundclip triggered");
			
			AudioClip narrationClip = other.gameObject.GetComponent<AudioClipContainer>().GetAudioClip();
			if (narrationClip == null) return;

			audioSource.clip = narrationClip;
			audioSource.Play();
		}
	}
}
