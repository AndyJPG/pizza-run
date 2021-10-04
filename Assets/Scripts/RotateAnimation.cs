using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    private float rotateSpeed = 40f;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
