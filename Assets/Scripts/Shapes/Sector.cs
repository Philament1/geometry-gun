using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector : Shape
{
    const int MAX_HEALTH = 210;
    const float SHAPE_SPEED = 0.75f;

    const int BULLET_DAMAGE_TRACKER = 11;
    const int BULLET_DAMAGE_HOMING = 190;
    const float FIRE_DELAY = 0.95f;
    const float BULLET_SPEED_TRACKER = 34f;
    const float BULLET_SPEED_HOMING = 3f;
    const float BULLET_SPEED_HOMING_VERTPERCENT = 0.75f;
    const float BULLET_SIZE_TRACKER = 0.5f;
    const float BULLET_WIDTH_HOMING = 2.2f;
    const float BULLET_HEIGHT_HOMING = 1.8f;

    [SerializeField] GameObject _bulletPrefab = null;

    Vector2 missileLauncherPosition;
    GameObject missileLauncher;
    float reloadTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;
        missileLauncher = transform.GetChild(1).gameObject;
        missileLauncherPosition = new Vector2(transform.position.x * 2, 0);
        missileLauncher.transform.position = missileLauncherPosition;
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
                Fire(BULLET_SPEED_TRACKER, BULLET_DAMAGE_TRACKER, BULLET_SIZE_TRACKER, BULLET_SIZE_TRACKER, transform.position).GetComponent<BulletSector>().InitHoming(false);
                transform.GetChild(0).gameObject.SetActive(false);
                reloadTime = 0f;
            }
        }
        missileLauncher.transform.position = missileLauncherPosition;
        _Update();
    }

    public void FireHoming()
    {
        Fire(BULLET_SPEED_HOMING, BULLET_DAMAGE_HOMING, BULLET_WIDTH_HOMING, BULLET_HEIGHT_HOMING, missileLauncher.transform.position).GetComponent<BulletSector>().InitHoming(true, BULLET_SPEED_HOMING_VERTPERCENT);
    }
}
