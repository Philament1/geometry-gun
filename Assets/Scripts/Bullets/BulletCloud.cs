using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCloud : Bullet
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
        }
        else if (collision.CompareTag("Outer Boundary"))
        {
            Destroy(gameObject);
        }
        else
        {
            string[] collisionName = collision.name.Split(' ');
            if (collisionName[0] == $"{System.Convert.ToInt32(isP1)}StormCloud")
            {
                self.GetComponent<Cloud>().ChargeStorm();
                Destroy(gameObject);
            }
        }
    }
}
