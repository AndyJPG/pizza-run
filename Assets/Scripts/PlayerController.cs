using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Game Manager
    public GameObject gameManager;
    private GameManager _gameManagerScript;

    // Character rigi and animator
    private Rigidbody playerRb;
    private Animator playerAnim;

    // Particles
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public ParticleSystem sparkParticle;

    // Audio properties
    private AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public AudioClip rewardSound;

    // Object moving script
    private MoveToTheLeft moveToTheLeftScript;
    private float moveSpeed;

    // Character values
    public float jumpForce = 10f;
    public float gravityModifier = 1;
    public bool isOnGround = true;
    public bool hasDoubleJump = true;
    public bool dashMode = false;
    public int pizzaCollected;

    // Score tracking
    public float score = 0;
    private float scoreUpdateInterval = 1f;

    // Properties for switching land
    private float _playerZPos = 4.8f;
    //private float[] playerZPositions = new float[] { 4.8f, 0f, -4.8f };

    // Start is called before the first frame update
    void Start()
    {
        // Initialize pizza collected
        pizzaCollected = 0;

        // Get game manager
        _gameManagerScript = gameManager.GetComponent<GameManager>();

        // Get move to the left script to get current moving speed
        moveToTheLeftScript = GameObject.Find("Ground").GetComponent<MoveToTheLeft>();
        moveSpeed = moveToTheLeftScript.moveSpeed;

        // Initial position behind scene to show game entry
        transform.position = new Vector3(-9f, transform.position.y, _playerZPos);

        // Get player animator and rigidbody
        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();

        // Set animator speed
        playerAnim.SetFloat("Speed_f", 1f);

        // Set jump animation duration with multiplier
        playerAnim.SetFloat("Running_Jump_Animation_Speed_Multiplier", 0.7f);

        Physics.gravity = new Vector3(0, -9.81f, 0);
        Physics.gravity *= gravityModifier;

        // Make dirt particle start by delay half second
        var main = dirtParticle.main;
        main.startDelay = 0.5f;

        // Update score
        Invoke("UpdateScore", scoreUpdateInterval);
    }

    // Update is called once per frame
    void Update()
    {
        // Bug
        //Debug.Log(gameMenuScript.gameStart);
        if (_gameManagerScript.IsGameStart)
        {
            if (!_gameManagerScript.IsGameOver && !_gameManagerScript.IsGameEntry)
            {
                // Jump trigger and animation
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    JumpAction();
                }

                // Dash Mode On
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    DashMode(dashMode: true);
                }

                // Dash Mode Off
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    DashMode(dashMode: false);
                }
            }

            // Game entry animation
            if (_gameManagerScript.IsGameEntry)
            {
                GameEntry();
            }

            // Check if the speed has changed
            if (moveSpeed != moveToTheLeftScript.moveSpeed)
            {
                moveSpeed = moveToTheLeftScript.moveSpeed;
            }

            // Check if player enters furious mode
            //if (pizzaCollected == 4)
            //{
            //    onFuriousMode = true;
            //    pizzaCollected = 0;
            //    Invoke("DisableFuriousMode", 20f);
            //}
        }

    }

    // Perform game entry animation
    private void GameEntry()
    {
        // Move player to position
        Vector3 newPosition = new Vector3(0, 0, _playerZPos);

        transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * 8f);

        if (transform.position == newPosition)
        {
            _gameManagerScript.GameEntryComplete();
        }
    }

    // Update score every seconds
    private void UpdateScore()
    {
        if (!_gameManagerScript.IsGameOver && !_gameManagerScript.IsGameEntry)
        {
            score += 1;

            if (dashMode)
            {
                // If is in dash mode score update faster
                Invoke("UpdateScore", scoreUpdateInterval / 2);
            }
            else
            {
                Invoke("UpdateScore", scoreUpdateInterval);
            }
        }
    }

    // Dash mode
    private void DashMode(bool dashMode)
    {
        // Set dash mode
        this.dashMode = dashMode;
        // Change speed if on dash mode
        float speed;
        if (dashMode)
        {
            speed = 2.0f;
        } else
        {
            speed = 1.0f;
        }
        // Set twice fast the animation
        playerAnim.SetFloat("Running_Animation_Speed_Multiplier", speed);
    }

    // Jump action
    private void JumpAction()
    {
        if (!isOnGround && hasDoubleJump)
        {
            // Set velocity to zero before add another force to prevent acceleration futher from
            // previous velocity
            playerRb.velocity = Vector3.zero;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            hasDoubleJump = false;
        }
        else if (isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        } else
        {
            return;
        }

        // Set back on ground
        isOnGround = false;
        // Trigger jump animation
        playerAnim.SetTrigger("Jump_trig");
        // Stop particle animations
        dirtParticle.Stop();

        // Jump sound effect
        playerAudio.PlayOneShot(jumpSound, 0.2f);
    }

    // Detect collision
    private void OnCollisionEnter(Collision collision)
    {
        if (!_gameManagerScript.IsGameOver)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                // Reset jump when at ground
                isOnGround = true;
                hasDoubleJump = true;
                dirtParticle.Play();
            }
            else if (collision.gameObject.CompareTag("Obstacle"))
            {
                // Stop collision and functionalities when game over
                Debug.Log("Game Over!");
                _gameManagerScript.GameOver();

                // Set animation
                playerAnim.SetBool("Death_b", true);
                playerAnim.SetInteger("DeathType_int", 1);
                explosionParticle.Play();
                dirtParticle.Stop();

                // Crash sound effect
                playerAudio.PlayOneShot(crashSound, 1.0f);

            }
        }
    }

    // Detech rewards
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reward"))
        {
            // Add reward to score
            score += 5;
            sparkParticle.Play();
            playerAudio.PlayOneShot(rewardSound, 1.0f);
            Destroy(other.gameObject);
        }
    }
}
