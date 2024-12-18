using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOctagon : Bullet
{
    float travel;
    bool stationary = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        travel -= Time.deltaTime;
        if (travel <= 0)
        {
            speed = 0f;
            stationary = true;
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

            collision.GetComponent<Shape>().Damage(damage);
            if (stationary)
            {
                self.GetComponent<Octagon>().SlowEnemy(collision);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Outer Boundary"))
        {
            Destroy(gameObject);
        }
    }

    public void SetStats(float travelDuration, float lingerDuration)
    {
        travel = travelDuration;
        Destroy(gameObject, transform.localScale.y + lingerDuration);
    }
}
