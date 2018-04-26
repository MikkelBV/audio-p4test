using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipContainer : MonoBehaviour {
	public AudioClip clip;
	public bool playOnlyOnce = true;
	private bool shouldPlay = true;

	public AudioClip GetAudioClip() 
	{
		if (!shouldPlay) return null;
		if (playOnlyOnce) shouldPlay = false;
		
		return this.clip;
	}
}
