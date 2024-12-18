using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star3 : Shape
{
    const int MAX_HEALTH = 225;
    const float SHAPE_SPEED = 0.8f;

    const int BULLET_DAMAGE = 75;
    const float FIRE_DELAY = 1.35f;
    const float BULLET_DELAY = 0.15f;
    const float LINGER_TIME = 0.4f;

    [SerializeField] GameObject _bulletPrefab = null;

    GameObject fireAnimation, targetObject;
    bool firing;
    float reloadTime = 0f, bulletTime = 0f, fireSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;
        targetObject = transform.GetChild(1).gameObject;
        fireAnimation = transform.GetChild(2).gameObject;
        fireSpeed = targetObject.transform.localPosition.x / BULLET_DELAY;

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
                transform.GetChild(0).gameObject.SetActive(false);
                reloadTime = 0f;
                bulletTime = 0f;
                firing = true;
                fireAnimation.transform.localPosition = Vector2.zero;
                fireAnimation.SetActive(true);
            }
        }
        if (firing)
        {
            bulletTime += Time.deltaTime;
            fireAnimation.transform.Translate(Vector2.right * fireSpeed * Time.deltaTime, Space.Self);
            Vector2 tempPos = fireAnimation.transform.localPosition;
            tempPos.y = 0f;
            fireAnimation.transform.localPosition = tempPos;
            if (bulletTime >= BULLET_DELAY)
            {
                Fire(0, BULLET_DAMAGE, targetObject.transform.localScale.x, targetObject.transform.localScale.y, targetObject.transform.position).GetComponent<BulletStar3>().SetStats(LINGER_TIME);
                firing = false;
                bulletTime = 0f;
                fireAnimation.SetActive(false);
            }
        }


        _Update();
    }
}
