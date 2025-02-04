using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 1175f;
    [SerializeField] private float doubleJumpForce = 800f;
    [SerializeField] private Transform leftFoot, rightFoot;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip doubleJumpSound;
    [SerializeField] private AudioClip healthPickupSound;
    [SerializeField] private AudioClip takeDamageSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip cherriesCollectedSound;
    [SerializeField] private GameObject cherryParticles;
    [SerializeField] private GameObject dustParticles;
    [SerializeField] private playerFootsteps footsteps;
    private bool canTakeDamageAgain = true;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillColor;
    [SerializeField] private Color redHealth;
    [SerializeField] private Color panicHealth;
    [SerializeField] private TMP_Text appleText;
    private float horizontalValue;
    private float rayDistance = 0.1f;
    private bool isGrounded;
    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private Animator anim;
    private int startingHealth = 3;
    private int currentHealth = 0;
    private bool canMove = false;
    private bool hasPlayedCherriesCollectedSound = false;
    public int cherriesCollected = 0;
    private AudioSource _audio;
    private GameObject playerLives;
    private float currentMoveSpeed;
    private bool canResetPosition = true;


    private bool hasDoubleJumped = false;
    

    //viktig information för att förstå koden:

    // Cherries = coins i spelet


    // Start is called before the first frame update
    void Start()
    {
        // Denna invoke gör så att spelaren inte kan röra sig förens 0.75 sekunder
        // har gått i början av spelet.
        Invoke("CanMoveAgain", 0.75f);
        //UI kod uppdaterar cherries 
        appleText.text = "" + cherriesCollected;

        currentHealth = startingHealth;

        //Storear lite Components i variables. Använda dessa istället för att använda 
        // Getcomponent varje gång!
        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfGrounded();
        CheckFootsteps();

        //Samlar ihop spelarens inputs på horizontollen
      horizontalValue = Input.GetAxis("Horizontal");

        //Flip sprite kod
        if (horizontalValue < 0)
        {
            FlipSprite(true);
        }

        if (horizontalValue > 0)
        {
            FlipSprite(false);
        }

        //Jump kod
        if (UnityEngine.Input.GetButtonDown("Jump") && CheckIfGrounded() == true && canMove == true)
        {
            Jump();
        }

        //DoubleJump Kod
        if (UnityEngine.Input.GetButtonDown("Jump") && hasDoubleJumped == false && CheckIfGrounded() == false && canMove == true)
        {
            rgbd.velocity = new Vector2(0, 0);
            DoubleJump();
            hasDoubleJumped = true;
        }

        //Animations kod för att springa, hoppa och att falla, håll utkik i animations tabben
        //i Unity.
        anim.SetFloat("MoveSpeed", Mathf.Abs(rgbd.velocity.x));
        anim.SetFloat("VertialSpeed", rgbd.velocity.y);
        anim.SetBool("IsGrounded", CheckIfGrounded());    
        

        CheckIfGrounded();

        //Om du är grounded har du inte double jumpat
        //Detta betyder att när du hoppar så hoppar du normalt om du står på marken
        //Om du står på luft så gör du en double jump!
        if (CheckIfGrounded())
        {
            hasDoubleJumped = false;
        }

        CheckFootsteps();

        ResetPositionOnPlayerdemand();
    }

    private void ResetPositionOnPlayerdemand()
    {
        if (Input.GetKeyDown(KeyCode.P) && canResetPosition == true)
        {
            canResetPosition = false;
            transform.position = spawnPoint.transform.position;
            currentHealth--;
            UpdateHealthBar();
            Invoke("CanResetPositionToTrue", 5f);

            if (currentHealth <= 0)
            {
                canMove = false;
                GetComponent<PlayerLives>().reduceLives();
                PlayDeathSound();
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                transform.position = spawnPoint.position;
                rgbd.velocity = Vector2.zero;
                Invoke("Respawn", 0.75f);
            }
        }
    }

    private void CanResetPositionToTrue()
    {
        canResetPosition = true;
    }

    private void CheckCherries()
    {
        if (cherriesCollected >= 10 && hasPlayedCherriesCollectedSound == false)
        {
            hasPlayedCherriesCollectedSound = true;
            _audio.PlayOneShot(cherriesCollectedSound, 0.5f);
        }
    }

    private void FixedUpdate()
    {
        if(canMove == false)
        {
            return;
        }
        else
        {
            //Movement koden, Rör aldrig denna.
            rgbd.velocity = new Vector2(horizontalValue * moveSpeed * Time.deltaTime, rgbd.velocity.y);
            

        }

    }

    public void CheckFootsteps()
    {
        if ((CheckIfGrounded() && canMove == true && horizontalValue > 0 && 0.1f < Mathf.Abs(rgbd.velocity.x))
            || horizontalValue < 0 && CheckIfGrounded() & canMove == true && 0.1f < Mathf.Abs(rgbd.velocity.x))
        {
            footsteps.MakeItSo();
        }
        else
        {
            footsteps.MakeItNotSo();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //plocka upp "cherries" alltså Coins
        if (other.CompareTag("Cherry"))
        {
            Destroy(other.gameObject);
            cherriesCollected++;
            appleText.text = "" + cherriesCollected;
            _audio.pitch = Random.Range(0.8f, 1.2f);
            _audio.PlayOneShot(pickupSound, 0.5f);
            Instantiate(cherryParticles, transform.position, Quaternion.identity);
            Invoke("CheckCherries", 0.25f);
        }

        // om du triggrar en health genom att gå in i den får du tillbaka liv.
        if (other.CompareTag("Health"))
        {
            RestoreHealth(other.gameObject);
        }
    }

    private void FlipSprite(bool direction)
    {
        rend.flipX = direction;
    }

    private void Jump()
    {
        rgbd.AddForce(new Vector2(0, jumpForce));        
        _audio.PlayOneShot(jumpSound, 0.5f);
        Instantiate(dustParticles, transform.position, dustParticles.transform.localRotation);
    }

    private void DoubleJump()
    {
        rgbd.AddForce(new Vector2(0, doubleJumpForce));
        _audio.pitch = Random.Range(0.8f, 1.2f);
        _audio.PlayOneShot(doubleJumpSound, 0.5f);
        Instantiate(dustParticles, transform.position, dustParticles.transform.localRotation);
    }

    private bool CheckIfGrounded()
    {

        RaycastHit2D leftHit = Physics2D.Raycast(leftFoot.position, Vector2.down, rayDistance, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(rightFoot.position, Vector2.down, rayDistance, whatIsGround);

        //Debug.DrawRay(leftFoot.position, Vector2.down * rayDistance, Color.blue, 0.5f);
        //Debug.DrawRay(rightFoot.position, Vector2.down * rayDistance, Color.red, 0.5f);


        if (leftHit.collider != null && leftHit.collider.CompareTag("Ground") || (rightHit.collider != null && rightHit.collider.CompareTag("Ground")))
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (canTakeDamageAgain)
        {
            currentHealth -= damageAmount;
            healthSlider.value = currentHealth;
            _audio.PlayOneShot(takeDamageSound, 0.5f);
            UpdateHealthBar();


            if (currentHealth <= 0)
            {
                PlayDeathSound();
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                transform.position = spawnPoint.position;
                rgbd.velocity = Vector2.zero;
                Invoke("Respawn", 0.75f);
            }

            canTakeDamageAgain = false;
            Invoke("TakeDamageAgainTimeReset", 0.1f);
        }
        


       
    }

    public void TakeDamageAgainTimeReset()
    {
        canTakeDamageAgain = true;
    }

    public void PlayDeathSound()
    {
        _audio.PlayOneShot(deathSound, 0.5f);
    }
    public void TakeKnockback(float knockbackForce, float upwards)
    {
        canMove = false;
        if(currentHealth > 0)
        {
            rgbd.AddForce(new Vector2(knockbackForce, upwards));
            Invoke("CanMoveAgain", 0.25f);
        }

    }

    private void CanMoveAgain()
    {
        canMove = true;
    }
    public void CanNotMoveAgain()
    {
        canMove = false;
    }

    public void Respawn()
    {
        if (currentHealth <= 0)
        {
            GetComponent<PlayerLives>().reduceLives();
        }
        canMove = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
        UpdateHealthBar();
        Invoke("CanMoveAgain", 1f);

        


    }
    public void RespawnWithoutResetingHealth()
    {
        canMove = false;
        healthSlider.value = currentHealth;
        UpdateHealthBar();
        Invoke("CanMoveAgain", 1f);
    }


    public void InvokeRespawn()
    {
        Invoke("Respawn", 0.75f);
    }

    private void RestoreHealth(GameObject healthPickup)
    {
        if(currentHealth >= startingHealth)
        {
            return;
        }
        else
        {
            int healthToRestore = healthPickup.GetComponent<HealthPickup>().healthAmount;
            currentHealth += healthToRestore;
            UpdateHealthBar();
            _audio.pitch = Random.Range(0.8f, 1.2f);
            _audio.PlayOneShot(healthPickupSound, 0.25f);
            Destroy(healthPickup);

            if(currentHealth > startingHealth)
            {
                currentHealth = startingHealth;
            }
        }
    }

    private void UpdateHealthBar()
    {
        healthSlider.value = currentHealth;
        if(currentHealth >= 2)
        {
            fillColor.color = redHealth;
        }
        else
        {
            fillColor.color = panicHealth;
        }

    }
}
