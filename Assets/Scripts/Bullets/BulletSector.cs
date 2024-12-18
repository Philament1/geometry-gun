using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSector : Bullet
{
    bool isHoming = false;
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
        if (isHoming)
        {
            float vertical = 0f;
            if (enemy.transform.position.y > transform.position.y)
                vertical = speedVert;
            else if (enemy.transform.position.y < transform.position.y)
                vertical = -speedVert;
            movementDir.y = vertical;
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
            if (!isHoming)
            {
                self.GetComponent<Sector>().FireHoming();
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Outer Boundary"))
        {
            Destroy(gameObject);
        }
    }
    public void InitHoming(bool _isHoming, float _speedVert = 0f)
    {
        isHoming = _isHoming;
        if (isHoming)
        {
            speedVert = _speedVert;
            enemy = GameObject.Find(targetName);
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/Shapes/Sector");
            GetComponent<CircleCollider2D>().radius = 0.4f;
        }
    }
}
