using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Shape
{
    const int MAX_HEALTH = 200;
    const float SHAPE_SPEED = 0.8f;

    const int BASE_BULLET_DAMAGE = 34;
    const int EMPOWERED_BULLET_DAMAGE = 85;
    const int MAX_CHARGE = 2;
    const int EMPOWERED_HEAL = 30;
    const float BULLET_SPEED = 33f;
    const float BULLET_WIDTH = 1.1f;
    const float BULLET_HEIGHT = 0.7f;
    const float FIRE_DELAY = 0.85f;
    const float SPEED_BOOST_AMOUNT = 0.3f;
    const float SPEED_BOOST_DURATION = 1f;

    [SerializeField] GameObject _bulletPrefab = null;

    int charge;
    float reloadTime;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;
        charge = 0;

        RotateSprite90();

        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        reloadTime += Time.deltaTime;
        if (isControllable && reloadTime >= FIRE_DELAY)
        {
            for (int i = 0; i < charge + 1; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            if (FireKeyHold())
            {
                if (charge < MAX_CHARGE)
                {
                    Fire(BULLET_SPEED, BASE_BULLET_DAMAGE, BULLET_WIDTH, BULLET_HEIGHT, transform.position);
                }
                else
                {
                    Fire(BULLET_SPEED, EMPOWERED_BULLET_DAMAGE, BULLET_WIDTH, BULLET_HEIGHT, transform.position).GetComponent<BulletRocket>().Empower();
                }

                for (int i = 0; i < charge + 1; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                reloadTime = 0f;
            }
        }

        _Update();
    }

    public void BulletHit(bool hit)
    {
        if (hit)
        {
            SpeedBoost(SPEED_BOOST_AMOUNT, SPEED_BOOST_DURATION);
            if (charge == MAX_CHARGE)
            {
                Heal(EMPOWERED_HEAL);
            }
            else
            {
                charge++;
            }
        }
        else
        {
            charge = 0;
        }
    }
}
