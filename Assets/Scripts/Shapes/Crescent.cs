using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crescent : Shape
{
    const int MAX_HEALTH = 195;
    const float SHAPE_SPEED_BASE = 0.75f;

    const int DAMAGE_PER_BULLET = 13;
    const float BULLET_SPEED = 23f;
    const float BULLET_WIDTH = 0.6f;
    const float BULLET_HEIGHT = 0.38f;
    const float FIRE_DELAY_BASE = 0.65f;

    const float MAX_FURY = 100f;
    const float FURY_GAIN_RATE = 5f;
    const float FURY_PER_HIT = 9f;

    const int HEAL_FURY = 1;
    const float SHAPE_SPEED_FURY = 1.4f;
    const float FIRE_DELAY_FURY = 0.14f;
    const float ARMOUR_FURY = 0.35f;
    const float FURY_DURATION = 6.2f;

    [SerializeField] GameObject _bulletPrefab = null;

    bool isFury = false;
    float fury = 0f, reloadTime = 0f, fireDelay, furyTime;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED_BASE;
        maxHealth = MAX_HEALTH;

        fireDelay = FIRE_DELAY_BASE;

        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        reloadTime += Time.deltaTime;
        if (isControllable && reloadTime >= fireDelay)
        {
            if (FireKeyHold())
            {
                FireBullet(true);
                FireBullet(false);
                if (isFury)
                {
                    Heal(HEAL_FURY);
                }
                reloadTime = 0;
            }
        }
        if (isFury)
        {
            furyTime -= Time.deltaTime;
            if (furyTime <= 0f)
            {
                DeFury();
                fury = 0f;
                furyTime = 0f;
            }
        }
        else
        {
            fury += FURY_GAIN_RATE * Time.deltaTime;
            Vector2 tempScale;
            if (fury >= MAX_FURY)
            {
                fury = MAX_FURY;
                EnFury();
            }
            tempScale.x = tempScale.y = fury / MAX_FURY;
            transform.GetChild(0).transform.localScale = tempScale;
        }

        _Update();
    }
    void FireBullet(bool isTop)
    {
        Vector2 bulletSpawnPosition = transform.position;
        if (isTop)
        {
            bulletSpawnPosition.y += BULLET_HEIGHT;
        }
        else
        {
            bulletSpawnPosition.y -= BULLET_HEIGHT;

        }
        Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_WIDTH, BULLET_HEIGHT, bulletSpawnPosition);
    }

    void EnFury()
    {
        isFury = true;
        speed = SHAPE_SPEED_FURY;
        fireDelay = FIRE_DELAY_FURY;
        armour = 0f;
        furyTime = FURY_DURATION;
        transform.GetChild(1).gameObject.SetActive(true);
    }
    void DeFury()
    {
        isFury = false;
        speed = SHAPE_SPEED_BASE;
        fireDelay = FIRE_DELAY_BASE;
        armour = ARMOUR_FURY;
        transform.GetChild(1).gameObject.SetActive(false);
    }
    public void AddFury()
    {
        fury += FURY_PER_HIT;
    }
}
