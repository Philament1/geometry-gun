using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kite : Shape
{
    const int MAX_HEALTH = 195;
    const float SHAPE_SPEED = 0.9f;

    const int DAMAGE_PER_BULLET = 36;
    const float BULLET_SPEED = 30f;
    const float BULLET_HEIGHT = 1.2f;
    const float BULLET_WIDTH = 1.4f;
    const float FIRE_DELAY = 0.5f;

    [SerializeField] GameObject _bulletPrefab = null;

    int charge = 0;
    float reloadTime;
    GameObject firstBullet, secondBullet;

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
            if (charge == 0)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            if (FireKeyHold())
            {
                if (charge == 0)
                {
                    firstBullet = Fire(0, DAMAGE_PER_BULLET, BULLET_WIDTH, BULLET_HEIGHT, transform.position);
                    charge++;
                    transform.GetChild(charge).gameObject.SetActive(true);
                }
                else if (charge == 1)
                {
                    secondBullet = Fire(0, DAMAGE_PER_BULLET, BULLET_WIDTH, BULLET_HEIGHT, transform.position);
                    charge++;
                    transform.GetChild(charge).gameObject.SetActive(true);
                }
                else if (charge == 2)
                {
                    Fire(0, DAMAGE_PER_BULLET, BULLET_WIDTH, BULLET_HEIGHT, transform.position).GetComponent<BulletKite>().AddForce(BULLET_SPEED);
                    FindAndFireBullets();
                    charge = 0;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                reloadTime = 0f;
            }
        }

        _Update();
    }

    void FindAndFireBullets()
    {
        if (firstBullet != null)
        {
            firstBullet.GetComponent<BulletKite>().AddForce(BULLET_SPEED);
        }
        if (secondBullet != null)
        {
            secondBullet.GetComponent<BulletKite>().AddForce(BULLET_SPEED);
        }
    }

}
