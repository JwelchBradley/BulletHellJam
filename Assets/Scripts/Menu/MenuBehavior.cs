/*****************************************************************************
// File Name :         MenuBehavior.cs
// Author :            Jacob Welch
// Creation Date :     28 August 2021
//
// Brief Description : Handles the chaning of panels and scenes from menus.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuBehavior : MonoBehaviour
{
    #region Variables
    [Tooltip("Animator of level crossfade")]
    public Animator crossfadeAnim;

    [Header("Panels")]
    [Tooltip("The playerpref names of volume controls")]
    [SerializeField] protected string[] InitializeAudioVolume;

    [Tooltip("The master audio mixer")]
    [SerializeField] protected AudioMixer audioMixer;
    #endregion

    #region Functions
    private void Start()
    {
        InitalizeVolume();
    }

    public void StopCoroutines()
    {
        StopAllCoroutines();
    }

    protected void InitalizeVolume()
    {
        foreach(string s in InitializeAudioVolume)
        {
            if (PlayerPrefs.HasKey(s))
            {
                // Converts linear slider value to exponential Audio Group value
                float vol = Mathf.Log10(PlayerPrefs.GetFloat(s)) * 20;

                audioMixer.SetFloat(s, vol);
            }
        }
    }

    #region Panel Setters
    /// <summary>
    /// Sets a panel to be active.
    /// </summary>
    /// <param name="panel">The panel to be set active.</param>
    public void LoadPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    /// <summary>
    /// Sets a panel to be inactive.
    /// </summary>
    /// <param name="panel">The panel to be set inactive.</param>
    public void DisablePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
    #endregion

    public void PlayAudio(AudioSource audio)
    {
        audio.PlayOneShot(audio.clip);
    }

    #region Loading Scenes
    /// <summary>
    /// Loads a scene from a given string.
    /// </summary>
    /// <param name="sceneName">Scene to be loaded.</param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneHelper(sceneName));
    }

    /// <summary>
    /// Loads a scene after the level has faded out.
    /// </summary>
    /// <param name="sceneName">Scene to be loaded.</param>
    /// <returns></returns>
    protected IEnumerator LoadSceneHelper(string sceneName)
    {
        crossfadeAnim.SetBool("levelEnd", true);

        yield return null;

        AsyncOperation loadOp;

        if (sceneName.Equals("Next Level") || sceneName.Equals(""))
        {
            loadOp = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1);
        }
        else
        {
            loadOp = SceneManager.LoadSceneAsync(sceneName);
        }
        // Preloads the scene and then loads it after the scene has faded out
        
        loadOp.allowSceneActivation = false;

        yield return new WaitForSecondsRealtime(crossfadeAnim.GetCurrentAnimatorStateInfo(0).length);

        // Resets game
        Time.timeScale = 1;
        AudioListener.pause = false;
        
        if (sceneName.Equals("Main Menu"))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        loadOp.allowSceneActivation = true;
    }
    #endregion

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
    #endregion
}