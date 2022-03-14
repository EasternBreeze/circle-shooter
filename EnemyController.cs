using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int hp = 10;
    [SerializeField] private int atk = 5;
    [SerializeField] private int score = 10;

    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float movePolar = 0f;
    [SerializeField] private int kbResistance = 0;
    private float[] resistRate = { 1.0f, 0.50f, 0.25f, 0.0f };

    [SerializeField] private GameObject particleCrash;
    [SerializeField] private GameObject particleAttack;

    private GameObject admin;

    private float distance = 350.0f;
    private float angle;
    private bool destroy = true; // 多重キル防止用フラグ
    private bool canMove = true;

    //
    void Start()
    {
        this.admin = GameObject.Find("StatusAdmin");
    }

    public void Initialize(float angle, int hpExtend)
    {
        this.angle = angle;
        this.hp = this.hp * hpExtend / 10;
        Move();
    }
    //

    //
    public void Stop(bool flag)
    {
        this.canMove = flag;
    }

    public void Hit(int damage, int knockback)
    {
        this.hp -= damage;
        admin.GetComponent<StatusAdministrator>().DamagePopup(transform.position, damage);

        this.distance += knockback * this.resistRate[this.kbResistance];
        this.distance = this.distance < 350 ? this.distance : 350;

        if (this.hp <= 0 && this.destroy)
        {
            GameObject.Find("Speaker").GetComponent<Speaker>().SePlay(Speaker.SE.EnemyCrash);
            this.destroy = false;
            Delete(true);
        }
        else
        {
            GameObject.Find("Speaker").GetComponent<Speaker>().SePlay(Speaker.SE.EnemyHit);
        }
    }

    void Update()
    {
        if (this.canMove)
        {
            Move();
        }

        if (this.distance <= 50) // プレイヤー被弾距離
        {
            Delete(false);
        }
    }

    private void Move()
    {
        this.angle += movePolar;
        this.distance -= moveSpeed;

        float posX = this.distance * Mathf.Sin(this.angle * Mathf.PI / 180.0f);
        float posY = this.distance * Mathf.Cos(this.angle * Mathf.PI / 180.0f);
        transform.position = new Vector3(posX, posY, 0);
    }

    private void Attack()
    {
        admin.GetComponent<StatusAdministrator>().Damaged(this.atk);
    }

    private void Delete(bool isKilled)
    {
        if (isKilled)
        {
            for (int i = 0; i < 5; i++)
            {
                Instantiate(this.particleCrash, this.transform.position, Quaternion.identity);
            }
            int score = (int)(this.score * (this.distance / 350.0f + 0.5f));
            admin.GetComponent<StatusAdministrator>().AddScore(score);
        }
        else
        {
            for (int i = 0; i < this.atk; i++)
            {
                Instantiate(this.particleAttack, this.transform.position, Quaternion.identity);
            }
            Attack();
        }

        admin.GetComponent<StatusAdministrator>().EnemyKilled();
        Destroy(gameObject);
    }
}
