using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSemiCircle : Bullet
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
    
    public void SetPosition(int pos)
    {
        Vector2 tempDir = movementDir;
        switch (pos)                                //0 to 3, top to bot
        {
            case 0:
                tempDir.y = 0.16f;
                break;
            case 1:
                tempDir.y = 0.08f;
                break;
            case 2:
                tempDir.y = -0.08f;
                break;
            case 3:
                tempDir.y = -0.16f;
                break;
        }
        movementDir = tempDir;
    }

}