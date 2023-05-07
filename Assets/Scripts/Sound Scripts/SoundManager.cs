using UnityEngine.Audio;
using UnityEngine;
using System;



public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;

    public static SoundManager instance;

    public bool playMenuMusic = true;

    void Awake()
    {
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
            s.srce = gameObject.AddComponent<AudioSource>();
            if (!s.multiSound)
                s.srce.clip = s.clip[0];

            s.srce.volume = s.volume;
            if(!s.pitchRandomness)
            s.srce.pitch = s.pitch;

            s.srce.loop = s.doesLoop;

            s.srce.outputAudioMixerGroup = s.outputChannel;
        }
    }

    //Plays the selected sound
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " not found. You complete buffoon.");
            return;
        }
        if(s.multiSound)
        {
            int rand = UnityEngine.Random.Range(0, s.clip.Length);
            s.srce.clip = s.clip[rand];
        }
        if(s.pitchRandomness)
        {
            float ptch = UnityEngine.Random.Range(0.75f, 1.5f);
            s.srce.pitch = ptch;
        }

        s.srce.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " not found. You stinkyhead.");
            return;
        }
        s.srce.Stop();
    }
}