using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHeptagon : Bullet
{
    bool squareSpell;
    // Start is called before the first frame update
    void Start()
    {
        _Start();
    }

    // Update is called once per frame
    void Update()
    {
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

            collision.GetComponent<Shape>().Damage(damage);
            if (squareSpell)
            {
                if (collision.GetComponent<Shape>().frozen)
                {
                    collision.GetComponent<Shape>().Damage(damage);
                }
            }
            else
            {
                self.GetComponent<Heptagon>().CircleSpell(collision);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Outer Boundary"))
        {
            Destroy(gameObject);
        }
    }

    public void SetSpell(bool _squareSpell)         //if square spell is false, set these values for circle spell
    {
        if (_squareSpell)
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/Shapes/Square");
            squareSpell = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/Shapes/Circle");
            squareSpell = false;
        }
    }
}
