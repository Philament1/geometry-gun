using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStar4 : Bullet
{
    bool forward = true;
    string selfName, oppositeBoundary, selfBoundary;
    // Start is called before the first frame update
    void Start()
    {
        _Start();

        if (isP1)
        {
            selfName = "Player 1";
            oppositeBoundary = "Boundry Right";
            selfBoundary = "Boundry Left";
        }
        else
        {
            selfName = "Player 2";
            oppositeBoundary = "Boundry Left";
            selfBoundary = "Boundry Right";
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * 360));
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
        else if (collision.name == oppositeBoundary)
        {
            speed *= -1;
            forward = false;
        }
        else if (collision.name == selfBoundary && !forward)
        {
            self.GetComponent<Star4>().ReturnBullet(false);
            Destroy(gameObject);
        }
        else if (collision.name == selfName && !forward)
        {
            self.GetComponent<Star4>().ReturnBullet(true);
            Destroy(gameObject);
        }                
    }
    public override void Disintegrate()
    {
        self.GetComponent<Star4>().ReturnBullet();
        Destroy(gameObject);
    }
}
