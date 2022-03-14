using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultScript : MonoBehaviour
{
    private Speaker speaker;
    private bool pressKey = true;
    void Start()
    {
        GameObject.Find("text_Score").GetComponent<Text>().text = string.Format("{0:#,0}", PlayerPrefs.GetInt("SCORE"));
        GameObject.Find("text_Reachwave").GetComponent<Text>().text = string.Format("{0:00}", PlayerPrefs.GetInt("WAVE"));
        GameObject.Find("text_Totaltime").GetComponent<Text>().text = string.Format("{0:0}", PlayerPrefs.GetString("TIME"));

        this.speaker = GameObject.Find("Speaker").GetComponent<Speaker>();
        this.speaker.MusicResult();
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
        SceneManager.LoadScene("TitleScene");
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
