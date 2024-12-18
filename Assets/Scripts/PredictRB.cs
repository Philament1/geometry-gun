using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictRB : MonoBehaviour
{
    const float FORCE = 10f;

    [SerializeField] float delay = 0f;

    Rigidbody2D rb;
    float startTime;
    float velocity = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        int numFixedUpdates = Mathf.RoundToInt(delay / Time.fixedDeltaTime);
        for (int i = 0; i < numFixedUpdates; i++)
        {
            velocity += (FORCE * Time.fixedDeltaTime) / rb.mass;
            velocity *= (1f - Time.fixedDeltaTime * rb.drag);
            transform.Translate(velocity * Time.fixedDeltaTime * Vector2.right);
        }

        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time - startTime >= delay)
        {
            rb.velocity = Mathf.Max(velocity, rb.velocity.x) * Vector2.right;
            rb.AddForce(FORCE * Vector2.right);
        }
    }
}
