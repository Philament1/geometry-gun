using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallelogram : Shape
{
    const int MAX_HEALTH = 200;
    const float SHAPE_SPEED = 0.75f;

    const int BULLET_DAMAGE = 20;
    const int DAMAGE_PER_TICK = 5;
    const float MAX_BULLET_WIDTH = 1.5f;
    const float MIN_BULLET_WIDTH = 0.5f;
    const float BULLET_HEIGHT = 0.5f;
    const float FIRE_DELAY = 0.75f;
    const float MAX_CHARGE_TIME = 0.8f;
    const float CHARGE_SLOW_AMOUNT = 0.3f;
    const float TRAVEL_DURATION = 0.6f;
    const float BULLET_SPEED = 16f;
    const float BEAM_SPEED = 45f;
    const float SLOW_AMOUNT = 0.65f;
    const float SLOW_DURATION = 1f;
    const float TICK_TIME = 0.07f;
    const float LINGER_TIME = 1.75f;

    [SerializeField] GameObject _bulletPrefab = null;

    float bulletWidth;

    float chargeTime = 0f;
    float reloadTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;

        bulletWidth = MIN_BULLET_WIDTH;

        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        reloadTime += Time.deltaTime;
        if (isControllable && reloadTime >= FIRE_DELAY)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            SetCharge();
            if (FireKeyHold())
            {
                SelfSlow(true, CHARGE_SLOW_AMOUNT);
                chargeTime += Time.deltaTime;
                SetCharge();
            }
            if (FireKeyUp())
            {
                SelfSlow(false);
                Fire(BULLET_SPEED, BULLET_DAMAGE, bulletWidth, BULLET_HEIGHT, transform.position).GetComponent<BulletParallelogram>().SetStats(TRAVEL_DURATION, BEAM_SPEED, LINGER_TIME, TICK_TIME, SLOW_AMOUNT, SLOW_DURATION, DAMAGE_PER_TICK);
                chargeTime = 0f;
                reloadTime = 0f;
                transform.GetChild(0).gameObject.SetActive(false);
                SetCharge();
            }
        }

        _Update();
    }

    void SetCharge()
    {
        if (chargeTime > MAX_CHARGE_TIME)
        {
            chargeTime = MAX_CHARGE_TIME;
        }
        bulletWidth = MIN_BULLET_WIDTH + (MAX_BULLET_WIDTH - MIN_BULLET_WIDTH) * chargeTime / MAX_CHARGE_TIME;
        Vector3 chargeSize = transform.GetChild(0).localScale;
        chargeSize[1] = bulletWidth * 0.75f;
        transform.GetChild(0).localScale = chargeSize;
    }
}
