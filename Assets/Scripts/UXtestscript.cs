using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UXtestscript : MonoBehaviour {
	public bool PlayFirstOnAwake = false;
	public List<TestClip> Sounds;
	private Queue<TestClip> soundQueue;

	void Start () {
		soundQueue = new Queue<TestClip>(Sounds);
		if (PlayFirstOnAwake) PlayNext();
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
		AudioSource.PlayClipAtPoint(sound.clip, sound.position);
		Debug.Log(sound.message);
	}
}

[System.Serializable]
public struct TestClip {
	public Vector3 position;
	public AudioClip clip;
	public string message;
}

