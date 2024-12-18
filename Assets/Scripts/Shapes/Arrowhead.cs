using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrowhead : Shape
{
    const int MAX_HEALTH = 200;
    const float SHAPE_SPEED = 0.8f;

    const int DAMAGE_PER_TICK = 3;
    const int BONUS_DAMAGE = 40;
    const float BULLET_SPEED = 25f;
    const float BULLET_HEIGHT = 0.8f;
    const float BULLET_WIDTH = 0.9f;
    const float FIRE_DELAY = 0.9f;
    const float SLOW_AMOUNT = 0.45f;
    const float POISON_DURATION = 2.05f;
    const float TICK_TIME = 0.11f;

    [SerializeField] GameObject _bulletPrefab = null;

    float reloadTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;

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
                Fire(BULLET_SPEED, BONUS_DAMAGE, BULLET_WIDTH, BULLET_HEIGHT, transform.position);
                transform.GetChild(0).gameObject.SetActive(false);
                reloadTime = 0;
            }
        }

        _Update();
    }

    public void Poison(Collider2D enemy) 
    {
        enemy.GetComponent<Shape>().Slow(SLOW_AMOUNT, POISON_DURATION);
        enemy.GetComponent<Shape>().Poison(TICK_TIME, DAMAGE_PER_TICK, POISON_DURATION);
    }
}
