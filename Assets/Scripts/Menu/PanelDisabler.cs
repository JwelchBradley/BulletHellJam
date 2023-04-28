/*****************************************************************************
// File Name :         PanelDisabler.cs
// Author :            Jacob Welch
// Creation Date :     28 August 2021
//
// Brief Description : Disables this panel if the player presses escape.
*****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelDisabler : MonoBehaviour
{
    private PauseMenuBehavior pauseMenu;

    private void Awake()
    {
        pauseMenu = FindObjectOfType<PauseMenuBehavior>();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu != null)
            {
                pauseMenu.CanClosePauseMenu(true);
            }

            gameObject.SetActive(false);
        }
    }
}
