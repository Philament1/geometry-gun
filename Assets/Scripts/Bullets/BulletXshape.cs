using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletXshape : Bullet
{
    float travel, linger, trapDuration;
    bool isTrap, activated = false, triggered = false;
    Color tempColor;
    
    // Start is called before the first frame update
    void Start()
    {
        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTrap)
        {
            travel -= Time.deltaTime;
            if (travel <= 0)
            {
                speed = 0f;
                activated = true;
                tempColor.a = 1f;
                transform.GetComponent<SpriteRenderer>().color = tempColor;
            }

            linger -= Time.deltaTime;
            if (linger <= 0f)
            {
                Destroy(gameObject);
            }
        }
        _Update();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == targetName)
        {
            if (GameInfo.isOnline)
            {
                SyncCollision();
            }
            if (!isTrap)
            {
                if (collision.GetComponent<Shape>().poisoned)
                {
                    collision.GetComponent<Shape>().Damage(damage * 4);
                }
                else
                {
                    collision.GetComponent<Shape>().Damage(damage);
                }
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Outer Boundary"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == targetName && isTrap && activated && !triggered && !collision.GetComponent<Shape>().poisoned)
        {
            self.GetComponent<Xshape>().Trap(collision);
            linger = trapDuration;
            transform.GetChild(0).transform.gameObject.SetActive(true);
            triggered = true;
        }
    }


    public void IsTrap(bool _isTrap, float travelTime = 0f, float lingerTime = 0f, float _trapDuration = 0f)
    {
        isTrap = _isTrap;
        if (isTrap)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/Shapes/Xshape");
            tempColor = transform.GetComponent<SpriteRenderer>().color;
            tempColor.a = 0.3f;
            transform.GetComponent<SpriteRenderer>().color = tempColor;
            travel = travelTime;
            linger = lingerTime;
            trapDuration = _trapDuration;
        }
    }
}
