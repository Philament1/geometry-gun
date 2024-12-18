using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Shape
{
    const int MAX_HEALTH = 205;
    const float M_SHAPE_SPEED = 0.75f;
    const float T_SHAPE_SPEED = 0.25f;
    const float SHAPE_ARMOUR = 0.1f;
    const float SWITCH_DELAY = 2f;

    const int M_BULLET_DAMAGE = 26;
    const float M_BULLET_SPEED = 22f;
    const float M_BULLET_SIZE = 0.55f;
    const float M_FIRE_DELAY = 0.5f;

    const int T_BULLET_DAMAGE = 7;
    const int HEAL_REGEN = 1;
    const float T_BULLET_SPEED = 15f;
    const float T_BULLET_SIZE = 0.23f;
    const float T_FIRE_DELAY = 0.19f;

    [SerializeField] GameObject _bulletPrefab = null;

    bool isMobile = true;
    float fireDelay;
    float reloadTime, switchTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = M_SHAPE_SPEED;
        maxHealth = MAX_HEALTH;
        armour = SHAPE_ARMOUR;
        reloadTime = fireDelay = M_FIRE_DELAY;

        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isControllable)
        {
            switchTime += Time.deltaTime;
            if (FireKeyDown() && switchTime >= SWITCH_DELAY)
            {
                SwitchMode();
                switchTime = 0f;
            }
        }
        
        reloadTime -= Time.deltaTime;
        if (reloadTime <= 0f)
        {
            if (isMobile)
            {
                Fire(M_BULLET_SPEED, M_BULLET_DAMAGE, M_BULLET_SIZE, M_BULLET_SIZE, transform.position);
            }
            else
            {
                Fire(T_BULLET_SPEED, T_BULLET_DAMAGE, T_BULLET_SIZE, T_BULLET_SIZE, transform.position);
                TowerFire(true);
                TowerFire(false);
                Heal(HEAL_REGEN);
            }
            reloadTime = fireDelay;
        }
        _Update();
    }
    void TowerFire(bool isTop)
    {
        Vector2 bulletSpawnPosition = transform.position;
        if (isTop)
        {
            bulletSpawnPosition.y += T_BULLET_SIZE * 2;
        }
        else
        {
            bulletSpawnPosition.y -= T_BULLET_SIZE * 2;

        }
        Fire(T_BULLET_SPEED, T_BULLET_DAMAGE, T_BULLET_SIZE, T_BULLET_SIZE, bulletSpawnPosition);
    }

    void SwitchMode()
    {
        if (isMobile)
        {
            isMobile = false;
            transform.GetChild(0).gameObject.SetActive(true);
            fireDelay = T_FIRE_DELAY;
            maxSpeed = T_SHAPE_SPEED;
        }
        else
        {
            isMobile = true;
            transform.GetChild(0).gameObject.SetActive(false);
            fireDelay = M_FIRE_DELAY;
            maxSpeed = M_SHAPE_SPEED;
        }
    }
}
