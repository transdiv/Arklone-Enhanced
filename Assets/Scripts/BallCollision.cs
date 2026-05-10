using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    Rigidbody2D rb;
    const float margen = 0.2f;
    const float impulse = 0.5f;
    [SerializeField] AudioClip ballFailClip;
    [SerializeField] AudioClip brickClip;
    [SerializeField] AudioClip playerClip;
    [SerializeField] AudioClip wallClip;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyIfIsBlock(collision);
        if (collision.gameObject.name == "Player")
        {
            GameManager.Instance.PlaySoundEffect(playerClip, 0.5f);
            // Horizontal difference between ball and paddle
            // float difference = (transform.position.x - collision.transform.position.x) * 2f;
            float difference = transform.position.x - collision.transform.position.x;
            // New direction
            Vector2 direction = new Vector2(difference, 1).normalized;
            // Maintain constant speed
            float speed = rb.linearVelocity.magnitude;

            rb.linearVelocity = direction * speed;
        }
        if (collision.gameObject.tag == "Wall")
        {
            float x = rb.linearVelocity.x;
            float y = rb.linearVelocity.y;
            float wallSide = 1;
            if (collision.gameObject.name == "WallUp")
            {
                if (Math.Abs(x) <= margen)
                {
                    if (transform.position.x >= 0 ) {
                        x -= impulse;
                    } else
                    {
                        x += impulse;
                    }
                    if (Math.Abs(y) <= margen)
                    {
                        y -= impulse * 5;
                    }

                    rb.linearVelocity = new Vector2(x, y);
                }
            }
            else
            {
                if (collision.gameObject.name == "WallLeft")
                {
                    wallSide = 1;
                }
                if (collision.gameObject.name == "WallRight")
                {
                    wallSide = -1;
                }

                if (Math.Abs(x) < margen)
                {
                    x = Math.Sign(x) * impulse;
                    x = (x == 0) ? wallSide * impulse : x;
                }
                if (Math.Abs(y) < margen)
                {
                    y = Math.Sign(y) * impulse;
                    y = (y == 0) ? wallSide * impulse : y;
                }

                rb.linearVelocity = new Vector2(x, y);
            }
            GameManager.Instance.PlaySoundEffect(wallClip, 0.25f);
        }
    }

    private void DestroyIfIsBlock(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            GameManager.Instance.PlaySoundEffect(brickClip, 0.25f);
            Destroy(collision.gameObject);
            GameManager.Instance.DecreaseBlock();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.PlaySoundEffect(ballFailClip, 0.5f);
        GameManager.Instance.RestartScene();
    }
}

