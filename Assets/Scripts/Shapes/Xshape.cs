using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xshape : Shape
{
    const int MAX_HEALTH = 200;
    const float SHAPE_SPEED = 0.8f;

    const int MAX_CHARGE = 9;
    const int DAMAGE_PER_BULLET = 20;
    const int DAMAGE_PER_TICK = 6;
    const float BULLET_SPEED = 16f;
    const float BULLET_SIZE = 0.7f;
    const float TRAP_SPEED = 14f;
    const float TRAP_WIDTH = 2.4f;
    const float TRAP_HEIGHT = 2.4f;
    const float LINGER_DURATION = 7f;
    const float TRAVEL_DURATION = 0.7f;
    const float TICK_TIME = 0.3f;
    const float TRAP_DURATION = 3.5f;
    const float SLOW_AMOUNT = 0.95f;
    const float FIRE_DELAY = 0.55f;
    const float CHARGE_DELAY = 0.05f;
    const float CHARGE_SLOW_AMOUNT = 0.3f;

    [SerializeField] GameObject _bulletPrefab = null;

    int charge;
    float chargeTime = 0f;
    float reloadTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;

        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        reloadTime += Time.deltaTime;
        if (isControllable && reloadTime >= FIRE_DELAY)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (FireKeyHold())
            {
                SelfSlow(true, CHARGE_SLOW_AMOUNT);
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
                SelfSlow(false);
                if (charge < MAX_CHARGE)
                {
                    Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_SIZE, BULLET_SIZE, transform.position).GetComponent<BulletXshape>().IsTrap(false); ;
                }
                else
                {
                    Fire(TRAP_SPEED, DAMAGE_PER_TICK, TRAP_WIDTH, TRAP_HEIGHT, transform.position).GetComponent<BulletXshape>().IsTrap(true, TRAVEL_DURATION, LINGER_DURATION, TRAP_DURATION);
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

    public void Trap(Collider2D enemy)
    {
        enemy.GetComponent<Shape>().Slow(SLOW_AMOUNT, TRAP_DURATION);
        enemy.GetComponent<Shape>().Poison(TICK_TIME, DAMAGE_PER_TICK, TRAP_DURATION);
    }
}
