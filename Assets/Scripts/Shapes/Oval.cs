using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oval : Shape
{
    const int MAX_HEALTH = 190;
    const float SHAPE_SPEED = 0.9f;

    const int MAX_CHARGE = 18;
    const int BASE_CHARGE = 8;
    const int DAMAGE_PER_BULLET = 14;
    const float BULLET_SPEED = 14f;
    const float BULLET_SIZE = 0.26f;
    const float CHARGE_DELAY = 0.3f;
    const float FIRE_DELAY = 0.15f;
    const float CHARGE_SLOW = 0.4f;

    [SerializeField] GameObject _bulletPrefab = null;

    int charge;
    float chargeTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;

        charge = BASE_CHARGE;

        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        chargeTime += Time.deltaTime;
        if (isControllable && chargeTime >= FIRE_DELAY)
        {
            if (FireKeyHold() && charge > 0 && chargeTime >= FIRE_DELAY)
            {
                SelfSlow(CHARGE_SLOW, FIRE_DELAY);
                Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_SIZE, BULLET_SIZE, transform.position).GetComponent<BulletOval>().SetPosition(true);
                Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_SIZE, BULLET_SIZE, transform.position);
                Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_SIZE, BULLET_SIZE, transform.position).GetComponent<BulletOval>().SetPosition(false);
                charge--;
                transform.GetChild(charge).gameObject.SetActive(false);
                chargeTime = 0;
            }
            else if (charge < MAX_CHARGE && chargeTime >= CHARGE_DELAY)
            {
                transform.GetChild(charge).gameObject.SetActive(true);
                charge++;
                chargeTime = 0;
            }
        }

        _Update();
    }

}
