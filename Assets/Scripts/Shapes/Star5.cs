using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star5 : Shape
{
    const int MAX_HEALTH = 205;
    const float SHAPE_SPEED = 0.75f;

    const int MAX_CHARGE = 5;
    const int DAMAGE_PER_BULLET = 18;
    const int BOMB_DAMAGE = 72;
    const float BULLET_SPEED = 30f;
    const float BULLET_SIZE = 0.7f;
    const float BOMB_SPEED = 25f;
    const float BOMB_SIZE = 1.45f;
    const float LINGER_DURATION = 4.5f;
    const float TRAVEL_DURATION = 0.36f;
    const float EXPLOSION_DURATION = 0.35f;
    const float SLOW_AMOUNT = 0.85f;
    const float SLOW_DURATION = 1.2f;
    const float FIRE_DELAY_BASE = 1f;
    const float FIRE_DELAY_TRIGGER = 0.15f;
    const float CHARGE_DELAY = 0.05f;

    [SerializeField] GameObject _bulletPrefab = null;

    int charge;
    float chargeTime = 0f;
    float reloadTime = 0f, fireDelay;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;

        fireDelay = FIRE_DELAY_BASE;

        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        reloadTime += Time.deltaTime;
        if (isControllable && reloadTime >= fireDelay)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (FireKeyHold())
            {
                if (charge < MAX_CHARGE)
                {
                    chargeTime += Time.deltaTime;
                    if (chargeTime >= CHARGE_DELAY)
                    {
                        chargeTime = 0f;
                        charge++;
                        transform.GetChild(charge).gameObject.SetActive(true);
                    }
                }
            }
            else if (FireKeyUp())
            {
                if (charge < MAX_CHARGE)
                {
                    Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_SIZE, BULLET_SIZE, transform.position).GetComponent<BulletStar5>().IsTrap(false);
                    fireDelay = FIRE_DELAY_BASE;
                }
                else
                {
                    Fire(BOMB_SPEED, BOMB_DAMAGE, BOMB_SIZE, BOMB_SIZE, transform.position).GetComponent<BulletStar5>().IsTrap(true, TRAVEL_DURATION, LINGER_DURATION, EXPLOSION_DURATION);
                    fireDelay = FIRE_DELAY_TRIGGER;
                }
                reloadTime = 0f;
                chargeTime = 0f;
                charge = 0;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        _Update();
    }
    public void SlowEnemy(Collider2D enemy)
    {
        enemy.GetComponent<Shape>().Slow(SLOW_AMOUNT, SLOW_DURATION);
    }
}
