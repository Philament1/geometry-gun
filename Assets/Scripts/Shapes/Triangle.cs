using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : Shape
{
    const int MAX_HEALTH = 200;
    const float SHAPE_SPEED = 0.8f;

    const int DAMAGE_PER_BULLET = 23;
    const float BULLET_SPEED = 18f;
    const float BULLET_HEIGHT = 0.6f;
    const float BULLET_WIDTH = 0.8f;
    const float CHARGE_DELAY = 0.6f;

    [SerializeField] GameObject _bulletPrefab = null;

    int charge = 0;
    float chargeTime;
    Vector2 bulletSpawnPosition;

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
        chargeTime += Time.deltaTime;
        if (isControllable)
        {
            if (chargeTime / CHARGE_DELAY < 4)
            {
                charge = (int)(chargeTime / CHARGE_DELAY);
                if (charge >= 1)
                {
                    transform.GetChild(charge - 1).gameObject.SetActive(true);
                }
            }
            else
                charge = 3;
            if (FireKeyHold() && charge >= 1)
            {
                FireTriangle(0);
                if (charge >= 2)
                {
                    FireTriangle(1);
                    FireTriangle(2);
                    if (charge >= 3)
                    {
                        FireTriangle(3);
                        FireTriangle(4);
                    }
                }
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                chargeTime = 0;
            }
        }

        _Update();
    }

    void FireTriangle(int position)
    {
        bulletSpawnPosition = transform.position;
        switch (position)
        {
            case 0: //Middle
                break;
            case 1: //Top (2 charges)
                bulletSpawnPosition.y += BULLET_HEIGHT;
                break;
            case 2: //Bottom (2 charges)
                bulletSpawnPosition.y -= BULLET_HEIGHT;
                break;
            case 3: //Top (3 charges)
                bulletSpawnPosition.y += BULLET_HEIGHT * 2;
                break;
            case 4: //Bottom (3 charges)
                bulletSpawnPosition.y -= BULLET_HEIGHT * 2;
                break;
        }
        Fire(BULLET_SPEED, DAMAGE_PER_BULLET, BULLET_WIDTH, BULLET_HEIGHT, bulletSpawnPosition);
    }

}
