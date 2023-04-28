/*****************************************************************************
// File Name :         PauseMenuBehavior.cs
// Author :            Jacob Welch
// Creation Date :     28 August 2021
//
// Brief Description : Handles the pause menu and allows players to pause the game.
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Cinemachine;

public class PauseMenuBehavior : MenuBehavior
{
    #region Fields
    /// <summary>
    /// Holds true if the game is currently paused.
    /// </summary>
    private bool isPaused = false;

    /// <summary>
    /// Enables and disables the pause feature.
    /// </summary>
    private bool canPause = false;

    /// <summary>
    /// Holds true if the pause menu is able to be closed.
    /// </summary>
    private bool canClosePauseMenu = true;

    /// <summary>
    /// Holds true if the game is able to be paused.
    /// </summary>
    public bool CanPause
    {
        get => canPause;
        set
        {
            canPause = value;
        }
    }

    bool wasActiveBefore = false;

    [Tooltip("The pause menu gameobject")]
    [SerializeField] private GameObject pauseMenu = null;
    #endregion

    #region Functions
    /// <summary>
    /// Initializes components.
    /// </summary>
    private void Awake()
    {
        StartCoroutine(WaitFadeIn());
    }

    private IEnumerator WaitFadeIn()
    {
        yield return new WaitForSeconds(crossfadeAnim.GetCurrentAnimatorStateInfo(0).length);

        canPause = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    /// <summary>
    /// If the player hits the pause game key, the game is paused.
    /// </summary>
    public void PauseGame()
    {
        // Opens pause menu and pauses the game
        if (canPause && canClosePauseMenu)
        {
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);
            AudioListener.pause = isPaused;
            Time.timeScale = Convert.ToInt32(!isPaused);

            if (isPaused)
            {
                if(Cursor.lockState == CursorLockMode.Confined)
                {
                    wasActiveBefore = true;
                }
                else
                {
                    wasActiveBefore = false;
                }
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else if(!wasActiveBefore)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void CanClosePauseMenu(bool canClose)
    {
        canClosePauseMenu = canClose;
    }

    /// <summary>
    /// Restarts the current level from the beginning.
    /// </summary>
    public void RestartLevel()
    {
        canPause = false;

        StartCoroutine(LoadSceneHelper(SceneManager.GetActiveScene().name));
    }
    #endregion
}