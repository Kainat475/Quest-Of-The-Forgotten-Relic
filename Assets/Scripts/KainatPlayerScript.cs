using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private string playerHealthString = "Player Health";

    [Header("Player Settings")]
    [SerializeField] private Rigidbody2D rb_player;
    [SerializeField] private Collider2D collider_player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Vector3 playerScale;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float playerMove;
    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private float dialoguePositionX;

    [Header("Player Health Settings")]
    [SerializeField] private Image playerHealthSprite;
    private float playerHealthValue;

    [Header("Player Speed Settings")]
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float defaultSpeed = 5f;    
    [SerializeField] private float pushForce = 3f;
    [SerializeField] private float speedThreshold = 0.05f;

    [Header("Player Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isGrounded;

    [Header("Player Power Up Settings")]
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private bool powerUpCollected = false;
    [SerializeField] private float powerUpTimer = 0.0f;
    [SerializeField] private float powerUp_maxTime = 7f;

    [Header("Areeb Power Up Settings")]
    [SerializeField] private bool Areeb_powerUpCollected = false;

    [Header("Cinemachine Settings")]
    [SerializeField] private KainatEarthQuakeScript vCam_cameraShake_script;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioPlayer;

    [Header("Level End Settings")]
    [SerializeField] private Collider2D levelEndTrigger;

    [Header("Safe ZoneTrigger Settings")]
    [SerializeField] private Collider2D safeZoneTrigger;

    string playerHealthDecrease_color1Hexa = "#FFA2A2";
    string playerHealthDecrease_color2Hexa = "#FF7878";
    Color c1, c2;

    string powerUp_postColor_Hexa = "#B9B9B9";
    Color powerUp_postColor;

    

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        playerRenderer = GetComponent<SpriteRenderer>();
        collider_player = GetComponent<Collider2D>();

        playerScale = transform.localScale;
        defaultSpeed = playerSpeed;

        playerHealthSprite.fillAmount = 1;
        //playerHealthSprite.fillAmount = PlayerPrefs.GetFloat(playerHealthString , 1); 

        ColorUtility.TryParseHtmlString(powerUp_postColor_Hexa, out powerUp_postColor);

        ColorUtility.TryParseHtmlString(playerHealthDecrease_color1Hexa, out c1);
        ColorUtility.TryParseHtmlString(playerHealthDecrease_color2Hexa, out c2);
    }

    // Update is called once per frame
    void Update()
    {
        // Taking Player Movement Input (left, right) (a, d)
        playerMove = Input.GetAxis("Horizontal");

        // PowerUp functionality
        if (powerUpCollected)
        {
            playerSpeed = sprintSpeed;
            powerUpTimer += Time.deltaTime;
        }
        else
            playerSpeed = defaultSpeed;

        if(powerUpTimer >= powerUp_maxTime)
        {
            powerUpCollected = false;
            powerUpTimer = 0.0f;    
            playerSpeed = defaultSpeed;
        }

        // Areeb PowerUp Functionality
        if(Areeb_powerUpCollected)
        {
            powerUpTimer += Time.deltaTime;
            if(powerUpTimer >= powerUp_maxTime)
            {
                Areeb_powerUpCollected = false;
                powerUpTimer = 0.0f;
            }
        }

        // Flip character sprite based on movement direction
        if (playerMove > 0)
        {
            // left direction facing
            transform.localScale = new Vector3(Mathf.Abs(playerScale.x), playerScale.y, playerScale.z);
            playerAnimator.SetBool("run", true);
            //KainatUIManager.Instance.playWalkAudio();
        }
        else if (playerMove < 0)
        {
            // right direction facing
            transform.localScale = new Vector3(-Mathf.Abs(playerScale.x), playerScale.y, playerScale.z);
            playerAnimator.SetBool("run", true);
            //KainatUIManager.Instance.playWalkAudio();
        }

        // Jumping Functionality
        isGrounded = isGroundeded();

        // Jump allwd only when Space is pressed and character is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerAnimator.SetTrigger("jump");
            isJumping = true;            
            rb_player.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            KainatUIManager.Instance.playJumpAudio();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        // run animation enabler
        if (Mathf.Abs(rb_player.velocity.x) > 0.00000001 && playerMove != 0)
        {
            playerAnimator.SetBool("run", true);
        }
        else
        {
            playerAnimator.SetBool("run", false);
        }

        // Checking if player still has health
        if (playerHealthSprite.fillAmount <= 0)
        {
            audioPlayer.Stop();
            gameOver(); 
         }
    }

    // Dealing with Player Movement in this function
    void FixedUpdate()
    {
        // Applying horizontal (left, right) movement
        rb_player.velocity = new Vector2(playerMove * playerSpeed, rb_player.velocity.y);
        //Debug.Log("Applied Velocity: " +  rb_player.velocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("SandWorm_5"))
        {
            StartCoroutine(ShowDamage());
            decreaseHealth(5);
        }
        else if (collision.collider.CompareTag("SandWorm_10"))
        {
            StartCoroutine(ShowDamage());
            decreaseHealth(10);
        }
        else if (collision.collider.CompareTag("Relic"))
        {
            safeZoneTrigger.enabled = true;
            levelEndTrigger.enabled = true;
        }        
        else if (collision.collider.CompareTag("ice") && !Areeb_powerUpCollected)
        {
            StartCoroutine(ShowDamage());
            decreaseHealth(5);
        }
        else if (collision.collider.CompareTag("bats"))
        {
            StartCoroutine(ShowDamage());
            decreaseHealth(2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            powerUpCollected = true;
            powerUpCollectedAnimationsAndSounds();
        }
        else if(collision.CompareTag("SafeZone_Kainat"))
        {
            levelEndTrigger.enabled = true;
            vCam_cameraShake_script.StopShake();
            KainatUIManager.Instance.dangerAverted();
            audioPlayer.Stop();
            StopCoroutine(addDelayBeforeContinuingAudio());
        }
        else if (collision.CompareTag("LevelEndTrigger"))
        {
            audioPlayer.Stop();
            KainatUIManager.Instance.level2Ended();
            // Stopping player once level end reached
            playerAnimator.SetBool("run", false);
            audioPlayer.Play();
            this.enabled = false;
        }
        else if(collision.CompareTag("PowerUp_Areeb"))
        {
            Debug.Log("Areeb Power Up Picked Up");
            Areeb_powerUpCollected = true;
            powerUpCollectedAnimationsAndSounds();
        }
        else if (collision.CompareTag("Relic_Areeb"))
        {
            audioPlayer.Stop();
            KainatUIManager.Instance.level3Ended();
            playerAnimator.SetBool("run", false);
            audioPlayer.Play();
            this.enabled = false;
        }
    }

    bool isGroundeded()
    {
        // Cast a ray downwards to check for ground
        float raylength = 2f; // Adjust based on character collider size
        LayerMask ground = LayerMask.GetMask("Ground"); // Ensure "Ground" layer exists
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raylength, ground);

        // Debug ray to visualize in the Scene view
        Debug.DrawRay(transform.position, Vector2.down * raylength, Color.yellow);

        // Return true if raycast hits ground
        bool answer = (hit.collider != null);
        //Debug.Log("Ground Detected: " + answer);
        return answer;
    }

    void decreaseHealth(float damage)
    {
        for(int i = 0; i < damage; i++)
        {
            playerHealthSprite.fillAmount -= 0.01f;
        }
        PlayerPrefs.SetFloat(playerHealthString, playerHealthSprite.fillAmount);

        KainatUIManager.Instance.showDamageText(damage);
    }

    IEnumerator addDelayBeforeContinuingAudio()
    {
        yield return new WaitForSeconds(2f);
        audioPlayer.Play();
    }
    IEnumerator ShowDamage()
    {        
        playerRenderer.color = c1;
        rb_player.AddForce(new Vector2(0, pushForce), ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.4f);
        playerRenderer.color = c2;
        yield return new WaitForSeconds(0.4f);
        playerRenderer.color = c1;
        yield return new WaitForSeconds(0.2f);
        playerRenderer.color = c2;
        yield return new WaitForSeconds(0.2f);
        playerRenderer.color = c1;

        yield return new WaitForSeconds(0.2f);
        playerRenderer.color = Color.white;

        KainatUIManager.Instance.playHurtAudio();
    }

    public void gameOver()
    {
        KainatUIManager.Instance.playDeathAudio();
        Time.timeScale = 0;

        // using unscaled time for animations while game over
        playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        playerAnimator.SetTrigger("dead");

        //collider_player.enabled = false;
        this.enabled = false;        
    }

    private void powerUpCollectedAnimationsAndSounds()
    {
        StartCoroutine(powerUp_PlayerAnimation());
        KainatUIManager.Instance.playPowerUpAudio();
    }

    private IEnumerator powerUp_PlayerAnimation()
    {
        playerRenderer.color = Color.white; 
        yield return new WaitForSeconds(0.1f);

        playerRenderer.color = powerUp_postColor;
        yield return new WaitForSeconds(0.1f);

        playerRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);

        playerRenderer.color = powerUp_postColor;
        yield return new WaitForSeconds(0.1f);

        playerRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);

        playerRenderer.color = powerUp_postColor;
        yield return new WaitForSeconds(0.1f);

        playerRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);

        playerRenderer.color = powerUp_postColor;
        yield return new WaitForSeconds(0.1f);

        playerRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);

        playerRenderer.color = powerUp_postColor;
        yield return new WaitForSeconds(0.1f);

        playerRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);

        playerRenderer.color = powerUp_postColor;
        yield return new WaitForSeconds(0.1f);

        playerRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);

        playerRenderer.color = powerUp_postColor;
        yield return new WaitForSeconds(0.1f);

        playerRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
    }
}
