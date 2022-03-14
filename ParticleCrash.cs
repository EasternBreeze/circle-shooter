using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCrash : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Vector2 move;
    private int t = 30;

    void Start()
    {
        this.sprite = this.GetComponent<SpriteRenderer>();
        this.move.x = Random.Range(0.1f, 2.0f) * (Random.Range(0, 2) == 0 ? 1 : -1);
        this.move.y = Random.Range(0.1f, 2.0f) * (Random.Range(0, 2) == 0 ? 1 : -1);
    }


    void Update()
    {
        this.t--;

        this.transform.Translate(this.move);
        this.sprite.color = new Color(255, 255, 255, this.t / 30.0f);

        if (this.t <= 0)
        {
            Destroy(gameObject);
        }
    }
}
