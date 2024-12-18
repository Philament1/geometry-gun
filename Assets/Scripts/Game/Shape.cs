using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shape : MonoBehaviour
{
    const float X_MOVEMENT_FORCE_MULTIPLIER = 1.7f;

    public bool slowed = false, frozen = false, poisoned = false, shielded = false, invulnerable = false;
    public int health;

    protected bool isP1;
    protected bool isControllable;
    protected int maxHealth, shield;
    protected string fireKey;
    protected GameObject bulletPrefab;
    protected float maxSpeed, speed, armour = 0f;

    Color p1Colour = new Color(0.13f, 0.26f, 1f);
    Color p2Colour = new Color(1f, 0f, 0f);
    string horizontalAxis, verticalAxis;
    Rigidbody2D rb;
    Slider healthSlider, shieldSlider;
    Text healthText;
    Vector2 forceDir = Vector2.up;

    bool toggleSlow = false, toggleSpeed = false, toggleShield = false;
    int poisonDamage;
    float selfSlow, enemySlow, speedBoost, poisonTick;
    float healthRegenDelay = 1f,  selfSlowDuration, enemySlowDuration, speedBoostDuration, poisonDuration, tickTimer, shieldDuration;

    // Start is called before the first frame update
    protected void _Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = maxSpeed;
        healthSlider.maxValue = maxHealth;
        health = maxHealth;
        SetHealthBar(health);
    }

    // Update is called once per frame
    protected void _Update()
    {
        healthRegenDelay -= Time.deltaTime;
        if (healthRegenDelay <= 0)
        {
            Heal(1);
            healthRegenDelay = 2f;
        }

        if (enemySlowDuration > 0f)
        {
            enemySlowDuration -= Time.deltaTime;
            if (enemySlow == 1f)
            {
                frozen = true;
                slowed = false;
            }
            else if (enemySlow > 0f)
            {
                slowed = true;
                frozen = false;
            }
        }
        else
        {
            enemySlowDuration = 0f;
            enemySlow = 0f;
            frozen = false;
            slowed = false;
        }

        if (selfSlowDuration > 0f)
        {
            selfSlowDuration -= Time.deltaTime;
        }
        else if (!toggleSlow)
        {
            selfSlowDuration = 0f;
            selfSlow = 0f;
        }

        if (speedBoostDuration > 0f)
        {
            speedBoostDuration -= Time.deltaTime;
        }
        else if (!toggleSpeed)
        {
            speedBoostDuration = 0f;
            speedBoost = 0f;
        }

        float greatestSlow = Mathf.Max(enemySlow, selfSlow);
        speed = maxSpeed * (1 - greatestSlow) * (1 + speedBoost);

        if (poisoned)
        {
            poisonDuration -= Time.deltaTime;
            if (poisonDuration > 0)
            {
                tickTimer -= Time.deltaTime;
                if (tickTimer <= 0)
                {
                    Damage(poisonDamage);
                    tickTimer = poisonTick;
                }
            }
            else
            {
                poisoned = false;
            }
        }

        if (shieldDuration > 0f || toggleShield)
        {
            shieldDuration -= Time.deltaTime;
            if (shield > 0)
            {
                shielded = true;
            }
            else
            {
                shieldDuration = 0f;
                shielded = false;
                toggleShield = false;
            }
        }
        else
        {
            shieldDuration = 0f;
            SetHealthBar(0, false);
            shielded = false;
        }
    }

    private void FixedUpdate()
    {
        if (isControllable)
        {
            float horizontalInput = Input.GetAxisRaw(horizontalAxis);
            float verticalInput = Input.GetAxisRaw(verticalAxis);
            if (isP1 || GameInfo.isOnline)
            {
                verticalInput = Mathf.Clamp(verticalInput + Input.GetAxisRaw("Controller Vertical"), -1f, 1f);
                horizontalInput = Mathf.Clamp(horizontalInput + Input.GetAxisRaw("Controller Horizontal"), -1f, 1f);
            }
            
            forceDir.x = horizontalInput * X_MOVEMENT_FORCE_MULTIPLIER;
            if (verticalInput > 0)
            {
                forceDir.y = 1f;
            }
            else if (verticalInput < 0)
            {
                forceDir.y = -1f;
            }
            rb.AddForce(forceDir * speed, ForceMode2D.Impulse);
        }
    }

    public void SetP1(bool _isP1, bool _isControllable)
    {
        isP1 = _isP1;
        isControllable = _isControllable;
        string playerNum;
        Color playerColour;
        if (isP1)
        {
            GameObject healthBar = GameObject.Find("P1 Health Bar");
            healthSlider = healthBar.GetComponent<Slider>();
            shieldSlider = healthBar.transform.GetChild(1).GetComponent<Slider>();
            healthText = GameObject.Find("P1 Health Bar").GetComponentInChildren<Text>();
            playerNum = "P1";
            playerColour = p1Colour;
        }
        else
        {
            GameObject healthBar = GameObject.Find("P2 Health Bar");
            healthSlider = healthBar.GetComponent<Slider>();
            shieldSlider = healthBar.transform.GetChild(1).GetComponent<Slider>(); 
            healthText = GameObject.Find("P2 Health Bar").GetComponentInChildren<Text>();
            playerNum = "P2";
            playerColour = p2Colour;
            Vector3 tempScale = transform.lossyScale;
            tempScale[0] *= -1;
            transform.localScale = tempScale;
        }
        GetComponent<SpriteRenderer>().color = playerColour;
        horizontalAxis = playerNum + " Horizontal";
        verticalAxis = playerNum + " Vertical";
        fireKey = playerNum + " Fire";
    }
    public void Damage(int damage, bool overrideInvulnerable = false)
    {
        if (invulnerable && !overrideInvulnerable)
        {
            damage = 0;
        }
        int _health = health;
        int _shield = shield;
        if (damage >= _shield)
        {
            damage -= _shield;
            SetHealthBar(0, false);
            damage -= Mathf.FloorToInt(armour * damage);
            _health -= TakeDamage(damage);
        }
        else
        {
            _shield -= damage;
            SetHealthBar(_shield, false);
        }
        if (_health <= 0)
        {
            _health = 0;
        }
        SetHealthBar(_health);
    }
    protected virtual int TakeDamage(int damage)
    {
        return damage;
    }

    protected void SetHealthBar(int newHealthOrShield, bool isHealth = true)            //If isHealth is false, set shield
    {
        if (isHealth)
        {
            health = newHealthOrShield;
            if (health <= 0)
            {
                health = 0;
                MyGameManager.PlayerWon(isP1);
            }
        }
        else
        {
            shield = newHealthOrShield;
        }
        healthSlider.maxValue = maxHealth + shield;
        shieldSlider.maxValue = healthSlider.value = health + shield;
        shieldSlider.value = shield;
        healthText.text = health.ToString();
    }

    protected bool FireKeyHold()
    {
        bool fireKeyHold = Input.GetButton(fireKey);
        if (isP1 || GameInfo.isOnline)
        {
            return Input.GetButton("Controller Fire") || fireKeyHold;
        }
        return fireKeyHold;
    }

    protected bool FireKeyDown()
    {
        bool fireKeyDown = Input.GetButtonDown(fireKey);
        if (isP1 || GameInfo.isOnline)
        {
            return Input.GetButtonDown("Controller Fire") || fireKeyDown;
        }
        return fireKeyDown;
    }

    protected bool FireKeyUp()
    {
        bool fireKeyUp = Input.GetButtonUp(fireKey);
        if (isP1 || GameInfo.isOnline)
        {
            return Input.GetButtonUp("Controller Fire") || fireKeyUp;
        }
        return fireKeyUp;
    }

    protected GameObject Fire(float speed, int damage, float bulletWidth, float bulletHeight, Vector2 bulletSpawnPosition)
    {
        string timestamp = NetworkController.Timestamp.ToString();
        string bulletName = $"{System.Convert.ToInt32(isP1)}{bulletPrefab.name} {timestamp.Substring(Mathf.Max(0, timestamp.Length - 5))}/{Random.Range(0, 1000)}";
        if (GameInfo.isOnline)
        {
            object[] parameters = new object[] { bulletPrefab.name, isP1, speed, damage, bulletWidth, bulletHeight, bulletSpawnPosition, bulletName };
            NetworkController.SendCallMethodRPC("MyGameManager", "InstantiateBullet", parameters);
        }

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);
        bullet.transform.localScale = new Vector3(bulletWidth, bulletHeight, 1);
        bullet.name = bulletName;
        bullet.tag = "Bullet";
        bullet.GetComponent<Bullet>().Initialise(isP1, speed, damage, gameObject);
        return bullet;
    }

    protected void Heal(int heal)
    {
        int _health = health;
        if (_health < maxHealth)
        {
            _health += heal;
            if (_health > maxHealth)
                _health = maxHealth;
        }
        SetHealthBar(_health);
    }
    protected void Shield(int shieldAmount, float _shieldDuration)
    {
        int _shield = shield;
        _shield = shieldAmount;
        shieldDuration = _shieldDuration;
        SetHealthBar(_shield, false);
    }
    protected void Shield(int shieldAmount, bool isStack = true)
    {
        int _shield = shield;
        if (isStack)
        {
            _shield += shieldAmount;
        }
        else
        {
            _shield = shieldAmount;
        }
        toggleShield = true;
        SetHealthBar(_shield, false);
    }

    public void Slow(float speedReduction, float slowDuration)
    {
        if (enemySlowDuration < slowDuration)
        {
            enemySlowDuration = slowDuration;
        }
        if (enemySlow < speedReduction)
        {
            enemySlow = speedReduction;
        }

    }

    protected void SelfSlow(float speedReduction, float slowDuration)
    {
        if (selfSlowDuration < slowDuration)
        {
            selfSlowDuration = slowDuration;
        }
        if (selfSlow < speedReduction)
        {
            selfSlow = speedReduction;
        }
    }

    protected void SelfSlow(bool on, float speedReduction = 0f)
    {
        if (on)
        {
            if (selfSlow < speedReduction)
            {
                selfSlow = speedReduction;
            }
            toggleSlow = true;
        }
        else
        {
            selfSlow = 0f;
            toggleSlow = false;
        }
    }

    protected void SpeedBoost(float speedIncrease, float speedDuration)
    {
        if (speedBoostDuration < speedDuration)
        {
            speedBoostDuration = speedDuration;
        }
        speedBoost += speedIncrease;
    }
    protected void SpeedBoost(bool on, float speedIncrease = 0f)
    {
        if (on)
        {
            speedBoost += speedIncrease;
            toggleSpeed = true;
        }
        else
        {
            speedBoost = 0f;
            toggleSpeed = false;
        }
    }

    public void Poison(float _tick, int _poisonDamage, float _poisonDuration)
    {
        poisoned = true;
        tickTimer = poisonTick = _tick;
        poisonDuration = _poisonDuration;
        poisonDamage = _poisonDamage;
    }

    protected void RotateSprite90()
    {
        transform.Rotate(Vector3.forward * -90f);
        Vector3 tempScale = transform.localScale;
        tempScale.y = tempScale.x;
        transform.localScale = tempScale;
    }
}
