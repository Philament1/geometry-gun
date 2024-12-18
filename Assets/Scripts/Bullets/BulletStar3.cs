using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStar3 : Bullet
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

    private void OnTriggerEnter2D(Collider2D collision)
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
    }

    public void SetStats(float linger)
    {
        Destroy(gameObject, linger);
    }
}
