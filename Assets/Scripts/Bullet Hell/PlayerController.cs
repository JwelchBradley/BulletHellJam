using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int[] abilityCosts;

    public int selectedAbility;

    public static PlayerController instance;

    public GameObject lazerCharge, lazerBeam;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameManager.instance.runningFrames)
        {
            Vector3 screenMousePos = Input.mousePosition;
            screenMousePos.z = -Camera.main.transform.position.z;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(screenMousePos);
            transform.up = (Vector3)mousePos - transform.position;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (selectedAbility == 0)
                {
                    PlayerMovement.instance.Blink();
                }
                else if (selectedAbility == 1)
                {
                    StartCoroutine(FiringMaLazor());
                }
            }
        }
    }

    public void SelectAbility(int ability)
    {
        if (selectedAbility == 0)
            PlayerMovement.instance.StopMove();

        selectedAbility = ability;
    }

    public IEnumerator FiringMaLazor()
    {
        GameManager.instance.runningFrames = true;
        float startFrame = GameManager.instance.frameCount;
        while(GameManager.instance.frameCount < startFrame + abilityCosts[1])
        {
            if (GameManager.instance.frameCount == startFrame)
            {
                Instantiate(lazerCharge, transform.position, transform.rotation);
            }
            else if(GameManager.instance.frameCount == startFrame + 6)
            {
                Instantiate(lazerBeam, transform.position, transform.rotation);

                Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + (transform.up * 20), new Vector2(1, 40), transform.rotation.eulerAngles.z);
                
                //RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up);
                foreach(Collider2D hit in hits)
                {
                    if(hit.transform.gameObject.tag == "Bullet")
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
            }
            yield return new WaitForFixedUpdate();
        }
        GameManager.instance.runningFrames = false;
    }

}
