using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coral : MonoBehaviour
{
    public int coralHealth = 4;

    public void FixedUpdate()
    {
        if (coralHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
