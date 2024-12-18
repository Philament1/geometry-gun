﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRocket : Bullet
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
            self.GetComponent<Rocket>().BulletHit(true);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Outer Boundary"))
        {
            self.GetComponent<Rocket>().BulletHit(false);
            Destroy(gameObject);
        }
    }

    public void Empower()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
