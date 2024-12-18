using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hourglass : Shape
{
    const int MAX_HEALTH = 190;
    const float SHAPE_SPEED = 0.7f;

    const int BULLET_DAMAGE = 29;
    const int MAX_SHIELD = 90;
    const int MAX_BULLETS_REMAINING = 8;
    const float BULLET_SIZE = 0.8f;
    const float BULLET_SPEED = 20f;
    const float FIRE_DELAY = 0.2f;
    const float CHARGE_SLOW_AMOUNT = 0.3f;
    const float MAX_SHIELD_DURATION = 3.25f;
    const float MIN_CHARGE_SIZE = 1.5f;
    const float MAX_CHARGE_SIZE_INCREASE = 1f;
    const float MIN_CHARGE_TIME = 0.25f;
    const float MAX_CHARGE_TIME = 1.5f;
 

    [SerializeField] GameObject _bulletPrefab = null;

    int bulletsRemaining = 0, shieldAmount = 0;
    float chargeTime = 0f, _shieldDuration = 0f, fireTime = 0f;

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
        if (shielded)
        {
            float shieldVisualSize = MIN_CHARGE_SIZE + MAX_CHARGE_SIZE_INCREASE * shield / MAX_SHIELD;
            transform.GetChild(1).localScale = new Vector3(shieldVisualSize, shieldVisualSize, 1f);
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
        if (isControllable && !shielded)
        {
            if (FireKeyHold())
            {
                SelfSlow(true, CHARGE_SLOW_AMOUNT);
                transform.GetChild(0).gameObject.SetActive(true);
                chargeTime += Time.deltaTime;
                if (chargeTime > MAX_CHARGE_TIME)
                {
                    chargeTime = MAX_CHARGE_TIME;
                }
                float chargeVisualSize = MIN_CHARGE_SIZE + MAX_CHARGE_SIZE_INCREASE * chargeTime / MAX_CHARGE_TIME;
                transform.GetChild(0).localScale = new Vector3(chargeVisualSize, chargeVisualSize, 1f);
            }
            if (FireKeyUp())
            {
                SelfSlow(false);
                if (chargeTime >= MIN_CHARGE_TIME)
                {
                    SetCharge();
                    Fire(BULLET_SPEED, BULLET_DAMAGE, BULLET_SIZE, BULLET_SIZE, transform.position);
                    Shield(shieldAmount, _shieldDuration);
                    transform.GetChild(1).gameObject.SetActive(true);
                }
                transform.GetChild(0).gameObject.SetActive(false);
                chargeTime = 0f;
                fireTime = FIRE_DELAY;
            }
        }
        if (bulletsRemaining > 0)
        {
            fireTime -= Time.deltaTime;
            if (fireTime <= 0f)
            {
                Fire(BULLET_SPEED, BULLET_DAMAGE, BULLET_SIZE, BULLET_SIZE, transform.position);
                fireTime = FIRE_DELAY;
                bulletsRemaining--;
            }
        }

        _Update();
    }

    void SetCharge()
    {
        bulletsRemaining = Mathf.FloorToInt(MAX_BULLETS_REMAINING * chargeTime / MAX_CHARGE_TIME);
        shieldAmount = Mathf.FloorToInt(MAX_SHIELD * chargeTime / MAX_CHARGE_TIME);
        _shieldDuration = MAX_SHIELD_DURATION * chargeTime / MAX_CHARGE_TIME;
    }
}
