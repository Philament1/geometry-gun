using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapezium : Shape
{
    const int MAX_HEALTH = 200;
    const float SHAPE_SPEED = 0.75f;

    const int DAMAGE_PER_TICK = 6;
    const float MAX_BULLET_HEIGHT = 1.2f;
    const float MIN_BULLET_HEIGHT = 0.5f;
    const float FIRE_DELAY = 0.53f;
    const float MAX_CHARGE_TIME = 0.95f;
    const float CHARGE_SLOW_AMOUNT = 0.3f;
    const float BEAM_SPEED = 55f;
    const float SLOW_AMOUNT = 0.65f;
    const float SLOW_DURATION = 0.5f;
    const float TICK_TIME = 0.06f;
    const float BASE_LINGER_TIME = 0.2f;

    [SerializeField] GameObject _bulletPrefab = null;

    float bulletHeight;

    float chargeTime = 0f;
    float reloadTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;

        bulletHeight = MIN_BULLET_HEIGHT;

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
                Fire(0f, DAMAGE_PER_TICK, 0f, bulletHeight, transform.position).GetComponent<BulletTrapezium>().SetStats(BEAM_SPEED, BASE_LINGER_TIME, TICK_TIME, SLOW_AMOUNT, SLOW_DURATION);
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
        bulletHeight = MIN_BULLET_HEIGHT + (MAX_BULLET_HEIGHT - MIN_BULLET_HEIGHT) * chargeTime / MAX_CHARGE_TIME;
        Vector3 chargeSize = transform.GetChild(0).localScale;
        chargeSize[1] = bulletHeight;
        transform.GetChild(0).localScale = chargeSize;
    }
}
