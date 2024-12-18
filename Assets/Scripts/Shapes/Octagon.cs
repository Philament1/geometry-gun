using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octagon : Shape
{
    const int MAX_HEALTH = 200;
    const float SHAPE_SPEED = 0.8f;

    const int MAX_CHARGE = 5;
    const int DAMAGE_PER_CHARGE = 16;
    const float BULLET_SPEED = 12f;
    const float MIN_BULLET_SIZE = 0.7f;
    const float BULLET_SIZE_INCREASE = 0.25f;
    const float FIRE_DELAY = 0.35f;
    const float CHARGE_DELAY = 0.15f;
    const float CHARGE_SLOW_AMOUNT = 0.3f;
    const float SLOW_AMOUNT = 0.25f;
    const float SLOW_DURATION = 4f;
    const float BASE_LINGER_TIME = 2.2f;
    const float TRAVEL_DURATION = 0.8f;

    [SerializeField] GameObject _bulletPrefab = null;

    int damage;
    float bulletSize;

    int charge;
    float chargeTime = 0f;
    float reloadTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;

        damage = DAMAGE_PER_CHARGE;

        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        reloadTime += Time.deltaTime;
        if (isControllable && reloadTime >= FIRE_DELAY)
        {
            if (FireKeyHold() && charge < MAX_CHARGE)
            {
                SelfSlow(true, CHARGE_SLOW_AMOUNT);
                chargeTime += Time.deltaTime;
                charge = (int)(chargeTime / CHARGE_DELAY) + 1;
                SetCharge();
                transform.GetChild(charge - 1).gameObject.SetActive(true);
            }
            if (FireKeyUp())
            {
                SelfSlow(false);
                Fire(BULLET_SPEED, damage, bulletSize, bulletSize, transform.position).GetComponent<BulletOctagon>().SetStats(TRAVEL_DURATION, BASE_LINGER_TIME);
                chargeTime = 0f;
                reloadTime = 0f;
                charge = 0;
                SetCharge();
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        _Update();
    }

    void SetCharge()
    {
        damage = DAMAGE_PER_CHARGE * charge;
        bulletSize = MIN_BULLET_SIZE + BULLET_SIZE_INCREASE * charge;
    }

    public void SlowEnemy(Collider2D enemy)
    {
        enemy.GetComponent<Shape>().Slow(SLOW_AMOUNT, SLOW_DURATION);
    }
}
