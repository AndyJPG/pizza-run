using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private PlayerController playerControllerScript;
    private Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        scoreText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text ="Score: " + playerControllerScript.score.ToString();
    }
}
