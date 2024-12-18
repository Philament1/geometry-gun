using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStar5 : Bullet
{
    public enum BombState { travelling, stationary, exploding };
    public BombState explosiveState;
    float travel, explosionTime;
    bool isExplosive, damagedEnemy;
    Color tempColor;
    GameObject explosion;
    
    // Start is called before the first frame update
    void Start()
    {
        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isExplosive)
        {
            if (explosiveState == BombState.travelling)
            {
                travel -= Time.deltaTime;
                if (travel <= 0)
                {
                    speed = 0f;
                    explosiveState = BombState.stationary;
                    tempColor.a = 1f;
                    transform.GetComponent<SpriteRenderer>().color = tempColor;
                }
            }
            else if (explosiveState == BombState.exploding)
            {
                explosion.transform.localScale = transform.GetChild(0).localScale;
            }
        }
        _Update();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isExplosive)
        {
            if (collision.name == targetName)
            {
                if (GameInfo.isOnline)
                {
                    SyncCollision();
                }
                collision.GetComponent<Shape>().Damage(damage);
            }
            else
            {
                string[] collisionName = collision.name.Split(' ');
                if (collisionName[0] == $"{System.Convert.ToInt32(isP1)}Star5")
                {
                    if (collision.GetComponent<BulletStar5>().explosiveState == BombState.stationary)
                    {
                        collision.GetComponent<BulletStar5>().Explode();
                        Destroy(gameObject);
                    }
                }
            }
        }
        if (collision.CompareTag("Outer Boundary"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (isExplosive)
        {
            if (collision.name == targetName)
            {
                if (explosiveState == BombState.stationary)
                {
                    self.GetComponent<Star5>().SlowEnemy(collision);
                }
                if (explosiveState == BombState.exploding && !damagedEnemy)
                {
                    collision.GetComponent<Shape>().Damage(damage);
                    self.GetComponent<Star5>().SlowEnemy(collision);
                    damagedEnemy = true;
                }
            }
            else
            {
                string[] collisionName = collision.name.Split(' ');
                if (collisionName[0] == $"{System.Convert.ToInt32(isP1)}Star5")
                {
                    if (explosiveState == BombState.exploding && collision.GetComponent<BulletStar5>().explosiveState == BombState.stationary)
                    {
                        collision.GetComponent<BulletStar5>().Explode();
                    }
                }
            }
        }
    }

    public void Explode()
    {
        explosiveState = BombState.exploding;
        Destroy(GetComponent<CircleCollider2D>());
        explosion.SetActive(true);
        Destroy(gameObject, explosionTime);
    }


    public void IsTrap(bool _isExplosive, float travelTime = 0f, float lingerTime = 0f, float _explosionTime = 0f)
    {
        isExplosive = _isExplosive;
        if (isExplosive)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/Shapes/Star5");
            tempColor = transform.GetComponent<SpriteRenderer>().color;
            tempColor.a = 0.3f;
            transform.GetComponent<SpriteRenderer>().color = tempColor;
            explosiveState = BombState.travelling;
            travel = travelTime;
            explosionTime = _explosionTime;
            damagedEnemy = false;
            explosion = transform.GetChild(1).gameObject;
            Destroy(gameObject, lingerTime);
        }
        else
        {
            explosiveState = BombState.exploding;
        }
    }
}
