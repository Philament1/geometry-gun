using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOval : Bullet
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
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Outer Boundary"))
        {
            Destroy(gameObject);
        }
    }
    
    public void SetPosition(bool top)
    {
        Vector2 tempDir = movementDir;
        if (top)
        {
            tempDir.y = 0.05f;
        }
        else
        {
            tempDir.y = -0.05f;
        }
        movementDir = tempDir;
    }

}