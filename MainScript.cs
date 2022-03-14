using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    [SerializeField] private GameObject particleAttack;
    private Speaker speaker;

    private int t = 0;
    private bool stop = true;

    void Start()
    {
        this.speaker = GameObject.Find("Speaker").GetComponent<Speaker>();
        this.speaker.MusicMain(-1);
        StartCoroutine(CoroutineGameStart());
    }

    private IEnumerator CoroutineGameStart()
    {
        yield return StartCoroutine(CoroutineFadeIn());
        yield return new WaitForSeconds(1);

        this.stop = false;
    }

    public void Gameover()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().SetShotFlag(-1);
        GameObject.Find("WaveAdmin").GetComponent<WaveAdministrator>().Stop();
        foreach (GameObject b in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Destroy(b);
        }
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            e.GetComponent<EnemyController>().Stop(false);
        }

        GameObject.Find("Speaker").GetComponent<Speaker>().SePlay(Speaker.SE.PlayerCrash);
        StartCoroutine(CoroutineSceneChange(true));
    }

    public void Gameclear()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().SetShotFlag(-1);
        GameObject.Find("WaveAdmin").GetComponent<WaveAdministrator>().Stop();
        foreach (GameObject b in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Destroy(b);
        }
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            e.GetComponent<EnemyController>().Stop(false);
        }

        StatusAdministrator s = GameObject.Find("StatusAdmin").GetComponent<StatusAdministrator>();
        s.AddScore(s.GetLife() * 200);
        StartCoroutine(CoroutineSceneChange(false));
    }

    private IEnumerator CoroutineSceneChange(bool killPlayer)
    {
        this.speaker.MusicFadeOut();

        if (killPlayer)
        {
            for (int i = 0; i < 50; i++)
            {
                Instantiate(this.particleAttack, this.transform.position, Quaternion.identity);
            }
            Destroy(GameObject.Find("_PlayerCore"));
            Destroy(GameObject.Find("Player"));
        }

        yield return new WaitForSeconds(3);

        yield return StartCoroutine(CoroutineFadeOut());

        // ƒŠƒUƒ‹ƒg—p•Ï”‘‚«ž‚Ý
        PlayerPrefs.SetInt("SCORE", GameObject.Find("StatusAdmin").GetComponent<StatusAdministrator>().GetScore());
        PlayerPrefs.SetInt("WAVE", GameObject.Find("StatusAdmin").GetComponent<StatusAdministrator>().GetWave());

        int time = this.t;
        string s = "";
        if (time >= 216000)
        {
            s += (time / 216000) + ":";
            time %= 216000;
        }
        s += string.Format("{0:00}:", time / 3600);
        time %= 3600;
        s += string.Format("{0:00}", time / 60);
        PlayerPrefs.SetString("TIME", s);
        PlayerPrefs.Save();
        //

        SceneManager.LoadScene("ResultScene");
    }

    void Update()
    {
        if (this.stop)
        {
            return;
        }

        if (this.t == 0)
        {
            GameObject.Find("StatusAdmin").GetComponent<StatusAdministrator>().GameStart();
            GameObject.Find("Player").GetComponent<PlayerController>().SetShotFlag(0);
        }
        this.t++;
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
