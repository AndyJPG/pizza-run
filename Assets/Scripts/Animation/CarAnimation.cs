using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimation : MonoBehaviour
{
    // Animation variables
    private float _viberationRate = 25f;
    private float _viberationSpeed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.deltaTime * _viberationSpeed) * _viberationRate, transform.position.z);
    }
}
