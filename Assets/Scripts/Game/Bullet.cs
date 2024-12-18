using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected bool isP1;
    protected float speed;
    protected int damage;
    protected GameObject self;

    protected Vector2 movementDir;
    protected string targetName;

    Color p1ColourBullet = new Color(0.14f, 0.38f, 0.74f);
    Color p2ColourBullet = new Color(0.78f, 0.3f, 0.1f);


    // Start is called before the first frame update
    protected void _Start()
    {
    }

    // Update is called once per frame
    protected void _Update()
    {
        transform.Translate(movementDir * speed * Time.deltaTime, Space.World);
    }

    public void Initialise(bool _isP1, float _speed, int _damage, GameObject _self)
    {
        isP1 = _isP1;
        Color bulletColour;
        if (isP1)
        {
            movementDir = Vector2.right;
            targetName = "Player 2";
            bulletColour = p1ColourBullet;
        }
        else
        {
            movementDir = Vector2.left;
            targetName = "Player 1";
            bulletColour = p2ColourBullet;
            Vector3 tempScale = transform.localScale;
            tempScale[0] *= -1;
            transform.localScale = tempScale;
        }
        SetBulletColour(bulletColour);

        speed = _speed;
        damage = _damage;
        self = _self;
    }

    protected virtual void SetBulletColour(Color bulletColour)
    {
        bulletColour.a = GetComponent<SpriteRenderer>().color.a;
        GetComponent<SpriteRenderer>().color = bulletColour;
    }

    public virtual void Disintegrate()
    {
        Destroy(gameObject);
    }

    protected void SyncCollision()
    {
        object[] parameters = new object[] { transform.name, targetName };
        NetworkController.SendCallMethodRPC("MyGameManager", "SyncBulletCollision", parameters);
    }
}
