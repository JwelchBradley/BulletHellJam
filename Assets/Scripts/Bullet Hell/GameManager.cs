using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int framesBetweenGhostFrame = 3;
    public static int totalGhostFrames = 10;

    public static int CurrentFrameCount
    {
        get
        {
            if (instance == null) return 0;

            return instance.frameCount;
        }
    }

    [HideInInspector]
    public int frameCount = 1;
    public TMP_Text frameText;

    public static GameManager instance;

    public List<Bullet> bullets;
    
    public bool runningFrames;

    void Awake()
    {
        bullets = new List<Bullet>();

        frameText.text = "Frames: " + frameCount;

        instance = this;
    }

    // Fixed Update so that the actual framerate doesnt matter
    void FixedUpdate()
    {
        if (runningFrames)
        {
            NextFrame();
        }

        if (bullets.Count == 0)
        {
            Debug.Log("You Win");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void NextFrame()
    {

        foreach(Bullet bullet in bullets)
        {
            bullet.Move();
        }

        frameCount++;
        frameText.text = "Frames: " + frameCount;
    }
}
