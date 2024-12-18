using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : Shape
{
    const int MAX_HEALTH = 180;
    const float SHAPE_SPEED = 0.9f;

    const int DAMAGE_PER_BULLET = 23;
    const int MAX_CHARGE = 5;
    const float BULLET_SPEED = 14f;
    const float BULLET_SPEED_VERTPERCENT = 0.15f;
    const float BULLET_LINGER = 0.76f;
    const float BULLET_WIDTH = 0.9f;
    const float BULLET_HEIGHT = 1.2f;
    const float CHARGE_DELAY = 0.17f;
    const float FIRE_DELAY = 0.22f;
    const float CHARGE_SLOW_AMOUNT = 0.3f;

    [SerializeField] GameObject _bulletPrefab = null;

    int charge = 0, bulletsRemaining = 0;
    float chargeTime = 0f, fireTime;

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
        if (isControllable && bulletsRemaining == 0)
        {
            if (FireKeyHold() && charge < MAX_CHARGE)
            {
                SelfSlow(true, CHARGE_SLOW_AMOUNT);
                chargeTime += Time.deltaTime;
                charge = Mathf.FloorToInt(chargeTime / CHARGE_DELAY);
                if (charge > 0)
                {
                    transform.GetChild(charge - 1).gameObject.SetActive(true);
                }
            }
            if (FireKeyUp())
            {
                SelfSlow(false);
                if (charge > 0)
                {
                    Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_WIDTH, BULLET_HEIGHT, transform.position).GetComponent<BulletDiamond>().InitDiamond(BULLET_SPEED_VERTPERCENT, BULLET_LINGER);
                    bulletsRemaining = charge;
                }
                fireTime = FIRE_DELAY;
                chargeTime = 0f;
                charge = 0;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        if (bulletsRemaining > 0)
        {
            fireTime -= Time.deltaTime;
            if (fireTime <= 0f)
            {
                Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_WIDTH, BULLET_HEIGHT, transform.position).GetComponent<BulletDiamond>().InitDiamond(BULLET_SPEED_VERTPERCENT, BULLET_LINGER);
                fireTime = FIRE_DELAY;
                bulletsRemaining--;
            }
        }
        _Update();
    }
}
