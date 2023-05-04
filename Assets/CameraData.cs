/*****************************************************************************
// File Name :         CameraData.cs
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

public class CameraData : MonoBehaviour
{
    #region Fields
    public static Camera MainCamera { get; private set; }
    public static float StartingCameraFOV { get; private set; }

    public static float StartingCameraOrthoGraphicSize { get; private set; }
    #endregion

    #region Functions
    private void Awake()
    {
        MainCamera = Camera.main;
    }

    // Start is called before the first frame update
    private void Start()
    {
        GetStartingFOV();
        GetStartingOrthoSize();
    }

    private void GetStartingFOV()
    {
        StartingCameraFOV = MainCamera.fieldOfView;
    }

    private void GetStartingOrthoSize()
    {
        StartingCameraOrthoGraphicSize = MainCamera.orthographicSize;
    }

    public static CinemachineVirtualCamera GetCurrentVirtualCamera()
    {
         return Camera.main?.GetComponent<CinemachineBrain>()?.ActiveVirtualCamera?.VirtualCameraGameObject?.GetComponent<CinemachineVirtualCamera>();
    }
    #endregion
}
