using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusAdministrator : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject textDamage;

    private GameObject main;
    private GameObject writer;
    private GameObject waver;

    private int score;
    private float scorerate;
    private int life = 100;
    [SerializeField] private int wave = 1;
    private int remain;

    void Start()
    {
        this.main = GameObject.Find("MainAdmin");
        this.writer = GameObject.Find("InterfaceWriter");
        this.waver = GameObject.Find("WaveAdmin");
    }

    public void GameStart()
    {
        this.remain = waver.GetComponent<WaveAdministrator>().WaveStart(this.wave);
    }
    public int GetScore() { return this.score; }
    public int GetLife() { return this.life; }
    public int GetWave() { return this.wave; }
    public int GetRemain() { return this.remain; }

    public void SetScorerate(float scorerate) { this.scorerate = scorerate; }

    public void RepairLife(int repair) { this.life += repair; }

    public void AddScore(int add)
    {
        int addscore = (int)(add * this.scorerate);
        this.score += addscore;
        this.writer.GetComponent<InterfaceWriter>().SetAddscore(addscore);
    }

    public void DamagePopup(Vector2 pos, int damage)
    {
        GameObject text = Instantiate(this.textDamage) as GameObject;
        text.transform.SetParent(this.canvas.transform, false);
        text.GetComponent<EffectDamagePopup>().Set(pos, damage);
    }

    public void Damaged(int damage)
    {
        GameObject.Find("Speaker").GetComponent<Speaker>().SePlay(Speaker.SE.PlayerHit);
        this.life -= damage;
        if (this.life <= 0)
        {
            this.life = 0;
            this.main.GetComponent<MainScript>().Gameover();
        }
    }

    public void EnemyKilled()
    {
        this.remain--;
        if (remain == 0)
        {
            WaveClear();
        }
    }

    private void WaveClear()
    {
        if (this.wave == 25)
        {
            // TODO: Ç»ÇÒÇ©ÉNÉäÉAââèoì¸ÇÍÇÈ
            this.main.GetComponent<MainScript>().Gameclear();
        }
        else
        {
            this.wave++;
            this.remain = waver.GetComponent<WaveAdministrator>().WaveStart(this.wave);
        }
    }
}
