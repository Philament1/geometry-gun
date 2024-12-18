using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletKite : Bullet
{
    bool moving = false;
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
        if (moving)
        {
            if (collision.name == targetName)
            {
                if (GameInfo.isOnline)
                {
                    SyncCollision();
                }

                collision.GetComponent<Shape>().Damage(damage);
                Destroy(gameObject);
            }
            else if (collision.CompareTag("Outer Boundary"))
            {
                Destroy(gameObject);
            }
        }
    }

    public void AddForce(float _bulletSpeed)
    {
        speed = _bulletSpeed;
        moving = true;
    }
}
