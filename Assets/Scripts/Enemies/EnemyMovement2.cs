using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement2: MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float bounciness = 900f;
    private SpriteRenderer rend;
    [SerializeField] private int damageGiven = 1;
    [SerializeField] private float knockbackForce = 400f;
    [SerializeField] private float upwardForce = 180f;
    [SerializeField] private GameObject enemyParticles;


    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        transform.Translate(new Vector2(moveSpeed, 0) * Time.deltaTime);

        if(moveSpeed > 0)
        {
            rend.flipX = false;
        }
        if (moveSpeed < 0)
        {
            rend.flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBlock"))
        {
            moveSpeed = -moveSpeed;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            moveSpeed = -moveSpeed;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<playerMovement>().TakeDamage(damageGiven);
            if(other.transform.position.x > transform.position.x)
            {
                other.gameObject.GetComponent<playerMovement>().TakeKnockback(knockbackForce, upwardForce);
            }
            else
            {
                other.gameObject.GetComponent<playerMovement>().TakeKnockback(-knockbackForce, upwardForce);

            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(other.GetComponent<Rigidbody2D>().velocity.x, 0);
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, bounciness));
            Instantiate(enemyParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
