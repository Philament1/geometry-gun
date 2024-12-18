using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrapezium : Bullet
{
    bool beaming = true;
    float tick, tickTime, slowAmount, slowDuration;
    float beamSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 bulletSize = transform.localScale;
        if (beaming)
        {
            if (isP1)
            { bulletSize[0] += beamSpeed * Time.deltaTime; }
            else { bulletSize[0] -= beamSpeed * Time.deltaTime; }
        }
        transform.localScale = bulletSize;

        _Update();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Outer Boundary"))
        {
            beaming = false;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == targetName)
        {
            if (GameInfo.isOnline)
            {
                SyncCollision();
            }
            tick -= Time.deltaTime;
            if (tick <= 0)
            {
                collision.GetComponent<Shape>().Damage(damage);
                collision.GetComponent<Shape>().Slow(slowAmount, slowDuration);
                tick = tickTime;
            }
        }
    }

    public void SetStats(float _beamSpeed, float lingerTime, float _tickTime, float _slowAmount, float _slowDuration)
    {
        beamSpeed = _beamSpeed;
        tickTime = _tickTime;
        slowAmount = _slowAmount;
        slowDuration = _slowDuration;
        Destroy(gameObject, transform.localScale.y + lingerTime);
    }
    protected override void SetBulletColour(Color bulletColour)
    {
        bulletColour.a = transform.GetChild(0).GetComponent<SpriteRenderer>().color.a;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = bulletColour;
    }
}
