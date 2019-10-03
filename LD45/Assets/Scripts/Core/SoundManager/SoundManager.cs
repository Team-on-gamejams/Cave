using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {
	const byte maxSimultaneouslySounds = 5;
	AudioSource[] sounds = new AudioSource[maxSimultaneouslySounds];

	protected SoundManager() { }

	void Awake() {
		for(byte i = 0; i < maxSimultaneouslySounds; ++i) {
			GameObject soundgo = new GameObject();
			soundgo.transform.parent = transform;
			soundgo.name = $"sounds[{i}]";
			sounds[i] = soundgo.AddComponent<AudioSource>();
		}
	}

	public void PlaySound(AudioClip clip) {
		sounds[0].PlayOneShot(clip);
	}
}
