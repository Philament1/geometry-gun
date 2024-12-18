using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentagon : Shape
{
    const int MAX_HEALTH = 200;
    const float SHAPE_SPEED = 0.8f;
    const float SHAPE_ARMOUR = 0.12f;

    const int DAMAGE_PER_BULLET = 37;
    const int SHIELD_AMOUNT = 20;
    const float BULLET_SPEED = 22f;
    const float BULLET_HEIGHT = 0.95f;
    const float BULLET_WIDTH = 1.1f;
    const float FIRE_DELAY = 0.85f;
    const float SLOW_AMOUNT = 0.5f;
    const float SLOW_DURATION = 2.1f;

    [SerializeField] GameObject _bulletPrefab = null;

    float reloadTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;
        armour = SHAPE_ARMOUR;

        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        reloadTime += Time.deltaTime;
        if (isControllable && reloadTime >= FIRE_DELAY)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (FireKeyHold())
            {
                Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_WIDTH, BULLET_HEIGHT, transform.position);
                transform.GetChild(0).gameObject.SetActive(false);
                reloadTime = 0;
            }
        }

        _Update();
    }

    public void SlowEnemy(Collider2D enemy, bool slow) //if slow is false, freeze instead
    {
        if (slow)
        {
            enemy.GetComponent<Shape>().Slow(SLOW_AMOUNT, SLOW_DURATION);
        }
        else
        {
            Shield(SHIELD_AMOUNT, true);
            enemy.GetComponent<Shape>().Slow(1f, SLOW_DURATION);
        }
    }
}
