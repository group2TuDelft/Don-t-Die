using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance; //Unique reference zodat bij nieuwe scenes niet heel de tijd een niewe audiomanager wordt aangemaakt
	// Use this for initialization
	void Awake () {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.name = s.name;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
		
	}
	
	public void Play (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "Not found");
            return;
        }
        s.source.Play();
		
	}

    public void RandomPlay(string name)
    {
        System.Collections.Generic.List<Sound> soundlist = new System.Collections.Generic.List<Sound>();
        for(int x=1; x<20; x++)
        {
        Sound randomsound = Array.Find(sounds, sound => sound.name == (name + x.ToString()));
        if (randomsound == null)
            {
                break;
            }
        soundlist.Add(randomsound);
        }

        if (soundlist.Count == 0)
        {
            Debug.LogWarning("Sound containing: " + name + "Not found");
            return;
        }
        int index = UnityEngine.Random.Range(0, soundlist.Count);
        //Debug.Log(soundlist.Count);
        Sound s = soundlist[index];
        //Debug.Log(s.name);
        s.source.Play();

    }
}
