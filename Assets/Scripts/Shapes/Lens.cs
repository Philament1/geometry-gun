using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lens : Shape
{
    const int MAX_HEALTH = 195;
    const float SHAPE_SPEED = 0.9f;

    const int BULLET_DAMAGE_F = 17;
    const int BULLET_DAMAGE_B = 35;
    const int MAX_CHARGE = 4;
    const float BULLET_SPEED_F = 40f;
    const float BULLET_SPEED_B = 25f;
    const float BULLET_HEIGHT = 0.8f;
    const float BULLET_WIDTH = 1.2f;
    const float FIRE_DELAY = 0.55f;

    [SerializeField] GameObject _bulletPrefab = null;

    int charge = 0;
    float reloadTime;
    GameObject[] bullets;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;
        bullets = new GameObject[MAX_CHARGE];

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
                if (charge < MAX_CHARGE)
                {
                    bullets[charge] = Fire(BULLET_SPEED_F, BULLET_DAMAGE_F, BULLET_WIDTH, BULLET_HEIGHT, transform.position);
                    bullets[charge].GetComponent<BulletLens>().bulletState = BulletLens.BulletState.forward;
                    charge++;
                    transform.GetChild(charge).gameObject.SetActive(true);
                    reloadTime = 0f;
                }
                else if (charge == MAX_CHARGE)
                {
                    FindAndReverseBullets();
                    reloadTime = 0f;
                    charge = 0;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }

        _Update();
    }

    void FindAndReverseBullets()
    {
        foreach (GameObject bullet in bullets)
        {
            if (bullet != null)
            {
                bullet.GetComponent<BulletLens>().ReverseBullet(BULLET_DAMAGE_B, BULLET_SPEED_B);
            }
        }
    }

}
