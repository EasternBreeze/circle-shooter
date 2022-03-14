using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Speaker : MonoBehaviour
{
    public enum SE
    {
        WaveStart, WaveCount, EnemyHit, EnemyCrash, ItemHit, //
        ItemGet, ItemUse, PlayerHit, PlayerCrash,
    }

    [SerializeField] private AudioClip titleBgm;
    [SerializeField] private AudioClip resultBgm;
    [SerializeField] private List<MusicData> bgmList = new List<MusicData>();
    [SerializeField] private List<SeData> seList = new List<SeData>();
    private AudioSource[] aud;

    private bool randomLoop = false;
    private float loopTime = 0;

    void Start()
    {
        this.aud = GetComponents<AudioSource>();
    }

    public void MusicTitle()
    {
        CheckAudio();
        this.randomLoop = false;
        this.aud[0].clip = this.titleBgm;
        this.aud[0].volume = 0.1f;
        this.aud[0].Play();
    }
    public void MusicResult()
    {
        CheckAudio();
        this.randomLoop = false;
        this.aud[0].clip = this.resultBgm;
        this.aud[0].volume = 0.1f;
        this.aud[0].Play();
    }
    public void MusicMain(int m)
    {
        CheckAudio();
        this.randomLoop = true;
        if (m < 0 || m >= this.bgmList.Count)
        {
            m = Random.Range(0, this.bgmList.Count);
        }

        if (this.aud == null)
        {
            this.aud = GetComponents<AudioSource>();
        }
        MusicData data = this.bgmList[m];
        this.aud[0].clip = data.mp3;
        this.aud[0].volume = data.volume;
        this.aud[0].Play();

        GameObject.Find("InterfaceWriter").GetComponent<InterfaceWriter>().SetMusicName(data.music, data.artist);
    }
    public void SePlay(SE name)
    {
        SeData data = seList[(int)name];

        this.aud[1].PlayOneShot(data.se, data.volume);
    }

    public void MusicFadeOut()
    {
        StartCoroutine(CoroutineMusicFadeOut());
    }
    private IEnumerator CoroutineMusicFadeOut()
    {
        float maxVolume = this.aud[0].volume;
        for (int i = 60; i >= 0; i--)
        {
            this.aud[0].volume = maxVolume * i / 60.0f;
            yield return null;
        }
    }

    void Update()
    {
        if (this.randomLoop && this.loopTime >= this.aud[0].clip.length)
        {
            this.loopTime = 0.0f;
            MusicMain(-1);
        }
        this.loopTime += Time.deltaTime;
    }

    private void CheckAudio()
    {
        if (this.aud == null)
        {
            this.aud = GetComponents<AudioSource>();
        }
    }

    [Serializable]
    private class MusicData
    {
        public AudioClip mp3;
        public float volume = 1.0f;
        public string music;
        public string artist;
    }

    [Serializable]
    private class SeData
    {
        public SE name;
        public AudioClip se;
        public float volume = 0.5f;
    }
}