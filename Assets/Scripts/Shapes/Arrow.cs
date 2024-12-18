using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Shape
{
    const int MAX_HEALTH = 190;
    const float SHAPE_SPEED = 0.8f;

    const int INIT_BULLET_DAMAGE = 21;
    const int V_BULLET_DAMAGE = 39;
    const float INIT_BULLET_SPEED = 17f;
    const float V_BULLET_SPEED = 9f;
    const float INIT_BULLET_HEIGHT = 0.9f;
    const float INIT_BULLET_WIDTH = 0.9f;
    const float V_BULLET_HEIGHT = 1.3f;
    const float V_BULLET_WIDTH = 1f;

    [SerializeField] GameObject _bulletPrefab = null;

    GameObject bulletObject;
    bool isFiring = false;    
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
        if (isControllable)
        {
            if (!isFiring)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            if (FireKeyDown())
            {
                if (!isFiring)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    bulletObject = Fire(INIT_BULLET_SPEED, INIT_BULLET_DAMAGE, INIT_BULLET_WIDTH, INIT_BULLET_HEIGHT, transform.position);
                    bulletObject.GetComponent<BulletArrow>().SetStats(0);
                    isFiring = true;
                }
                else
                {
                    Split();
                }
            }
        }

        _Update();
    }
    void Split()
    {
        Destroy(bulletObject);
        Fire(V_BULLET_SPEED, V_BULLET_DAMAGE, V_BULLET_WIDTH, V_BULLET_HEIGHT, bulletObject.transform.position).GetComponent<BulletArrow>().SetStats(1);
        Fire(V_BULLET_SPEED, V_BULLET_DAMAGE, V_BULLET_WIDTH, V_BULLET_HEIGHT, bulletObject.transform.position).GetComponent<BulletArrow>().SetStats(2);
        isFiring = false;
    }
    public void Reload()
    {
        isFiring = false;
    }
}
