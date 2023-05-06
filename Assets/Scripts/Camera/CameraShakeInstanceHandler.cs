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

    private CinemachineVirtualCamera currentActiveVirtualCamera;

    [Range(0.0f, 5.0f)]
    [SerializeField] private float zoomInAmount = 10;

    [Range(0.0f, 10.0f)]
    [SerializeField] private float zoomInTime = 0.1f;

    [Range(0.0f, 10.0f)]
    [SerializeField] private float zoomOutTime = 0.1f;

    [Range(0.0f, 10.0f)]
    [SerializeField] private float zoomStayTime = 0.05f;
    #endregion

    #region Functions
    // Start is called before the first frame update
    private void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

        zoomInAmount = CameraData.StartingCameraOrthoGraphicSize - zoomInAmount;

        GetCurrentVirtualCamera();
    }

    private void GetCurrentVirtualCamera()
    {
        currentActiveVirtualCamera = CameraData.GetCurrentVirtualCamera();
    }

    private void Start()
    {
        GenerateCameraShake();
        Destroy(gameObject, ObjectLifeTime());
    }

    private float ObjectLifeTime()
    {
        return zoomInTime + zoomOutTime + zoomStayTime + 2.0f;
    }

    private void GenerateCameraShake()
    {
        if (cinemachineImpulseSource)
        {
            cinemachineImpulseSource.GenerateImpulse();

            StartCoroutine(ZoomRoutine());
        }
    }

    private IEnumerator ZoomRoutine()
    {
        var zoomInTimer = zoomInTime;
        var zoomOutTimer = zoomOutTime;
        var zoomStayTimer = zoomStayTime;

        while(zoomInTimer > 0.0f)
        {
            zoomInTimer -= Time.deltaTime;

            var inverse = Mathf.InverseLerp(zoomInTime, 0.0f, zoomInTimer);
            var newFOV = Mathf.Lerp(CameraData.StartingCameraOrthoGraphicSize, zoomInAmount, inverse);

            if (currentActiveVirtualCamera)
            {
                currentActiveVirtualCamera.m_Lens.OrthographicSize = newFOV;
            }

            yield return new WaitForEndOfFrame();
        }

        while (zoomStayTimer > 0.0f)
        {
            zoomStayTimer -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        while (zoomOutTimer > 0.0f)
        {
            zoomOutTimer -= Time.deltaTime;

            var inverse = Mathf.InverseLerp(zoomOutTime, 0.0f, zoomOutTimer);
            var newFOV = Mathf.Lerp(zoomInAmount, CameraData.StartingCameraOrthoGraphicSize, inverse);

            if (currentActiveVirtualCamera)
            {
                currentActiveVirtualCamera.m_Lens.OrthographicSize = newFOV;
            }

            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
