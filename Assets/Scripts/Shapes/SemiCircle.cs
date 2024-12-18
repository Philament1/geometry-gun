using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiCircle : Shape
{
    const int MAX_HEALTH = 225;
    const float SHAPE_SPEED = 0.6f;

    const int MAX_CHARGE = 10;
    const int BASE_CHARGE = 6;
    const int DAMAGE_PER_BULLET = 8;
    const int HEAL_AMOUNT = 3;
    const float BULLET_SPEED = 16f;
    const float BULLET_SIZE = 0.24f;
    const float CHARGE_DELAY = 0.29f;
    const float FIRE_DELAY = 0.18f;

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
        if (isControllable)
        {
            if (FireKeyHold() && charge > 0 && chargeTime >= FIRE_DELAY)
            {
                SelfSlow(1f, FIRE_DELAY);
                Heal(HEAL_AMOUNT);
                Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_SIZE, BULLET_SIZE, transform.position).GetComponent<BulletSemiCircle>().SetPosition(0);
                Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_SIZE, BULLET_SIZE, transform.position).GetComponent<BulletSemiCircle>().SetPosition(1);
                Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_SIZE, BULLET_SIZE, transform.position);
                Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_SIZE, BULLET_SIZE, transform.position).GetComponent<BulletSemiCircle>().SetPosition(2);
                Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_SIZE, BULLET_SIZE, transform.position).GetComponent<BulletSemiCircle>().SetPosition(3);
                charge--;
                transform.GetChild(charge).gameObject.SetActive(false);
                chargeTime = 0;
            }
            else
            {
                if (charge < MAX_CHARGE && chargeTime >= CHARGE_DELAY)
                {
                    transform.GetChild(charge).gameObject.SetActive(true);
                    charge++;
                    chargeTime = 0;
                }
            }
        }

        _Update();
    }

}
