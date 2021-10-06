using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Game Manager
    public GameObject gameManager;
    private GameManager _gameManagerScript;

    // Character rigi and animator
    private Rigidbody _playerRb;
    private Animator _playerAnim;

    // Particles
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public ParticleSystem sparkParticle;

    // Audio properties
    private AudioSource _playerAudio;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public AudioClip rewardSound;

    // Character values
    private float _jumpForce = 650f;
    private bool _isOnGround = true;
    private bool _hasDoubleJump = true;

    // Properties for switching land
    private float _playerZPos = 0; // { 4.8f, 0f, -4.8f };

    // Start is called before the first frame update
    void Start()
    {
        // Get game manager
        _gameManagerScript = gameManager.GetComponent<GameManager>();

        // Initial position behind scene to show game entry
        transform.position = new Vector3(-9f, transform.position.y, _playerZPos);

        // Get player animator and rigidbody
        _playerAnim = GetComponent<Animator>();
        _playerRb = GetComponent<Rigidbody>();
        _playerAudio = GetComponent<AudioSource>();

        // Set animator speed
        _playerAnim.SetFloat("Speed_f", 1f);

        // Set jump animation duration with multiplier
        _playerAnim.SetFloat("Running_Jump_Animation_Speed_Multiplier", 0.7f);

        // Make dirt particle start by delay half second
        var main = dirtParticle.main;
        main.startDelay = 0.5f;
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
                    _gameManagerScript.EnterDashMode();
                    _playerAnim.SetFloat("Running_Animation_Speed_Multiplier", 2.0f);
                }

                // Dash Mode Off
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    _gameManagerScript.ExitDashMode();
                    _playerAnim.SetFloat("Running_Animation_Speed_Multiplier", 1.0f);
                }
            }

            // Game entry animation
            if (_gameManagerScript.IsGameEntry)
            {
                GameEntryAnimation();
            }
        }

    }

    // Perform game entry animation
    private void GameEntryAnimation()
    {
        // Move player to position
        Vector3 newPosition = new Vector3(0, 0, _playerZPos);

        transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * 8f);

        if (transform.position == newPosition)
        {
            _gameManagerScript.GameEntryComplete();
        }
    }

    // Jump action
    private void JumpAction()
    {
        if (!_isOnGround && _hasDoubleJump)
        {
            // Set velocity to zero before add another force to prevent acceleration futher from
            // previous velocity
            _playerRb.velocity = Vector3.zero;
            _playerRb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _hasDoubleJump = false;
        }
        else if (_isOnGround)
        {
            _playerRb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        } 
        else
        {
            return;
        }

        // Set back on ground
        _isOnGround = false;
        // Trigger jump animation
        _playerAnim.SetTrigger("Jump_trig");
        // Stop particle animations
        dirtParticle.Stop();

        // Jump sound effect
        _playerAudio.PlayOneShot(jumpSound, 0.2f);
    }

    // Detect collision
    private void OnCollisionEnter(Collision collision)
    {
        if (!_gameManagerScript.IsGameOver)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                // Reset jump when at ground
                _isOnGround = true;
                _hasDoubleJump = true;
                dirtParticle.Play();
            }
            else if (collision.gameObject.CompareTag("Obstacle"))
            {
                // Stop collision and functionalities when game over
                _gameManagerScript.GameOver();

                // Set animation
                _playerAnim.SetBool("Death_b", true);
                _playerAnim.SetInteger("DeathType_int", 1);
                explosionParticle.Play();
                dirtParticle.Stop();

                // Crash sound effect
                _playerAudio.PlayOneShot(crashSound, 1.0f);
            }
        }
    }

    // Detech rewards
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reward"))
        {
            // Add reward to score
            _gameManagerScript.AddBonusScore(5);
            sparkParticle.Play();
            _playerAudio.PlayOneShot(rewardSound, 1.0f);
            Destroy(other.gameObject);
        }
    }
}
