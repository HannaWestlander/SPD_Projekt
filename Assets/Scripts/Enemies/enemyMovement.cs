using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private Transform checker;
    [SerializeField] private LayerMask whatIsGround;
    private float rayDistance = 0.25f;
    private SpriteRenderer rend;
    [SerializeField] private int damageGiven = 1;
    [SerializeField] private float knockbackForce = 400f;
    [SerializeField] private float upwardForce = 180f;
    [SerializeField] private GameObject enemyParticles;
    private bool canMove = false;
    private Rigidbody2D rgbd;
    private Animator anim;
    [SerializeField] private bool shouldDie = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        canMove = true;
        rend = GetComponent<SpriteRenderer>();
        rgbd = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Ta bort hopprelaterad kod
        // Kolla om fienden är på marken (checkar om fienden står på marken)
        CheckIfGrounded();

        if (canMove == true)
        {
            transform.Translate(new Vector2(moveSpeed, 0) * Time.deltaTime);
        }

        // Flip sprite beroende på rörelse
        if (moveSpeed > 0)
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
            if (other.transform.position.x > transform.position.x)
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
        if (other.CompareTag("Player") && shouldDie == true)
        {
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(other.GetComponent<Rigidbody2D>().velocity.x, 0);
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, upwardForce));
            Instantiate(enemyParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private bool CheckIfGrounded()
    {
        RaycastHit2D rightHit = Physics2D.Raycast(checker.position, Vector2.down, rayDistance, whatIsGround);

        // Här kollar vi om fienden står på marken
        return rightHit.collider != null && rightHit.collider.CompareTag("Ground");
    }
}