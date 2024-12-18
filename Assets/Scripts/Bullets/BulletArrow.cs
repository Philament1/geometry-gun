using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletArrow : Bullet
{
    enum BulletPos { initial, top, bottom};
    BulletPos bulletPos;
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

            if (bulletPos == BulletPos.initial)
            {
                self.GetComponent<Arrow>().Reload();
            }
            collision.GetComponent<Shape>().Damage(damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Outer Boundary"))
        {
            if (bulletPos == BulletPos.initial)
            {
                self.GetComponent<Arrow>().Reload();
            }
            Destroy(gameObject);
        }
    }
    public void SetStats(int _bulletPos)
    {
        bulletPos = (BulletPos)_bulletPos;
        if (bulletPos == BulletPos.top)
        {
            movementDir = Vector2.up;
            transform.Rotate(Vector3.forward * 90f);
        }
        else if (bulletPos == BulletPos.bottom)
        {
            movementDir = Vector2.down;
            transform.Rotate(Vector3.forward * -90f);
        }
    }
    public override void Disintegrate()
    {
        if (bulletPos == BulletPos.initial)
        {
            self.GetComponent<Arrow>().Reload();
        }
        Destroy(gameObject);
    }
}
