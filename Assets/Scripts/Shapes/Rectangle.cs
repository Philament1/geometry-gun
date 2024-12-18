using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle : Shape
{
    const int INIT_HEALTH = 205;
    const int MAX_HEALTH = 250;
    const float SHAPE_SPEED = 0.7f;

    const int DAMAGE_PER_BULLET = 34;
    const int LIFE_STEAL = 12;
    const int HEALTH_DRAIN = 6;
    const float BULLET_SPEED = 20f;
    const float BULLET_WIDTH = 0.4f;
    const float MIN_BULLET_HEIGHT = 1f;
    const float MAX_BULLET_HEIGHT = 2.1f;
    const float RELOAD_DELAY = 1.35f;
    const float FIRE_DELAY = 0.2f;

    [SerializeField] GameObject _bulletPrefab = null;

    float bulletHeight;

    int bulletsRemaining = 0;
    float reloadTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;
        bulletHeight = MIN_BULLET_HEIGHT;

        _Start();

        Damage(MAX_HEALTH - INIT_HEALTH);
    }

    // Update is called once per frame
    void Update()
    {
        reloadTime += Time.deltaTime;
        if (isControllable)
        {
            if (bulletsRemaining > 0 && reloadTime >= FIRE_DELAY)
            {
                transform.GetChild(bulletsRemaining - 1).gameObject.SetActive(false);
                bulletsRemaining--;
                SetRectangleBullet();
                Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_WIDTH, bulletHeight, transform.position);
                reloadTime = 0f;
            }
            else if (reloadTime >= RELOAD_DELAY - FIRE_DELAY * 2)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                if (FireKeyHold())
                {
                    SetRectangleBullet();
                    Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_WIDTH, bulletHeight, transform.position);
                    reloadTime = 0f;
                    bulletsRemaining = 2;
                    transform.GetChild(bulletsRemaining).gameObject.SetActive(false);
                }
            }
        }
        _Update();
    }

    public void SelfHeal()
    {
        Heal(LIFE_STEAL);
    }

    void SetRectangleBullet()
    {
        if (health > 0)
        {
            bulletHeight = MAX_BULLET_HEIGHT - (MAX_BULLET_HEIGHT - MIN_BULLET_HEIGHT) * health / MAX_HEALTH;
            if (health > HEALTH_DRAIN)
            {
                Damage(HEALTH_DRAIN);
            }
            else
            {
                health = 1;
            }
        }
    }
}
