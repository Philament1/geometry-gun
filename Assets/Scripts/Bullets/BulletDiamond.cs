using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDiamond : Bullet
{
    GameObject enemy;
    float speedVert;
    // Start is called before the first frame update
    void Start()
    {
        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = 0f;
        if (enemy.transform.position.y > transform.position.y)
            vertical = speedVert;
        else if (enemy.transform.position.y < transform.position.y)
            vertical = -speedVert;
        movementDir.y = vertical;
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
    public void InitDiamond(float _speedVert, float linger)
    {
        Destroy(gameObject, linger);
        speedVert = _speedVert;
        enemy = GameObject.Find(targetName);
    }
}
