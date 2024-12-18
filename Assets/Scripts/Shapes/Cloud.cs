using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : Shape
{
    const int MAX_HEALTH = 220;
    const float SHAPE_SPEED = 0.7f;

    const int BULLET_DAMAGE = 16;
    const int STORM_DAMAGE_BASE = 14;
    const int STORM_DAMAGE_INCREASE = 1;
    const int MAX_STORM_CHARGE = 5;
    const float FIRE_DELAY = 0.65f;
    const float BULLET_SPEED = 25f;
    const float BULLET_SIZE = 0.85f;
    const float STORM_SPEED_INCREASE = 0.43f;
    const float STORM_SIZE = 3.7f;
    const float TICK_TIME = 0.38f;
    const float FREEZE_DURATION = 0.26f;
    const float CHARGE_DECAY_TIME = 1.45f;

    [SerializeField] GameObject _bulletPrefab = null;
    [SerializeField] GameObject stormPrefab = null;

    GameObject stormObject;

    float reloadTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;

        SpawnCloud();
        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        reloadTime += Time.deltaTime;
        if (isControllable && reloadTime >= FIRE_DELAY)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (FireKeyDown())
            {
                Fire(BULLET_SPEED, BULLET_DAMAGE, BULLET_SIZE, BULLET_SIZE, transform.position);
                transform.GetChild(0).gameObject.SetActive(false);
                reloadTime = 0f;
            }
        }
        _Update();
    }
    void SpawnCloud()
    {
        stormObject = Instantiate(stormPrefab, new Vector2(transform.position.x * -2, 0f), Quaternion.identity);
        stormObject.transform.localScale = new Vector3(STORM_SIZE, STORM_SIZE, 1);
        stormObject.name = $"{System.Convert.ToInt32(isP1)}StormCloud";
        stormObject.tag = "Indestructible";
        stormObject.GetComponent<Bullet>().Initialise(isP1, 0f, STORM_DAMAGE_BASE, gameObject);
        stormObject.GetComponent<StormCloud>().InitStorm(TICK_TIME, FREEZE_DURATION, STORM_SPEED_INCREASE, MAX_STORM_CHARGE, CHARGE_DECAY_TIME, STORM_DAMAGE_INCREASE);
    }
    public void ChargeStorm()
    {
        stormObject.GetComponent<StormCloud>()._ChargeStorm();
    }
}
