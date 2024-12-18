using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star4 : Shape
{
    const int MAX_HEALTH = 180;
    const float SHAPE_SPEED = 1f;

    const int BASE_DAMAGE = 22;
    const int DAMAGE_PER_CHARGE = 20;
    const float BULLET_SPEED = 18f;
    const float BULLET_SIZE = 2f;
    const float BULLET_RETURN_TIMER = 3f;

    [SerializeField] GameObject _bulletPrefab = null;

    bool returned = true;
    int damage, charge;
    float bulletReturn;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;

        damage = BASE_DAMAGE;
        charge = 0;
        bulletReturn = BULLET_RETURN_TIMER;

        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isControllable)
        {
            if (FireKeyHold() && returned)
            {
                Fire(BULLET_SPEED, damage, BULLET_SIZE, BULLET_SIZE, transform.position);
                bulletReturn = BULLET_RETURN_TIMER;
                returned = false;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            if (!returned)
            {
                bulletReturn -= Time.deltaTime;
                if (bulletReturn <= 0f)
                {
                    ReturnBullet(false);
                }
            }
        }
        
        _Update();
    }

    public void ReturnBullet(bool caught)
    {
        returned = true;
        if (caught)
        {
            if (charge < transform.childCount - 1)
            {
                damage += DAMAGE_PER_CHARGE;
                charge++;
            }
        }
        else
        {
            damage = BASE_DAMAGE;
            charge = 0;
        }
        for (int i = 0; i < charge + 1; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void ReturnBullet()
    {
        returned = true;
        for (int i = 0; i < charge + 1; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
