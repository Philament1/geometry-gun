using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTeardrop : Bullet
{
    float maxSpeed, slowAmount, tick, tickTime, sizeGrowth;
    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = speed;
        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 bulletSize = transform.localScale;
        bulletSize[1] += sizeGrowth * Time.deltaTime;
        if (isP1)
        { bulletSize[0] += sizeGrowth * Time.deltaTime; }
        else { bulletSize[0] -= sizeGrowth * Time.deltaTime; }
        transform.localScale = bulletSize;
        _Update();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == targetName)
        {
            if (GameInfo.isOnline)
            {
                SyncCollision();
            }
            speed = maxSpeed * (1 - slowAmount);
            tick -= Time.deltaTime;
            if (tick <= 0)
            {
                self.GetComponent<Teardrop>().Orb(collision);
                tick = tickTime;
            }
        }
        else if (collision.CompareTag("Outer Boundary"))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == targetName)
        {
            speed = maxSpeed;
        }
    }
    public void SetStats(float _tickTime, float _sizeGrowth, float bulletSlowAmount)
    {
        tick = tickTime = _tickTime;
        sizeGrowth = _sizeGrowth;
        slowAmount = bulletSlowAmount;
    }
}
