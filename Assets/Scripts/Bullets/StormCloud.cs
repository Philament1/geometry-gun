using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormCloud : Bullet
{
    GameObject enemy;
    int charge = 0, maxCharge, baseDamage, damageIncrease;
    float tick, freezeDuration, homingSpeed, chargeDecayTime, speedIncrease;
    float tickTimer = 0f, decayTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (charge > 0)
        {
            decayTimer += Time.deltaTime;
            if (decayTimer >= chargeDecayTime)
            {
                charge--;
                decayTimer = 0f;
            }
        }
        damage = baseDamage + charge * damageIncrease;


        _Update();
    }

    private void FixedUpdate()
    {
        homingSpeed = (charge + 1) * speedIncrease;
        transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, homingSpeed * Time.deltaTime);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == targetName)
        {
            if (GameInfo.isOnline)
            {
                SyncCollision();
            }

            tickTimer -= Time.deltaTime;
            if (tickTimer <= 0)
            {
                collision.GetComponent<Shape>().Damage(damage);
                collision.GetComponent<Shape>().Slow(1f, freezeDuration);
                tickTimer = tick;
            }
        }
        else if (collision.CompareTag("Outer Boundary"))
        {
            Destroy(gameObject);
        }
    }
    public void InitStorm(float _tick, float _freezeDuration, float _speedIncrease, int _maxCharge, float _chargeDecayTime, int _damageIncrease)
    {
        tick = _tick;
        freezeDuration = _freezeDuration;
        speedIncrease = _speedIncrease;
        maxCharge = _maxCharge;
        chargeDecayTime = _chargeDecayTime;
        baseDamage = damage;
        damageIncrease = _damageIncrease;
        enemy = GameObject.Find(targetName);
    }

    public void _ChargeStorm()
    {
        if (charge < maxCharge)
        {
            charge++;
            decayTimer = 0f;
        }
    }
}
