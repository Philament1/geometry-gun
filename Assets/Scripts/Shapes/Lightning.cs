using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Shape
{
    const int MAX_HEALTH = 170;
    const float SHAPE_SPEED = 0.9f;

    const int DAMAGE_PER_BULLET = 45;
    const int HEALTH_DRAIN = 3;
    const int HEALTH_GAIN = 2;
    const float BULLET_SPEED = 38f;
    const float BULLET_WIDTH = 3f;
    const float BULLET_HEIGHT = 0.45f;
    const float FIRE_DELAY = 1.1f;
    const float VULNERABLE_DURATION = 2f;
    const float VULNERABLE_SLOW_AMOUNT = 0.5f;

    [SerializeField] GameObject _bulletPrefab = null;

    float reloadTime = 0f, vulnerableTime = 0f, healthDrainTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;
        invulnerable = true;

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
                SelfSlow(true, VULNERABLE_SLOW_AMOUNT);
                Heal(HEALTH_GAIN);
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                vulnerableTime = VULNERABLE_DURATION;
                invulnerable = false;
                reloadTime = 0;
            }
        }
        if (invulnerable)
        {
            healthDrainTime -= Time.deltaTime;
            if (healthDrainTime <= 0)
            {
                Damage(HEALTH_DRAIN, true);
                healthDrainTime = 1f;
            }
        }
        else
        {
            vulnerableTime -= Time.deltaTime;
            if (vulnerableTime <= 0f)
            {
                invulnerable = true;
                SelfSlow(false);
                transform.GetChild(1).gameObject.SetActive(true);
            }
        }

        _Update();
    }
}
