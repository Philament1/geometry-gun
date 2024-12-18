using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heptagon : Shape
{
    const int MAX_HEALTH = 185;
    const float SHAPE_SPEED = 0.8f;

    const int SQUARE_DAMAGE = 47;
    const int CIRCLE_DAMAGE = 30;
    const int TRIANGLE_SHIELD_AMOUNT = 60;
    const float SQUARE_SPEED = 16f;
    const float CIRCLE_SPEED = 23f;
    const float SQUARE_SIZE = 1.5f;
    const float CIRCLE_SIZE = 1.1f;
    const float CIRCLE_FREEZE_DURATION = 2.2f;
    const float TRIANGLE_SHIELD_DURATION = 6f;
    const float FIRE_DELAY = 0.8f;
    const float SPELL_SWITCH_DELAY = 0.4f;
    const float CHARGE_SLOW_AMOUNT = 0.3f;

    [SerializeField] GameObject _bulletPrefab = null;

    GameObject shieldObject;

    int currentSpell;
    float reloadTime = 0f;
    float spellSwitchTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = _bulletPrefab;
        maxSpeed = SHAPE_SPEED;
        maxHealth = MAX_HEALTH;
        shieldObject = transform.Find("Shield").gameObject;

        RotateSprite90();

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
            transform.GetChild(currentSpell).gameObject.SetActive(true);
            if (FireKeyHold())
            {
                SelfSlow(true, CHARGE_SLOW_AMOUNT);
                spellSwitchTime += Time.deltaTime;
                if (spellSwitchTime >= SPELL_SWITCH_DELAY)
                {
                    transform.GetChild(currentSpell).gameObject.SetActive(false);
                    currentSpell++;
                    if (currentSpell > 2)
                        currentSpell = 0;
                    transform.GetChild(currentSpell).gameObject.SetActive(true);

                    spellSwitchTime = 0f;
                }

            }
            if (FireKeyUp())
            {
                SelfSlow(false);
                switch (currentSpell)
                {
                    case 0:
                        Fire(SQUARE_SPEED, SQUARE_DAMAGE, SQUARE_SIZE, SQUARE_SIZE, transform.position).GetComponent<BulletHeptagon>().SetSpell(true);
                        break;
                    case 1:
                        Fire(CIRCLE_SPEED, CIRCLE_DAMAGE, CIRCLE_SIZE, CIRCLE_SIZE, transform.position).GetComponent<BulletHeptagon>().SetSpell(false);
                        break;
                    case 2:
                        Shield(TRIANGLE_SHIELD_AMOUNT, TRIANGLE_SHIELD_DURATION);
                        shieldObject.SetActive(true);
                        break;
                }

                reloadTime = 0f;
                spellSwitchTime = 0f;
                transform.GetChild(currentSpell).gameObject.SetActive(false);
                currentSpell = 0;
            }
        }

        _Update();
    }

    public void CircleSpell(Collider2D enemy)
    {
        enemy.GetComponent<Shape>().Slow(1f, CIRCLE_FREEZE_DURATION);
    }
}
