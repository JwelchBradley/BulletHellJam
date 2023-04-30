using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

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

            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(UseAbility(selectedAbility));
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
