using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceWriter : MonoBehaviour
{
    private GameObject status;
    private GameObject gradeup;

    private Text textScore;
    private int writeScore;
    private Text textLife;
    private Text textWave;
    private Text textRemain;
    private GameObject objAddscore;
    private Text textAddscore;
    private int timeAddscore;
    private Text[] textsUpgrade = new Text[5];
    private Text[] textsLevel = new Text[5];

    void Start()
    {
        this.status = GameObject.Find("StatusAdmin");
        this.gradeup = GameObject.Find("GradeupAdmin");

        this.textScore = GameObject.Find("text_Score").GetComponent<Text>();
        this.writeScore = 0;
        this.textLife = GameObject.Find("text_Life").GetComponent<Text>();
        this.textWave = GameObject.Find("text_Wave").GetComponent<Text>();
        this.textRemain = GameObject.Find("text_Remain").GetComponent<Text>();
        this.objAddscore = GameObject.Find("text_Addscore");
        this.textAddscore = this.objAddscore.GetComponent<Text>();

        this.textsUpgrade[0] = GameObject.Find("text_Upgrade1").GetComponent<Text>();
        this.textsUpgrade[1] = GameObject.Find("text_Upgrade2").GetComponent<Text>();
        this.textsUpgrade[2] = GameObject.Find("text_Upgrade3").GetComponent<Text>();
        this.textsUpgrade[3] = GameObject.Find("text_Upgrade4").GetComponent<Text>();
        this.textsUpgrade[4] = GameObject.Find("text_Upgrade5").GetComponent<Text>();

        this.textsLevel[0] = GameObject.Find("text_Level1").GetComponent<Text>();
        this.textsLevel[1] = GameObject.Find("text_Level2").GetComponent<Text>();
        this.textsLevel[2] = GameObject.Find("text_Level3").GetComponent<Text>();
        this.textsLevel[3] = GameObject.Find("text_Level4").GetComponent<Text>();
        this.textsLevel[4] = GameObject.Find("text_Level5").GetComponent<Text>();
        SetTextUpgrade(0);
    }

    public void SetMusicName(string music, string artist)
    {
        StartCoroutine(CoroutineMusicName(music, artist));
    }
    private IEnumerator CoroutineMusicName(string music, string artist)
    {
        GameObject textMusic = GameObject.Find("text_Music");
        GameObject textArtist = GameObject.Find("text_Artist");
        textMusic.GetComponent<Text>().text = string.Format("ÅÙ{0,0}", music);
        textArtist.GetComponent<Text>().text = string.Format("/ {0,0}", artist);

        for (int i = 0; i < 90; i++)
        {
            textMusic.transform.Translate(0, 1, 0);
            textArtist.transform.Translate(0, 1, 0);
            yield return null;
        }
        yield return new WaitForSeconds(5);
        for (int i = 0; i < 90; i++)
        {
            textMusic.transform.Translate(0, -1, 0);
            textArtist.transform.Translate(0, -1, 0);
            yield return null;
        }
    }

    public void SetAddscore(int addscore)
    {
        Color c = this.textAddscore.color;
        this.textAddscore.color = new Color(c.r, c.g, c.b, 0);

        this.textAddscore.text = string.Format("+ {0:#,0}", addscore);
        this.textAddscore.GetComponent<RectTransform>().localPosition = new Vector2(490, -260);

        this.timeAddscore = 40;
    }

    public void SetTextUpgrade(int stack)
    {
        for (int i = 0; i < 5; i++)
        {
            float c = 0.5f;
            if (i + 1 == stack)
            {
                c = 1.0f;
            }
            this.textsUpgrade[i].color = new Color(c, c, c);
        }

        this.textsLevel[0].text = string.Format("{0,0}", this.gradeup.GetComponent<GradeupAdministrator>().GetLvScore());
        this.textsLevel[1].text = string.Format("{0,0}", this.gradeup.GetComponent<GradeupAdministrator>().GetLvPower());
        this.textsLevel[3].text = string.Format("{0,0}", this.gradeup.GetComponent<GradeupAdministrator>().GetLvSpeed());
        this.textsLevel[4].text = string.Format("{0,0}", this.gradeup.GetComponent<GradeupAdministrator>().GetLvMultishot());
    }


    void Update()
    {
        int div = this.status.GetComponent<StatusAdministrator>().GetScore() - this.writeScore;
        if (div > 0)
        {
            this.writeScore += div / 20 + 1;
        }

        this.textScore.text = string.Format("{0:#,0}", this.writeScore);
        this.textLife.text = string.Format("{0:0}", this.status.GetComponent<StatusAdministrator>().GetLife());
        this.textWave.text = string.Format("Wave {0:00}", this.status.GetComponent<StatusAdministrator>().GetWave());
        this.textRemain.text = string.Format("{0:0}", this.status.GetComponent<StatusAdministrator>().GetRemain());


        if (this.timeAddscore > 30)
        {
            RectTransform pos = this.textAddscore.GetComponent<RectTransform>();
            pos.localPosition = new Vector2(pos.localPosition.x + 5, -260);
            Color c = this.textAddscore.color;
            float alpha = 1.0f - ((this.timeAddscore - 30) / 10.0f);
            this.textAddscore.color = new Color(c.r, c.g, c.b, alpha);
        }
        else if (this.timeAddscore == 0)
        {
            Color c = this.textAddscore.color;
            this.textAddscore.color = new Color(c.r, c.g, c.b, 0);
        }
        this.timeAddscore--;
    }

}