using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetSound : MonoBehaviour {

    public AudioClip[] clips;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
