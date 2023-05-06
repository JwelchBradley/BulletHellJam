using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    int ghostFrames, betweenGhostFrames;

    public PlayerAbility[] abilities;

    private List<AbilityButton> abilityButtons = new List<AbilityButton>();

    public int selectedAbility, health, maxHealth;

    public static PlayerController instance;

    public GameObject abilityButtonPrefab, abilityButtonParent;

    public TMP_Text healthText;

    public List<GameObject> interactingEnemies;

    #region Abilities
    #region Basic Bubble Attack
    [Header("Basic Bubble Attack")]
    [SerializeField] private GameObject bubble;
    [SerializeField] private float bubblesAngle = 10;
    #endregion

    #region Explosive Bubble Attack
    [Header("Explosive Bubble Attack")]
    [SerializeField] private GameObject explosiveBubble;
    #endregion
    #endregion

    #region Health fields
    public Slider playerHealth;

    public Image lerpPlayerHealth;

    public bool lerpingHealth = false;

    float t = 0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        SetupUI();

        maxHealth = 3;

        if(healthText)
        healthText.text = "Health: " + maxHealth;

        InitializeHealth();

        InitializeCooldowns();

        SelectAbility(0);
    }

    private void InitializeCooldowns()
    {
        foreach (var ability in abilities)
        {
            ability.CurrentCooldown = 0;
        }
    }

    void SetupUI()
    {
        for(int i = 0; i < abilities.Length; i++)
        {
            PlayerAbility ability = abilities[i];
            GameObject newButton = Instantiate(abilityButtonPrefab, abilityButtonParent.transform);

            var abilityButton = newButton.GetComponentInChildren<AbilityButton>();
            if (abilityButton)
            {
                abilityButtons.Add(abilityButton);
                abilityButton.SetAbilityNameText(ability.name);
                abilityButton.SetAbilityKeybindText(ability.Keybind.ToString());
                abilityButton.SetAbilityIcon(ability.AbilityIcon);
                abilityButton.SetCooldown(0);
            }

            /*
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            int x = i;
            entry.callback.AddListener(delegate { SelectAbility(x); });
            newButton.GetComponent<EventTrigger>().triggers.Add(entry);*/
        }
    }

    // Update is called once per frame
    private void Update()
    {
        SelectNewAbilityInput();

        UseAbilityInput();

        if (playerHealth != null)
        {
            UpdateHealthBar(health, maxHealth);
            LerpHealth();
        }
    }

    #region Abilities
    private void SelectNewAbilityInput()
    {
        for(int i = 0; i < abilities.Length; i++)
        {
            if (Input.GetKeyDown(abilities[i].Keybind))
            {
                SelectAbility(i);
            }
        }

        /*
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectAbility(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectAbility(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectAbility(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectAbility(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectAbility(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectAbility(5);
        }*/
    }

    private void UseAbilityInput()
    {
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
                            GameObject ghostSpawn = Instantiate(fEvent.spawn, transform.position, transform.rotation, transform);

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
                        else if (fEvent.GhostOnlySpawn)
                        {
                            var extraAngle = 0.0f;

                            GameObject ghostSpawn = Instantiate(fEvent.GhostOnlySpawn, transform.position, transform.rotation);
                            ghostSpawn.transform.rotation *= fEvent.GhostOnlySpawn.transform.rotation;

                            ghostSpawn.transform.parent = transform;

                            if (ghostSpawn.TryGetComponent(out Bullet bullet))
                            {
                                Destroy(bullet.gameObject, 1.0f);
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

        if (selectedAbility == 0 && PlayerMovement.instance)
            PlayerMovement.instance.StopMove();

        abilityButtons[selectedAbility].SetNormalColor();
        selectedAbility = ability;
        abilityButtons[selectedAbility].SetSelectedColor();
    }

    public IEnumerator UseAbility(int ability)
    {
        if (!abilities[ability].CanUseAbility) yield break;

        UpdateAbilityCooldowns();

        abilities[ability].CurrentCooldown = abilities[ability].TurnCooldown;
        abilityButtons[ability].SetCooldown(abilities[ability].CurrentCooldown);

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

                    if (fEvent.cameraShake)
                    {
                        Instantiate(fEvent.cameraShake);
                    }
                }
                else if (fEvent.TickEveryFixedUpdate)
                {
                    if (fEvent.invoke.Length > 0)
                        Invoke(fEvent.invoke, 0f);
                }
            }
            yield return new WaitForFixedUpdate(); 
        }

        ChangeAbilityIfOnCooldown(ability);

        GameManager.instance.runningFrames = false;
    }

    private void ChangeAbilityIfOnCooldown(int ability)
    {
        return;

        if (abilities[ability].TurnCooldown != 0)
        {
            if (ability <= 1)
            {
                SelectAbility(0);
            }
            else
            {
                SelectAbility(2);
            }
        }
    }

    private void UpdateAbilityCooldowns()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            var ability = abilities[i];

            ability.CurrentCooldown = (int)Mathf.Clamp(ability.CurrentCooldown - 1, 0, Mathf.Infinity);
            abilityButtons[i].SetCooldown(ability.CurrentCooldown);
        }
    }

    #region Ability Actions/Invokes
    #region Attack
    private void BubbleShot1()
    {
        Instantiate(bubble, transform.position, transform.rotation);
    }

    private void BubbleShot2()
    {
        Instantiate(bubble, transform.position, transform.rotation * Quaternion.Euler(new Vector3(0, 0, bubblesAngle)));
    }

    private void BubbleShot3()
    {
        Instantiate(bubble, transform.position, transform.rotation * Quaternion.Euler(new Vector3(0, 0, -bubblesAngle)));
    }

    private void ExplosiveBubble()
    {
        Instantiate(explosiveBubble, transform.position, transform.rotation);
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
    #endregion

    #region Movement
    private void NormalMovement()
    {
        PlayerMovement.instance.NormalMovement(abilities[0].frameCost);
    }

    void Blink()
    {
        PlayerMovement.instance.Blink();
    }

    private void Parry()
    {

    }
    #endregion
    #endregion
    #endregion

    #region Enemy/player collisions
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Bullet")
        {
            if (!interactingEnemies.Contains(collision.gameObject))
            {
                interactingEnemies.Add(collision.gameObject);
                health = health - collision.gameObject.GetComponent<Bullet>().damage;
                healthText.text = "Health: " + health;
                if (health <= 0)
                {
                    Debug.Log("You Lose");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (interactingEnemies.Contains(collision.gameObject))
        {
            interactingEnemies.Remove(collision.gameObject);
        }
    }
    #endregion

    #region Health
    /// <summary>
    /// Updates the health bar of the player.
    /// </summary>
    /// <param name="health"></param>
    /// <param name="maxHealth"></param>
    private void UpdateHealthBar(int health, int maxHealth)
    {
            playerHealth.value = (float)health / maxHealth;
    }

    void LerpHealth()
    {
        if (lerpPlayerHealth.fillAmount != playerHealth.value)
        {
            lerpPlayerHealth.fillAmount = Mathf.Lerp(lerpPlayerHealth.fillAmount, playerHealth.value, t);
            t += 0.001f * Time.deltaTime;
        }
        else
        {
            t = 0;
        }

        if (lerpPlayerHealth.fillAmount <= playerHealth.value + 0.01f)
        {
            lerpPlayerHealth.fillAmount = playerHealth.value;
        }
    }

    void InitializeHealth()
    {
        var lerpHealthObj = GameObject.FindGameObjectWithTag("Lerp Bar");
        
        if(lerpHealthObj)
        lerpPlayerHealth = lerpHealthObj.GetComponent<Image>();

        var playerHealthObj = GameObject.FindGameObjectWithTag("Player Health Bar");

        if(playerHealthObj)
        playerHealth = playerHealthObj.GetComponent<Slider>();
    }

    #endregion
}
