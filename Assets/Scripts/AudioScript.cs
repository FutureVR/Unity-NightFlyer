using UnityEngine;
using System.Collections;

public class AudioScript : MonoBehaviour {

    public AudioClip start;
    public AudioClip loop;
    public AudioClip winSound;

    AudioSource audioSource;
    
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();

        if (start != null)
        {
            audioSource.clip = start;
            audioSource.Play();
            audioSource.loop = false;
        }
	}
	

	void Update ()
    {
	    if (audioSource.isPlaying == false)
        {
            if (loop != null)
            {
                audioSource.clip = loop;
                audioSource.loop = true;
            }
        }
	}

    public void playWin()
    {
        if (winSound != null)
        {
            //audioSource
        }
    }
}
