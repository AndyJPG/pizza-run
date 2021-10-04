using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    // Game Menu
    private GameMenu gameMenuScript;

    // Object moving script
    private MoveToTheLeft moveToTheLeftScript;
    private float moveSpeed;

    // Character values
    public float jumpForce = 10f;
    public float gravityModifier = 1;
    public bool isOnGround = true;
    public bool isGameOver = false;
    public bool hasDoubleJump = true;
    public bool dashMode = false;
    public bool onGameEntry = true;
    public bool onFuriousMode;
    public int pizzaCollected;

    // Score tracking
    public float score = 0;
    private float scoreUpdateInterval = 1f;

    // Properties for switching land
    private float switchStep = 15f;
    private int playerZPosIndex = 0;
    private float[] playerZPositions = new float[] { 4.8f, 0f, -4.8f };

    // Start is called before the first frame update
    void Start()
    {
        // Initialize furious mode
        onFuriousMode = false;

        // Initialize pizza collected
        pizzaCollected = 0;

        // Get game menu script
        gameMenuScript = GameObject.Find("Menu").GetComponent<GameMenu>();

        // Get move to the left script to get current moving speed
        moveToTheLeftScript = GameObject.Find("Ground").GetComponent<MoveToTheLeft>();
        moveSpeed = moveToTheLeftScript.moveSpeed;

        // Initial position behind scene to show game entry
        transform.position = new Vector3(-9f, transform.position.y, playerZPositions[playerZPosIndex]);

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
        if (gameMenuScript.gameStart)
        {
            if (!isGameOver && !onGameEntry)
            {
                // Jump trigger and animation
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isOnGround)
                    {
                        JumpAction();
                    }
                    else if (hasDoubleJump)
                    {
                        JumpAction(isDoubleJump: true);
                        hasDoubleJump = false;
                    }
                }

                // Move to left land
                //if (Input.GetKeyDown(KeyCode.LeftArrow))
                //{
                //    SwitchLand(-1);
                //    Debug.Log("Left arrow");
                //}

                // Move to right land
                //if (Input.GetKeyDown(KeyCode.RightArrow))
                //{
                //    SwitchLand(1);
                //    Debug.Log("Right arrow");

                //}

                // Dash Mode
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    dashMode = true;
                    DashMode(speed: 2.0f);
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    dashMode = false;
                    DashMode(speed: 1.0f);
                }

                // Switch lands
                if (transform.position.z != playerZPositions[playerZPosIndex])
                {
                    Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, playerZPositions[playerZPosIndex]);

                    transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * switchStep);
                }
            }

            // Game entry animation
            if (onGameEntry)
            {
                // Move player to position
                Vector3 newPosition = new Vector3(0, 0, playerZPositions[playerZPosIndex]);

                transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * 8f);

                if (transform.position == newPosition)
                {
                    onGameEntry = false;
                }
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

    private void DisableFuriousMode()
    {
        onFuriousMode = false;
    }

    private void UpdateScore()
    {
        if (!isGameOver)
        {
            if (!onGameEntry)
            {
                score += 1;
            }

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

    private void DashMode(float speed)
    {
        // Set twice fast the animation
        playerAnim.SetFloat("Running_Animation_Speed_Multiplier", speed);
    }

    private void JumpAction(bool isDoubleJump = false)
    {
        if (isDoubleJump)
        {
            // Set velocity to zero before add another force to prevent acceleration futher from
            // previous velocity
            playerRb.velocity = Vector3.zero;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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

    // Switch land
    private void SwitchLand(int zPosIndex)
    {
        // Get new position index
        int newZPosIndex = playerZPosIndex + zPosIndex;

        // Check if index is within the range
        if (newZPosIndex >= 0 && newZPosIndex < playerZPositions.Length)
        {
            playerZPosIndex = newZPosIndex;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isGameOver)
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
                isGameOver = true;
                playerAnim.SetBool("Death_b", true);
                playerAnim.SetInteger("DeathType_int", 1);
                explosionParticle.Play();
                dirtParticle.Stop();

                // Crash sound effect
                playerAudio.PlayOneShot(crashSound, 1.0f);

            }
            else if (collision.gameObject.CompareTag("FuriousObject"))
            {

                float collideForce = moveSpeed * 40f;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * collideForce, ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reward"))
        {
            score += 5;
            sparkParticle.Play();
            playerAudio.PlayOneShot(rewardSound, 1.0f);
            Destroy(other.gameObject);
        }
    }
}
