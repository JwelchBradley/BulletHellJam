/*****************************************************************************
// File Name :         CameraShakeInstanceHandler.cs
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
using Cinemachine;

public class CameraShakeInstanceHandler : MonoBehaviour
{
    #region Fields
    private CinemachineImpulseSource cinemachineImpulseSource;
    #endregion

    #region Functions
    // Start is called before the first frame update
    private void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        GenerateCameraShake();
        Destroy(gameObject, 5.0f);
    }

    private void GenerateCameraShake()
    {
        if (cinemachineImpulseSource)
        {
            cinemachineImpulseSource.GenerateImpulse();
        }
    }
    #endregion
}
