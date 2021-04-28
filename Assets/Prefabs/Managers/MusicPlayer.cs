using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    static MusicPlayer instance = null;

    public AudioClip startClip;
    public AudioClip gameClip;
    public AudioClip endClip;
    public AudioClip winClip;

    private AudioSource music;

    void Awake()
    {
        // If instance aldready exists, destroy the duplicate music player.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            print("Duplicate music player self-destructing!");
        }
        else // If instance doesn't exists, create the music player and assign it to "this".
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
            music = GetComponent<AudioSource>();
            music.clip = startClip;
            music.loop = true;
            music.volume = 0.05f;
            music.Play();
        }


    }

    // Use this for initialization
    void Start ()
    {



	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("MusicPlayer: loaded level " + level);

        if (music)
        {
            music.loop = true;
            music.volume = 0.05f;

            AudioClip previousClip = music.clip;

            if (level != 4)
            {
                if ((previousClip != startClip && level != 0) || (level != 0))
                {
                    music.Stop();
                }

                if (level == 0 && previousClip != startClip)
                {
                    music.Stop();
                }
            }

            if (level == 0)
            {
                music.clip = startClip;
            }

            if (level == 1)
            {
                music.clip = gameClip;
            }

            if (level == 2)
            {
                music.clip = endClip;
            }

            if (level == 3)
            {
                music.clip = winClip;
            }

            if (level != 4)
            {
                if ((previousClip != startClip && level != 0) || (level != 0))
                {
                    music.Play();
                }

                if(level == 0 && previousClip != startClip)
                {
                    music.Play();
                }
            }
        }
    }
}
