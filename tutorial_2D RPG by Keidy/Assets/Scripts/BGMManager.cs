using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    public AudioClip[] clips;
    private AudioSource source;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private void Awake()
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
        source = GetComponent<AudioSource>();
    }

    public void Play(int trackNo)
    {
        source.clip = clips[trackNo];
        FadeIn();
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void SetVolume(float _volume)
    {
        source.volume = _volume;
    }
    public void Pause()
    {
        source.Pause();
    }

    public void UnPause()
    {
        source.UnPause();
    }
    void FadeIn()
    {
        StopAllCoroutines();    // fadeIn, fadeOut 같이 실행되면 꼬이므로
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        for (float i = 0; i <= 1; i += 0.01f)
        {
            source.volume += i;
            yield return waitTime;   // new 생성자가 for문안에서 돌면 성능에 문제가 생기므로 new WaitForSeconds를 밖에 선언
        }
    }

    void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeOutCoroutine()
    {
        for (float i = 1.0f; i <= 0; i -= 0.01f)
        {
            source.volume -= i;
            yield return waitTime;
        }
    }
}
