using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject ghost;
    int ghostFrames;
    int betweenGhostFrames;

    public bool homing;
    [Tooltip("The Bullet's movement (forward) per frame")]
    public float moveSpeed;
    [Tooltip("The Bullet's turn speed (right) per frame [BECOMES POSITIVE LERP IF HOMING]")]
    public float turnSpeed;

    private void Start()
    {
        GameManager.instance.bullets.Add(this);
    }

    public void Move(bool _ghost = false)
    {
        GameObject obj = gameObject;
        if (_ghost)
            obj = ghost;

        obj.transform.position = obj.transform.position + (obj.transform.up * moveSpeed);
        if (homing)
        {
            Vector3 dir = PlayerMovement.instance.transform.position - obj.transform.position;
            obj.transform.up = Vector3.Lerp(obj.transform.up, dir, turnSpeed);
        }
        else
        {
            obj.transform.Rotate(obj.transform.forward * turnSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (ghost)
        {
            if (GameManager.instance.runningFrames)
            {
                ghost.SetActive(false);
                ghostFrames = 0;
                betweenGhostFrames = 0;
            }
            else
            {
                betweenGhostFrames++;
                if (betweenGhostFrames >= GameManager.framesBetweenGhostFrame)
                {
                    betweenGhostFrames = 0;
                    ghost.SetActive(true);
                    ghostFrames++;
                    Move(true);

                    if (ghostFrames >= GameManager.totalGhostFrames)
                    {
                        ghostFrames = 0;
                        ghost.transform.position = transform.position;
                        ghost.transform.rotation = transform.rotation;
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.bullets.Remove(this);
    }
}
