using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectDamagePopup : MonoBehaviour
{
    private int t = 60;

    public void Set(Vector2 vec, int damage)
    {
        RectTransform pos = GetComponent<RectTransform>();
        vec.x += Random.Range(-10, 10);
        vec.y += Random.Range(-5, 5);
        pos.localPosition = vec;
        GetComponent<Text>().text = string.Format("{0:0}", damage);
    }

    void Update()
    {
        transform.Translate(0, 1, 0);

        GetComponent<Text>().color = new Color(255, 255, 255, this.t / 60.0f);

        this.t--;
        if (this.t == 0)
        {
            Destroy(gameObject);
        }
    }
}
