using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemcontroller : MonoBehaviour
{
    [SerializeField] private int hp = 5;
    [SerializeField] private GameObject particle;

    private GameObject gradeup;
    private GameObject admin;

    private SpriteRenderer sprite;
    private bool alpha = true;
    private int t = 600;
    private bool destroy = true; // 多重キル防止用フラグ

    void Start()
    {
        this.gradeup = GameObject.Find("GradeupAdmin");
        this.admin = GameObject.Find("StatusAdmin");
        this.sprite = this.GetComponent<SpriteRenderer>();
    }

    public void Initialize(float angle, float distance)
    {
        float posX = distance * Mathf.Sin(angle * Mathf.PI / 180.0f);
        float posY = distance * Mathf.Cos(angle * Mathf.PI / 180.0f);
        transform.position = new Vector3(posX, posY, 0);
    }

    public void Hit()
    {
        this.hp--;
        admin.GetComponent<StatusAdministrator>().DamagePopup(transform.position, 1);

        if (this.hp <= 0 && this.destroy)
        {
            GameObject.Find("Speaker").GetComponent<Speaker>().SePlay(Speaker.SE.ItemGet);
            this.destroy = false;
            this.gradeup.GetComponent<GradeupAdministrator>().ItemGet();
            for (int i = 0; i < 5; i++)
            {
                Instantiate(this.particle, this.transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        else
        {
            GameObject.Find("Speaker").GetComponent<Speaker>().SePlay(Speaker.SE.ItemHit);
        }
    }
    void Update()
    {
        this.t--;

        if (this.t <= 0)
        {
            Destroy(gameObject);
        }
        else if (this.t <= 60)
        {
            this.alpha = this.t % 5 == 0 ? !this.alpha : this.alpha;
        }
        else if (this.t <= 180)
        {
            this.alpha = this.t % 10 == 0 ? !this.alpha : this.alpha;
        }
        else if (this.t <= 300)
        {
            this.alpha = this.t % 20 == 0 ? !this.alpha : this.alpha;
        }

        this.sprite.color = this.alpha ? new Color(255, 255, 255, 1) : new Color(255, 255, 255, 0.5f);
    }
}
