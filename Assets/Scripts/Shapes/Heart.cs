using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Shape
{
    const int MAX_HEALTH = 165;
    const float SHAPE_SPEED = 0.7f;

    const int MAX_CHARGE = 3;
    const int BASE_DAMAGE = 45;
    const int DAMAGE_INCREASE = 21;
    const float BULLET_SPEED = 20f;
    const float MIN_BULLET_SIZE = 1.2f;
    const float BULLET_SIZE_INCREASE = 0.15f;
    const float CHARGE_SLOW_AMOUNT = 0.35f;
    const float LINGER_TIME = 0.5f;
    const float FIRE_DELAY = 1.7f;
    const float CHARGE_DELAY = 0.4f;
    const float SPELLSHIELD_COOLDOWN = 12f;

    [SerializeField] GameObject _bulletPrefab = null;

    int damage;
    float bulletSize;

    int charge;
    float chargeTime = 0f;
    float reloadTime = 0f;
    float spellshieldTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;

        damage = BASE_DAMAGE;
        spellshieldTime = SPELLSHIELD_COOLDOWN;

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
                charge = Mathf.FloorToInt(chargeTime / CHARGE_DELAY) + 1;
                SetCharge();
                transform.GetChild(charge).gameObject.SetActive(true);
            }
            if (FireKeyUp())
            {
                SelfSlow(false);
                Fire(BULLET_SPEED, damage, bulletSize, bulletSize, transform.position).GetComponent<BulletHeart>().SetStats(LINGER_TIME);
                chargeTime = 0f;
                reloadTime = 0f;
                charge = 0;
                SetCharge();
                for (int i = 1; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        if (!invulnerable)
        {
            spellshieldTime += Time.deltaTime;
            if (spellshieldTime >= SPELLSHIELD_COOLDOWN)
            {
                invulnerable = true;
                transform.GetChild(0).gameObject.SetActive(true);
                spellshieldTime = 0f;
            }
        }

        _Update();
    }

    void SetCharge()
    {
        damage = BASE_DAMAGE + DAMAGE_INCREASE * charge;
        bulletSize = MIN_BULLET_SIZE + BULLET_SIZE_INCREASE * charge;
    }
    protected override int TakeDamage(int damage)
    {
        if (invulnerable)
        {
            invulnerable = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
        spellshieldTime = 0f;
        return damage;
    }
}
