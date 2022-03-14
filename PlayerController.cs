using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    private int repeatwait;
    private float bulletspeed;
    private int power;
    private int multishot;

    private bool autoshot = true;
    private int shotCooltime = -1;

    public void SetShotFlag(int cooltime) { this.shotCooltime = cooltime; }
    public void SetRepeatwait(int repeatwait) { this.repeatwait = repeatwait; }
    public void SetBulletspeed(float bulletspeed) { this.bulletspeed = bulletspeed; }
    public void SetPower(int power) { this.power = power; }
    public void SetMultishot(int multi) { this.multishot = multi; }

    void Update()
    {
        // マウスの方向に向く
        Vector2 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        vec = vec.normalized;
        float angle = Vector2.Angle(new Vector2(0, 1), vec);
        if (vec.x > 0)
        {
            angle *= -1;
        }
        angle += 90;
        this.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);
        //

        // 右クリックによりショット切り替え
        if (Input.GetMouseButtonDown(1))
        {
            this.autoshot = !this.autoshot;
        }
        //

        // ショット
        if (this.autoshot)
        {
            if (this.shotCooltime == 0)
            {
                Shot(this.multishot, angle);
            }
            this.shotCooltime -= this.shotCooltime > 0 ? 1 : 0;
        }
        //
    }

    void Shot(int multi, float angle)
    {
        Bullet[] set = new Bullet[multi];
        switch (multi)
        {
            case 1:
                set[0] = new Bullet(angle, this.power, 0);
                break;
            case 2:
                set[0] = new Bullet(angle - 7.5f, this.power, 1);
                set[1] = new Bullet(angle + 7.5f, this.power, 1);
                break;
            case 3:
                set[0] = new Bullet(angle - 15.0f, this.power, 2);
                set[1] = new Bullet(angle, this.power, 1);
                set[2] = new Bullet(angle + 15.0f, this.power, 2);
                break;
            case 4:
                set[0] = new Bullet(angle - 22.5f, this.power, 2);
                set[1] = new Bullet(angle - 7.5f, this.power, 1);
                set[2] = new Bullet(angle + 7.5f, this.power, 1);
                set[3] = new Bullet(angle + 22.5f, this.power, 2);
                break;
            case 5:
                set[0] = new Bullet(angle - 30.0f, this.power, 3);
                set[1] = new Bullet(angle - 15.0f, this.power, 2);
                set[2] = new Bullet(angle, this.power, 1);
                set[3] = new Bullet(angle + 15.0f, this.power, 2);
                set[4] = new Bullet(angle + 30.0f, this.power, 3);
                break;
        }
        foreach (Bullet b in set)
        {
            GameObject shot = Instantiate(bullet) as GameObject;

            shot.GetComponent<BulletController>().Inti(transform.position, b.angle, this.bulletspeed, b.power, b.kb);
        }

        this.shotCooltime = this.repeatwait;
    }

    private class Bullet
    {
        static int[] mul = { 1, 3, 2, 1 };
        static int[] div = { 1, 4, 3, 2 };
        public float angle;
        public int power;
        public int kb;
        public Bullet(float angle, int power, int damp)
        {
            this.angle = angle;
            this.power = power * mul[damp] / div[damp];
            this.kb = 5 * mul[damp] / div[damp];
        }
    }
}
