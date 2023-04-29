using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject ghost;

    public bool homing;
    [Tooltip("The Bullet's movement (forward) per frame")]
    public float moveSpeed;
    [Tooltip("The Bullet's turn speed (right) per frame [BECOMES POSITIVE LERP IF HOMING]")]
    public float turnSpeed;

    private void Start()
    {
        GameManager.instance.bullets.Add(this);
    }

    public void Move()
    {
        transform.position = transform.position + (transform.up * moveSpeed);
        if (homing)
        {
            Vector3 dir = PlayerMovement.instance.transform.position - transform.position;
            transform.up = Vector3.Lerp(transform.up, dir, turnSpeed);
        }
        else
        {
            transform.Rotate(transform.forward * turnSpeed);
        }
    }
}
