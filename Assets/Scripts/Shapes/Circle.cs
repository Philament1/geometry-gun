using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : Shape
{
    const int MAX_HEALTH = 185;
    const float SHAPE_SPEED = 1f;

    const int MAX_CHARGE = 20;
    const int BASE_CHARGE = 8;
    const int DAMAGE_PER_BULLET = 19;
    const float BULLET_SPEED = 16f;
    const float BULLET_SIZE = 0.5f;
    const float CHARGE_DELAY = 0.18f;
    const float FIRE_DELAY = 0.09f;

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
        if (isControllable && FireKeyHold() && charge > 0 && chargeTime >= FIRE_DELAY)
        {
            Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_SIZE, BULLET_SIZE, transform.position);
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

        _Update();
    }
}
