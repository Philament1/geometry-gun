using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : Shape
{
    const int MAX_HEALTH = 205;
    const float SHAPE_SPEED = 0.7f;
    const float SHAPE_ARMOUR = 0.15f;

    const int MAX_CHARGE = 9;
    const int SHIELD_PER_TURRET = 15;
    const int BULLET_DAMAGE = 11;
    const float BULLET_SPEED = 19f;
    const float BULLET_WIDTH = 0.8f;
    const float BULLET_HEIGHT = 0.6f;
    const float TURRET_WIDTH = 1.2f;
    const float TURRET_HEIGHT = 1.2f;
    const float LINGER_TIME = 8.5f;
    const float FIRE_DELAY = 0.25f;
    const float CHARGE_DELAY = 0.1f;

    [SerializeField] GameObject _bulletPrefab = null;
    [SerializeField] GameObject turretPrefab = null;

    GameObject shieldObject;

    int charge;
    float chargeTime = 0f;
    float reloadTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;
        armour = SHAPE_ARMOUR;
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
                SelfSlow(true, 1f);
                if (charge < MAX_CHARGE)
                {
                    chargeTime += Time.deltaTime;
                    if (chargeTime >= CHARGE_DELAY)
                    {
                        chargeTime = 0f;
                        charge++;
                        transform.GetChild(charge).gameObject.SetActive(true);
                    }
                }
            }
            else if (FireKeyUp())
            {
                SelfSlow(false);
                if (charge < MAX_CHARGE)
                {
                    Fire(BULLET_SPEED, BULLET_DAMAGE, BULLET_WIDTH, BULLET_HEIGHT, transform.position);
                }
                else
                {
                    CreateTurret();
                    Shield(SHIELD_PER_TURRET);
                    shieldObject.SetActive(true);
                }
                reloadTime = 0f;
                chargeTime = 0f;
                charge = 0;
                for (int i = 0; i < MAX_CHARGE; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        _Update();
    }

    void CreateTurret()
    {
        GameObject turret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        turret.transform.localScale = new Vector3(TURRET_WIDTH, TURRET_HEIGHT, 1);
        turret.tag = "Bullet";
        turret.GetComponent<Bullet>().Initialise(isP1, 0f, BULLET_DAMAGE, gameObject);
        turret.GetComponent<TurretHexagon>().SetStats(FIRE_DELAY, LINGER_TIME, BULLET_WIDTH, BULLET_HEIGHT, BULLET_SPEED, _bulletPrefab);
    }
}
