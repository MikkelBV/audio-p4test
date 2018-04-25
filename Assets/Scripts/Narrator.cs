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
			audioSource.clip = other.gameObject.GetComponent<AudioClipContainer>().GetAudioClip();
			audioSource.Play();
		}
	}
}
