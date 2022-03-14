using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    private Speaker speaker;
    private bool pressKey = true;

    void Start()
    {
        Application.targetFrameRate = 60;
        this.speaker = GameObject.Find("Speaker").GetComponent<Speaker>();
        this.speaker.MusicTitle();
        StartCoroutine(CoroutineFade());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.pressKey)
        {
            this.pressKey = false;
            StartCoroutine(CoroutineSceneChange());
        }
    }

    private IEnumerator CoroutineSceneChange()
    {
        this.speaker.MusicFadeOut();
        yield return StartCoroutine(CoroutineFadeOut());
        SceneManager.LoadScene("GameScene");
    }

    private IEnumerator CoroutineFadeOut()
    {
        yield return null;
        SpriteRenderer fade = GameObject.Find("transition_FadeInOut").GetComponent<SpriteRenderer>();
        for (int i = 1; i <= 60; i++)
        {
            fade.color = new Color(0, 0, 0, i / 60.0f);
            yield return null;
        }
    }
    private IEnumerator CoroutineFade()
    {
        yield return StartCoroutine(CoroutineFadeIn());
    }
    private IEnumerator CoroutineFadeIn()
    {
        yield return null;
        SpriteRenderer fade = GameObject.Find("transition_FadeInOut").GetComponent<SpriteRenderer>();
        for (int i = 60; i >= 0; i--)
        {
            fade.color = new Color(0, 0, 0, i / 60.0f);
            yield return null;
        }
    }
}
