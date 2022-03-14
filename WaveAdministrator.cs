using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WaveAdministrator : MonoBehaviour
{
    [SerializeField] private List<Wave> list = new List<Wave>();
    [SerializeField] private GameObject item;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject textTemp;

    private int currentWave;
    private int hpExtend;
    private int itemRemain;
    private int t = 0;

    private Wave enemyList;

    public int WaveStart(int wave)
    {
        this.t = -1;
        this.currentWave = wave;

        this.enemyList = this.currentWave <= 25 ? list[this.currentWave] : ExtraWave(this.currentWave);
        int remain = 0;
        foreach (Pattern p in this.enemyList.patterns)
        {
            remain += p.count * p.multiple;
        }
        this.hpExtend = 10 + (this.currentWave - 1) / 5 * 2; // 5wave毎に+20%
        this.itemRemain = (this.currentWave - 1) / 5 + 3;

        // Wave開始カウントダウン
        StartCoroutine(CoroutineCountdown());
        return remain;
    }

    public void Stop() { this.t = -1; }

    void Spawn(GameObject enemy, float angle)
    {
        GameObject e = Instantiate(enemy) as GameObject;
        e.GetComponent<EnemyController>().Initialize(angle, this.hpExtend);
    }

    void Update()
    {
        if (this.currentWave <= 0 || this.t == -1)
        {
            return;
        }

        foreach (Pattern p in this.enemyList.patterns)
        {
            if (this.t >= p.delay && (this.t - p.delay) % p.wait == 0 && this.t < p.delay + p.wait * p.count)
            {
                float angle = Random.Range(0, 360);
                for (int j = 0; j < p.multiple; j++)
                {
                    Spawn(p.enemy, angle + (360f / p.multiple * j));
                }

            }
        }

        if ((this.t + 300) % 600 == 0 && this.itemRemain > 0)
        {
            GameObject obj = Instantiate(item) as GameObject;
            obj.GetComponent<Itemcontroller>().Initialize(Random.Range(0, 360), Random.Range(150, 300));
            this.itemRemain--;
        }
        this.t++;
    }

    private IEnumerator CoroutineCountdown()
    {
        GameObject tWave = Instantiate(this.textTemp) as GameObject;
        tWave.GetComponent<RectTransform>().localPosition = new Vector2(0, 225);
        tWave.GetComponent<Text>().text = string.Format("Wave {0:0}", this.currentWave);
        tWave.transform.SetParent(this.canvas.transform, false);

        GameObject tCount = Instantiate(this.textTemp) as GameObject;
        tCount.GetComponent<RectTransform>().localPosition = new Vector2(0, 125);
        tCount.transform.SetParent(this.canvas.transform, false);

        Speaker s = GameObject.Find("Speaker").GetComponent<Speaker>();
        for (int i = 5; i > 0; i--)
        {
            s.SePlay(Speaker.SE.WaveCount);
            tCount.GetComponent<Text>().text = string.Format("Starting in {0:0}", i);
            yield return new WaitForSeconds(1); // TODO: デバッグ用(あとで１に設定)
        }
        Destroy(tWave);
        Destroy(tCount);
        s.SePlay(Speaker.SE.WaveStart);
        this.t = 0;
    }

    private Wave ExtraWave(int n)
    {
        n -= 25;
        Wave extra = n % 5 == 0 ? this.list[27] : this.list[26];
        if (n % 5 == 0)
        {
            extra.patterns[6].count = n / 5;
            extra.patterns[6].wait = 900 - n / 5 * 60 >= 300 ? 900 - n / 5 * 60 : 300;
            extra.patterns[6].multiple = n / 5 + 2;
        }
        extra.patterns[0].count = n * 2 + 10;
        extra.patterns[0].wait = 60 - n * 4 >= 20 ? 60 - n * 4 : 20;

        extra.patterns[1].count = n * 2 + 5;
        extra.patterns[1].wait = 120 - n * 3 >= 20 ? 120 - n * 3 : 20;

        extra.patterns[2].count = n * 2 + 3;
        extra.patterns[2].wait = 120 - n * 3 >= 20 ? 120 - n * 3 : 20;
        extra.patterns[3].count = n * 2 + 3;
        extra.patterns[3].wait = 120 - n * 3 >= 20 ? 120 - n * 3 : 20;

        extra.patterns[4].count = n * 2 + 3;
        extra.patterns[4].wait = 90 - n * 3 >= 20 ? 90 - n * 3 : 20;
        extra.patterns[5].count = n * 2 + 3;
        extra.patterns[5].wait = 90 - n * 3 >= 20 ? 90 - n * 3 : 20;
        return extra;
    }

    [Serializable]
    private class Wave
    {
        public List<Pattern> patterns;
    }

    [Serializable]
    private class Pattern
    {
        public int delay; // スポーン開始フレーム
        public GameObject enemy; // スポーンさせる敵
        public int count = 1; // スポーン数
        public int wait = 60; // スポーン間隔フレーム
        public int multiple = 1; // 同時出現数 全方位に等間隔配置
    }
}