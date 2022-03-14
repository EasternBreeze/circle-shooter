using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject particle;

    private Vector2 moveVector;

    private int power;
    private int knockback;

    public void Inti(Vector2 postion, float angle, float bulletspeed, int power, int knockback)
    {
        transform.position = postion;
        Vector2 vec;
        vec.x = Mathf.Cos(angle * Mathf.Deg2Rad);
        vec.y = Mathf.Sin(angle * Mathf.Deg2Rad);
        vec *= bulletspeed;
        this.moveVector = vec;
        this.power = power;
        this.knockback = knockback;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // param: damage knockback
            collision.GetComponent<EnemyController>().Hit(this.power, this.knockback);
            Crash();
        }
        else if (collision.gameObject.tag == "Item")
        {
            collision.GetComponent<Itemcontroller>().Hit();
            Crash();
        }

    }

    private void Crash()
    {
        for (int i = 0; i < 4; i++)
        {
            Instantiate(this.particle, this.transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    void Update()
    {
        transform.Translate(moveVector);

        float distance = Vector2.Distance(transform.position, new Vector2(0, 0));
        if (distance <= 75)
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, distance / 75);
        }

        if (distance > 360)
        {
            Destroy(gameObject);
        }
    }
}
