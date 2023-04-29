using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public GameObject blinkEffectPrefab;

    public GameObject ghost;

    [Tooltip("Max Distance the player can blink")]
    public float blinkDist;
    public int blinkFrames;
    Vector2 blinkPos;

    public static PlayerMovement instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.runningFrames && PlayerController.instance.selectedAbility == 0)
        {
            ShowMove();
        }
    }

    void ShowMove()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector2 clickPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (Vector3.Distance(clickPos, transform.position) > blinkDist)
        {
            Vector2 blinkDir = clickPos - (Vector2)transform.position;
            blinkDir = blinkDir.normalized;

            ghost.SetActive(true);
            ghost.transform.position = transform.position + (Vector3)(blinkDir * blinkDist);
        }
        else
        {
            ghost.SetActive(true);
            ghost.transform.position = clickPos;
        }
        blinkPos = ghost.transform.position;
    }

    public void StopMove()
    {
        blinkPos = transform.position;
        ghost.SetActive(false);
    }

    public void Blink()
    {
        Instantiate(blinkEffectPrefab, transform.position, transform.rotation);
        transform.position = blinkPos;
        StopMove();

        GameManager.instance.RunFrames(blinkFrames);
    }
}
