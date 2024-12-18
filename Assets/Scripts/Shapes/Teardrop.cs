using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teardrop : Shape
{
    const int MAX_HEALTH = 190;
    const float SHAPE_SPEED = 0.7f;

    const int DAMAGE_PER_TICK = 4;
    const int SHIELD_AMOUNT = 35;
    const float BULLET_SPEED = 15f;
    const float BULLET_SIZE = 1f;
    const float BULLET_SIZE_GROWTH = 1.3f;
    const float BULLET_SLOW_AMOUNT = 0.82f;
    const float FIRE_DELAY = 1.15f;
    const float SLOW_AMOUNT = 0.8f;
    const float SLOW_DURATION = 0.85f;
    const float TICK_TIME = 0.06f;
    const float SHIELD_COOLDOWN = 10f;

    [SerializeField] GameObject _bulletPrefab = null;

    GameObject shieldObject;

    float reloadTime = 0f, shieldTimer = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;
        shieldTimer = SHIELD_COOLDOWN;
        shieldObject = transform.Find("Shield").gameObject;

        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (shielded)
        {
            float shieldVisualSize = 1f + shield * 0.005f;
            shieldObject.transform.localScale = new Vector3(shieldVisualSize, shieldVisualSize, 1f);
        }
        else
        {
            shieldObject.SetActive(false);
        }
        reloadTime += Time.deltaTime;
        if (isControllable && reloadTime >= FIRE_DELAY)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (FireKeyHold())
            {
                Fire(BULLET_SPEED, DAMAGE_PER_TICK, BULLET_SIZE, BULLET_SIZE, transform.position).GetComponent<BulletTeardrop>().SetStats(TICK_TIME, BULLET_SIZE_GROWTH, BULLET_SLOW_AMOUNT);
                transform.GetChild(0).gameObject.SetActive(false);
                reloadTime = 0;
            }
        }
        if (!shielded)
        {
            shieldTimer += Time.deltaTime;
            if (shieldTimer >= SHIELD_COOLDOWN)
            {
                Shield(SHIELD_AMOUNT, false);
                shieldObject.SetActive(true);
                shieldTimer = 0f;
            }
        }

        _Update();
    }
    protected override int TakeDamage(int damage)
    {
        shieldTimer = 0f;
        return damage;
    }

    public void Orb(Collider2D enemy) 
    {
        enemy.GetComponent<Shape>().Slow(SLOW_AMOUNT, SLOW_DURATION);
        enemy.GetComponent<Shape>().Damage(DAMAGE_PER_TICK);
    }
}
