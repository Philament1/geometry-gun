using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParallelogram : Bullet
{
    bool isStationary = false, isGrowing = true;
    int tickDamage;
    float tick, tickTime, slowAmount, slowDuration;
    float travel, beamSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 bulletSize = transform.localScale;

        if (isStationary)
        {
            if (isGrowing)
            {
                bulletSize[1] += beamSpeed * Time.deltaTime;
            }
        }
        else
        {
            travel -= Time.deltaTime;
            if (travel <= 0)
            {
                travel = speed = 0f;
                isStationary = true;
            }
        }
        transform.localScale = bulletSize;

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
            if (!isStationary)
            {
                collision.GetComponent<Shape>().Damage(damage);
            }
        }
        else if (collision.CompareTag("Outer Boundary"))
        {
            if (!isStationary)
            {
                Destroy(gameObject);
            }
            else
            {
                isGrowing = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == targetName && isStationary)
        {
            if (GameInfo.isOnline)
            {
                SyncCollision();
            }
            tick -= Time.deltaTime;
            if (tick <= 0)
            {
                collision.GetComponent<Shape>().Damage(tickDamage);
                collision.GetComponent<Shape>().Slow(slowAmount, slowDuration);
                tick = tickTime;
            }
        }
    }

    public void SetStats(float _travelTime, float _beamSpeed, float lingerTime, float _tickTime, float _slowAmount, float _slowDuration, int _tickDamage)
    {
        travel = _travelTime;
        beamSpeed = _beamSpeed;
        tickTime = _tickTime;
        slowAmount = _slowAmount;
        slowDuration = _slowDuration;
        tickDamage = _tickDamage;
        Destroy(gameObject, lingerTime + _travelTime);
    }
}
