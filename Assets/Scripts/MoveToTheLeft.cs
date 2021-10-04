using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTheLeft : MonoBehaviour
{
    public float moveSpeed = 20.0f;
    private float leftBoundry = -15f;
    private float zBoundry = 20.0f;
    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

        if (playerControllerScript.dashMode)
        {
            moveSpeed *= 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControllerScript.isGameOver && !playerControllerScript.onGameEntry)
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed *= 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed /= 2;
        }

        
        if (gameObject.CompareTag("Obstacle") || gameObject.CompareTag("Scene"))
        {
            if (playerControllerScript.onFuriousMode)
            {
                Invoke("DelayDestroy", 10f);
            } else if (transform.position.x <= leftBoundry || transform.position.z <= -zBoundry || transform.position.z >= zBoundry)
            {
                // Destroy obstacle and background scene
                Destroy(gameObject);
            }
        }
    }

    private void DelayDestroy()
    {
        Destroy(gameObject);
    }
}
