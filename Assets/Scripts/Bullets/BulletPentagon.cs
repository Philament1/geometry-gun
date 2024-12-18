using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPentagon : Bullet
{
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
            if (collision.GetComponent<Shape>().frozen)
            {
                self.GetComponent<Pentagon>().SlowEnemy(collision, false);
            }
            else if (collision.GetComponent<Shape>().slowed)
            {
                self.GetComponent<Pentagon>().SlowEnemy(collision, false);
            }
            else
            {
                self.GetComponent<Pentagon>().SlowEnemy(collision, true);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Outer Boundary"))
        {
            Destroy(gameObject);
        }
    }
}
