using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource _audio;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void OnPlay(AudioClip clip)
    {
        _audio.PlayOneShot(clip);
    }
  }
