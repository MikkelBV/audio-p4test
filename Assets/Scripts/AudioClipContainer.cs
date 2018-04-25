using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipContainer : MonoBehaviour {
	public AudioClip clip;

	public AudioClip GetAudioClip() 
	{
		return this.clip;
	}
}
