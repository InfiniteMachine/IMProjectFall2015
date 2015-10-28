using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
    
    private static AudioManager instance;
    public static AudioManager GetInstance(){
        return instance;
    }

    public AudioClip[] soundEffects;
    private Dictionary<int, Audio> activeClips;
    private int nextID = 1000;

    public GameObject audioPrefab;

	// Use this for initialization
	void Start () {
	    activeClips = new Dictionary<int,Audio>();
        if(instance != null){
            Debug.Log("AudioManager: There are more than one AudioManagers in the scene");
        }else{
            instance = this;
        }
	}
	
	public int PlaySound(string name, bool background)
    {
        return PlaySound(name, background, 1);
    }

    public int PlaySound(string name, bool background, float volume)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            if (soundEffects[i].name == name)
            {
                return PlaySound(i, background, volume);
            }
        }
        Debug.Log("AudioManager: The sound '" + name + "' does not exist.");
        return -1;
    }

    public int PlaySound(int location, bool background)
    {
        return PlaySound(location, background, 1);
    }

    public int PlaySound(int location, bool background, float volume)
    {
        if (location < 0 || location > soundEffects.Length)
        {
            Debug.Log("AudioManager: The desired index is out of range.");
            return -1;
        }
        GameObject go = (GameObject)GameObject.Instantiate(audioPrefab);
        go.transform.SetParent(transform);
        Audio a = go.GetComponent<Audio>();
        if (background)
        {
            a.PlayBackground(soundEffects[location], volume);
        }
        else
        {
            a.PlayOneShot(soundEffects[location], volume);
        }
        activeClips[nextID] = a;
        nextID++;
        return nextID - 1;
    }

    public void StopSound(int id)
    {
        if(activeClips.ContainsKey(id)){
            if (activeClips[id] != null)
            {
                activeClips[id].StopSound();
            }
        }
    }

    public void StopAllSounds()
    {
        foreach(Audio a in activeClips.Values){
            a.StopSound();
        }
    }
}