using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chevron : Shape
{
    const int MAX_HEALTH = 165;
    const float SHAPE_SPEED = 0.95f;

    const int BASE_DAMAGE = 25;
    const int MAX_CHARGE = 5;
    const float BULLET_HEIGHT = 1.1f;
    const float BULLET_WIDTH = 0.8f;
    const float BASE_BULLET_SPEED = 19f;
    const float BULLET_SPEED_MULTIPLIER = 1.25f;
    const float DAMAGE_MULTIPLIER = 0.4f;
    const float FIRE_DELAY = 0.6f;
    const float CHARGE_DELAY = 0.18f;

    [SerializeField] GameObject _bulletPrefab = null;

    float bulletSpeed;

    int charge = 0;
    float chargeTime = 0f;
    float reloadTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;
        bulletSpeed = BASE_BULLET_SPEED;

        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        reloadTime += Time.deltaTime;
        if (isControllable && reloadTime >= FIRE_DELAY)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (FireKeyHold() && charge < MAX_CHARGE)
            {
                chargeTime += Time.deltaTime;
                charge = (int)(chargeTime / CHARGE_DELAY);
                transform.GetChild(charge).gameObject.SetActive(true);            }
            if (FireKeyUp())
            {
                bulletSpeed = BASE_BULLET_SPEED * Mathf.Pow(BULLET_SPEED_MULTIPLIER, charge);
                Fire(bulletSpeed, BASE_DAMAGE, BULLET_WIDTH, BULLET_HEIGHT, transform.position);
                chargeTime = 0f;
                reloadTime = 0f;
                charge = 0;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        _Update();
    }

    public int CalculateDamage(Collider2D enemy)
    {
        int damage = BASE_DAMAGE + Mathf.FloorToInt(enemy.GetComponent<Shape>().health * DAMAGE_MULTIPLIER);
        return damage;
    }
}
