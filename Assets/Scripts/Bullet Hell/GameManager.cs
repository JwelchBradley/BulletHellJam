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

    public List<List<Bullet>> ListOfBulletLists = new List<List<Bullet>>();
    public List<Bullet> PlayerBullets = new List<Bullet>();

    public int numLevels;
    public int currentLevel;
    //public List<Bullet> bullets;
    
    public bool runningFrames;

    public bool gameWon = false;

    void Awake()
    {
        ListOfBulletLists = new List<List<Bullet>>(numLevels);

        for (int i = 0; i < numLevels; i++)
        {
            ListOfBulletLists.Add(new List<Bullet>());
        }

        frameText.text = "Frames: " + frameCount;

        instance = this;
    }

    // Fixed Update so that the actual framerate doesnt matter
    void FixedUpdate()
    {
        if (runningFrames && !gameWon)
        {
            NextFrame();
        }

        if (!gameWon)
        {
            if (ListOfBulletLists.Count == 0 || ListOfBulletLists.Count <= currentLevel) return;

            if (ListOfBulletLists[currentLevel].Count == 0)
            {
                currentLevel++;
                Debug.Log("You Win");

                if (currentLevel == numLevels)
                {
                    Debug.Log("You beat the game!");
                    gameWon = true;
                }
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void NextFrame()
    {
        foreach(Bullet bullet in ListOfBulletLists[currentLevel])
        {
            bullet.Move();
        }

        foreach(Bullet bullet in PlayerBullets)
        {
            print("PlayerBullet Exists");

            bullet.Move();
        }

        frameCount++;
        frameText.text = "Frames: " + frameCount;
    }
}
