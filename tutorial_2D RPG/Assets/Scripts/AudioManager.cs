using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   // inspector창에 sounds를 띄우기 위해 직렬화
public class Sound
{
    public string name;    // 사운드의 이름
    public AudioClip clip; // 사운드 파일
    private AudioSource source; // 사운드 플레이어

    public float volume;
    public bool loop;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
    }

    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
    public void SetLoop()
    {
        source.loop = loop;
    }

    public void SetVolume()
    {
        source.volume = volume;
    }
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField]
    public Sound[] sounds;

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject("사운드 파일 이름 : " + i + "=" + sounds[i]);
            soundObject.transform.SetParent(this.transform);    // Hierachy의 AudioManager 밑에 들어가게 한다
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
        }
    }

    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }
    }
    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }
    public void SetLoop(string _name, bool flag)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].loop = flag;
                sounds[i].SetLoop();
                return;
            }
        }
    }

    public void SetVolume(string _name, float _volume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].volume = _volume;
                sounds[i].SetVolume();
                return;
            }
        }
    }
}
