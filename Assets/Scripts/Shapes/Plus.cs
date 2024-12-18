using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plus : Shape
{
    const int MAX_HEALTH = 180;
    const float BASE_SHAPE_SPEED = 0.74f;

    const int MAX_LEVEL = 7;
    const int DAMAGE_PER_BULLET = 18;
    const int HEAL_AMOUNT = 20;
    const float SHAPE_SPEED_INCREASE = 0.08f;
    const float MAX_LEVEL_ARMOUR = 0.25f;
    const float BASE_BULLET_SPEED = 22f;
    const float BULLET_SPEED_INCREASE = 1f;
    const float BULLET_SIZE = 0.73f;
    const float BASE_FIRE_DELAY = 0.84f;
    const float FIRE_DELAY_DECREASE = 0.1f;

    [SerializeField] GameObject _bulletPrefab = null;

    int level = 0;
    float bulletSpeed, fireDelay, reloadTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = BASE_SHAPE_SPEED;
        maxHealth = MAX_HEALTH;

        fireDelay = BASE_FIRE_DELAY;
        bulletSpeed = BASE_BULLET_SPEED;

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
                Fire(bulletSpeed, DAMAGE_PER_BULLET, BULLET_SIZE, BULLET_SIZE, transform.position);
                reloadTime = 0;
            }
        }

        _Update();
    }

    public void LevelUp()
    {
        if (level < MAX_LEVEL)
        {
            Heal(HEAL_AMOUNT);
            speed += SHAPE_SPEED_INCREASE;
            maxSpeed += SHAPE_SPEED_INCREASE;
            fireDelay -= FIRE_DELAY_DECREASE;
            bulletSpeed += BULLET_SPEED_INCREASE;
            transform.GetChild(level).gameObject.SetActive(true);
            level++;
            if (level == MAX_LEVEL)
            {
                armour = MAX_LEVEL_ARMOUR;
            }
        }
    }
}
