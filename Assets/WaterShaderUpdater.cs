/*****************************************************************************
// File Name :         WaterShaderUpdater.cs
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

public class WaterShaderUpdater : MonoBehaviour
{
    #region Fields
    private Renderer waterRenderer;
    [SerializeField] private float timeStepBetweenFrames = 0.25f;
    #endregion

    #region Functions
    private void Awake()
    {
        waterRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        UpdateWaterRenderer();
    }

    private void UpdateWaterRenderer()
    {
        if (waterRenderer == null) return;

        waterRenderer.material.SetFloat("_ShaderTime", GameManager.CurrentFrameCount*timeStepBetweenFrames);
    }
    #endregion
}
