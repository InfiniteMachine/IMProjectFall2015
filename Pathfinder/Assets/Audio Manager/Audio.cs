using UnityEngine;
using System.Collections.Generic;

public class Audio : MonoBehaviour {
    private AudioSource aSource;

    public void PlayOneShot(AudioClip c)
    {
        PlayOneShot(c, 1);
    }

    public void PlayOneShot(AudioClip c, float volume)
    {
        aSource = gameObject.AddComponent<AudioSource>();
        aSource.volume = volume;
        aSource.clip = c;
        aSource.loop = false;
        aSource.Play();
        Invoke("StopSound", c.length);
    }

    public void PlayBackground(AudioClip c)
    {
        PlayBackground(c, 1);
    }

    public void PlayBackground(AudioClip c, float volume)
    {
        aSource = gameObject.AddComponent<AudioSource>();
        aSource.volume = volume;
        aSource.clip = c;
        aSource.loop = true;
        aSource.Play();
    }

    public void StopSound(){
        aSource.Stop();
        Destroy(gameObject);
    }
}
