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
    Vector2 blinkPos;

    [SerializeField]
    private float normalMoveSpeed = 5;
    private float currentNormalMoveGhostPosition = 0;

    private Rigidbody rigidbody;

    public static PlayerMovement instance;

    Ray ray;

    RaycastHit hit;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.runningFrames && PlayerController.instance.selectedAbility == 1)
        {
            ghost.SetActive(true);
            ShowBlink();
        }
        else if(!GameManager.instance.runningFrames && (PlayerController.instance.selectedAbility == 0)) //|| PlayerController.instance.selectedAbility == 2))
        {

        }
        else
        {
            ghost.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.runningFrames && PlayerController.instance.selectedAbility == 0)
        {
            ghost.SetActive(true);
            ShowNormalMove();
        }
        /*else if (!GameManager.instance.runningFrames && PlayerController.instance.selectedAbility == 2)
        {
            ghost.SetActive(true);
            ShowTailWhip();
        }*/
    }

    #region Ghosts
    private void ShowNormalMove()
    {
        currentNormalMoveGhostPosition += normalMoveSpeed * Time.fixedDeltaTime;
        currentNormalMoveGhostPosition %= normalMoveSpeed;
        ghost.transform.position = transform.position + currentNormalMoveGhostPosition * transform.up;
    }

    void ShowBlink()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector2 clickPos = Camera.main.ScreenToWorldPoint(mousePos);
        ray = new Ray(transform.position, transform.up);

        if (Physics.Raycast(ray, out hit, blinkDist))
        {
            ghost.SetActive(true);
            ghost.transform.position = hit.point;
        }
        else
        {
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
        }

        blinkPos = ghost.transform.position;
    }

    private void ShowTailWhip()
    {

    }
    #endregion

    public void StopMove()
    {
        blinkPos = transform.position;
        ghost.SetActive(false);
    }

    #region Movement Abilities
    public void NormalMovement(float totalFrames)
    {
        transform.position = transform.position + normalMoveSpeed * transform.up / totalFrames;
    }

    public void Blink()
    {
        Instantiate(blinkEffectPrefab, transform.position, transform.rotation);
        transform.position = blinkPos;
    }

    public void TailWhip()
    {

    }

    public void Parry()
    {

    }
    #endregion
}
