using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHexagon : Bullet
{
    GameObject bulletPrefab;
    float fireDelay, bulletSpeed, bulletWidth, bulletHeight;
    float reloadTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _Start();
    }

    // Update is called once per frame
    void Update()
    {
        reloadTime += Time.deltaTime;
        if (reloadTime >= fireDelay)
        {
            reloadTime = 0f;
            Fire();
        }

        _Update();
    }

    public void SetStats(float _fireDelay, float linger, float _bulletWidth, float _bulletHeight, float _bulletSpeed, GameObject _bulletPrefab)
    {
        fireDelay = _fireDelay;
        Destroy(gameObject, linger);
        bulletWidth = _bulletWidth;
        bulletHeight = _bulletHeight;
        bulletSpeed = _bulletSpeed;
        bulletPrefab = _bulletPrefab;
    }

    void Fire()
    {
        if (GameInfo.isOnline)
        {
            object[] parameters = new object[] { bulletPrefab.name, isP1, bulletSpeed, damage, bulletWidth, bulletHeight, (Vector2)transform.position };
            NetworkController.SendCallMethodRPC("MyGameManager", "InstantiateBullet", parameters);
        }

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.transform.localScale = new Vector3(bulletWidth, bulletHeight, 1);
        bullet.GetComponent<Bullet>().Initialise(isP1, bulletSpeed, damage, gameObject);
    }

}
