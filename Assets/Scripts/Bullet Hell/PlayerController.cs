using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    int ghostFrames, betweenGhostFrames;

    public PlayerAbility[] abilities;

    public int selectedAbility;

    public static PlayerController instance;

    public GameObject abilityButtonPrefab, abilityButtonParent;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        SetupUI();
    }

    void SetupUI()
    {
        for(int i = 0; i < abilities.Length; i++)
        {
            PlayerAbility ability = abilities[i];
            GameObject newButton = Instantiate(abilityButtonPrefab, abilityButtonParent.transform);
            newButton.GetComponentInChildren<TMP_Text>().text = ability.name;

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            int x = i;
            entry.callback.AddListener(delegate { SelectAbility(x); });
            newButton.GetComponent<EventTrigger>().triggers.Add(entry);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectAbility(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectAbility(1);
        

        if (!GameManager.instance.runningFrames)
        {
            Vector3 screenMousePos = Input.mousePosition;
            screenMousePos.z = -Camera.main.transform.position.z;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(screenMousePos);
            transform.up = (Vector3)mousePos - transform.position;

            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(UseAbility(selectedAbility));
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.runningFrames)
        {
            ghostFrames = 0;
            betweenGhostFrames = 0;
        }
        else
        {
            betweenGhostFrames++;
            if (betweenGhostFrames >= GameManager.framesBetweenGhostFrame)
            {
                betweenGhostFrames = 0;
                foreach (FrameEvent fEvent in abilities[selectedAbility].events)
                {
                    if (fEvent.frame == ghostFrames)
                    {
                        if (fEvent.spawn)
                        {
                            GameObject ghostSpawn = Instantiate(fEvent.spawn, transform.position, transform.rotation);

                            SpriteRenderer[] sprs = ghostSpawn.GetComponentsInChildren<SpriteRenderer>();
                            foreach (SpriteRenderer spr in sprs)
                            {
                                Color newColor = spr.color;
                                newColor.a = 0.2f;
                                spr.color = newColor;
                            }

                            ParticleSystem[] parts = ghostSpawn.GetComponentsInChildren<ParticleSystem>();
                            foreach (ParticleSystem part in parts)
                            {
                                ParticleSystem.MainModule main = part.main;
                                UnityEngine.ParticleSystem.MinMaxGradient grad = main.startColor;
                                if (grad.gradient != null)
                                {
                                    for (int i = 0; i < grad.gradient.alphaKeys.Length; i++)
                                    {
                                        grad.gradient.alphaKeys[i].alpha = grad.gradient.alphaKeys[i].alpha * 0.2f;
                                    }
                                }
                                else if(grad.color != null)
                                {
                                    Color newColor = grad.color;
                                    newColor.a = newColor.a * 0.2f;
                                    grad.color = newColor;
                                }
                                main.startColor = grad;
                            }
                        }
                    }
                }
                ghostFrames++;

                if (ghostFrames >= GameManager.totalGhostFrames)
                {
                    ghostFrames = 0;
                }
            }
        }
    }

    public void SelectAbility(int ability)
    {
        if (ability >= abilities.Length)
            return;

        if (selectedAbility == 0)
            PlayerMovement.instance.StopMove();

        selectedAbility = ability;
    }

    public IEnumerator UseAbility(int ability)
    {
        GameManager.instance.runningFrames = true;
        float startFrame = GameManager.instance.frameCount;
        while(GameManager.instance.frameCount < startFrame + abilities[ability].frameCost)
        {
            foreach (FrameEvent fEvent in abilities[ability].events)
            {
                if (fEvent.frame == GameManager.instance.frameCount - startFrame)
                {
                    if(fEvent.spawn)
                        Instantiate(fEvent.spawn, transform.position, transform.rotation);
                    if (fEvent.invoke.Length > 0)
                        Invoke(fEvent.invoke, 0f);
                }
            }
            yield return new WaitForFixedUpdate(); 
        }
        GameManager.instance.runningFrames = false;
    }


    void ShootLazor()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + (transform.up * 20), new Vector2(1, 40), transform.rotation.eulerAngles.z);

        //RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up);
        foreach (Collider2D hit in hits)
        {
            if (hit.transform.gameObject.tag == "Bullet")
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }

    void Blink()
    {
        PlayerMovement.instance.Blink();
    }
}
