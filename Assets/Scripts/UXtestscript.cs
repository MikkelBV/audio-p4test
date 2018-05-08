using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UXtestscript : MonoBehaviour {
	public List<TestClip> Sounds;
	private Queue<TestClip> soundQueue;

	void Start () {
		soundQueue = new Queue<TestClip>(Sounds);
	}
	
	void Update () {
		if (Input.GetKeyDown("space")) PlayNext();
	}

	void PlayNext() {
		if (soundQueue.Count < 1) 
		{
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}

		TestClip sound = soundQueue.Dequeue();
		sound.clip.Play();
		Debug.Log(sound.message);
	}
}

[System.Serializable]
public struct TestClip {
	public AudioSource clip;
	public string message;
}

