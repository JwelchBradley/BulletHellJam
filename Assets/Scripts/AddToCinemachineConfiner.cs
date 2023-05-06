/*
 * (Jacob Welch)
 * (AddToCinemachineConfiner)
 * (Food Fight)
 * (Description: )
 */
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToCinemachineConfiner : MonoBehaviour
{
    #region Fields

    #endregion

    #region Functions
    /// <summary>
    /// Handles initilization of components and other fields before anything else.
    /// </summary>
    private void Awake()
    {
        FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = GetComponent<CompositeCollider2D>();
    }
    #endregion
}
