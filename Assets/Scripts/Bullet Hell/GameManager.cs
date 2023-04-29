using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public int frameCount = 1;
    public TMP_Text frameText;

    public static GameManager instance;

    public List<Bullet> bullets;

    float frameTarget;
    
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

            if (frameTarget == frameCount)
                runningFrames = false;
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

    public void RunFrames(int frames)
    {
        if (!runningFrames)
        {
            runningFrames = true;
            frameTarget = frameCount + frames;
        }
    }
}
