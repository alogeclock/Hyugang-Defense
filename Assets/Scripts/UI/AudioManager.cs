using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public AudioSource audioSource;
    public AudioClip winSound;
    public AudioClip loseSound;

    private void Awake()
    {
        // Set up singleton instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one AudioManager exists
            return;
        }

        DontDestroyOnLoad(gameObject); // Prevent destruction when changing scenes

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on AudioManager.");
        }
    }

    void Start() {
        AudioManager.instance.PlayBGM(Config.MenuBGM);
    }

    // Play background music (can be used for both battle and menu)
    public void PlayBGM(string path)
    {
        // Load audio clip from the Resources folder
        AudioClip ac = Resources.Load<AudioClip>(path);
        if (ac != null)
        {
            audioSource.clip = ac;
            audioSource.loop = true; // Enable looping
            audioSource.Play();
            Debug.Log($"Now playing: {ac.name}");
        }
        else
        {
            Debug.LogError($"Failed to load AudioClip at path: {path}");
        }
    }

    // Method to play a sound effect (e.g., for button clicks or actions)
    public void PlaySoundEffect(string path)
    {
        AudioClip ac = Resources.Load<AudioClip>(path);
        
        if (ac != null)
        {
            audioSource.PlayOneShot(ac);
            Debug.Log($"Played sound effect: {ac.name}");
        }
        else
        {
            Debug.LogError($"Failed to load sound effect at path: {path}");
        }
    }

    // Method to stop any playing background music
    public void StopBGM()
    {
        audioSource.Stop();
    }
}


