/*****************************************************************************
// File Name :         TailWhipHandler.cs
// Author :            #AUTHOR#
// Contact :           #CONTACT#
// Creation Date :     #DATE#
// Company :           #COMPANY#
//
// Brief Description : Description of what the script does
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailWhipHandler : MonoBehaviour
{
    #region Fields
    private Collider collisionArea;
    #endregion

    #region Functions
    // Start is called before the first frame update
    private void Awake()
    {
        collisionArea = GetComponent<Collider>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (GameManager.instance.runningFrames)
        {
            CheckForBullets();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CheckForBullets()
    {

    }
    #endregion
}
