using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningSawBlade : MonoBehaviour
{
    public float rotationSpeed = -3;
    public bool isBackwards = false;
    public float movementSpeed = 3;

    //fix

    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + rotationSpeed);

        GetComponent<Rigidbody2D>().velocity = Vector2.right * movementSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SawPoint"))
        {
            if (isBackwards)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                isBackwards = false;
                rotationSpeed = -3;
                movementSpeed = 3;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
                isBackwards = true;
                rotationSpeed = 3;
                movementSpeed = -3;
            }
        }
    }
}
