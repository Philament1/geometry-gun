using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLens : Bullet
{
    public enum BulletState { forward, stationary, backward };
    public BulletState bulletState;
    string oppositeBoundary, selfBoundary;
    // Start is called before the first frame update
    void Start()
    {
        _Start();

        if (isP1)
        {
            oppositeBoundary = "Boundry Right";
            selfBoundary = "Boundry Left";
        }
        else
        {
            oppositeBoundary = "Boundry Left";
            selfBoundary = "Boundry Right";
        }
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
            if (bulletState == BulletState.forward)
            {
                collision.GetComponent<Shape>().Damage(damage);

            }
            else if (bulletState == BulletState.backward)
            {
                collision.GetComponent<Shape>().Damage(damage);
                Destroy(gameObject);
            }
        }
        else if (collision.name == oppositeBoundary && bulletState == BulletState.forward)
        {
            speed = 0f;
            bulletState = BulletState.stationary;
        }
        else if (collision.name == selfBoundary && bulletState == BulletState.backward)
        {
            Destroy(gameObject);
        }
            
    }

    public void ReverseBullet(int bulletDamage, float bulletSpeed)
    {
        bulletState = BulletState.backward;
        damage = bulletDamage;
        speed = bulletSpeed * -1;
    }
}
