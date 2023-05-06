using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FrameEvent
{
    public int frame;
    public GameObject spawn;


    [field: SerializeField]
    public GameObject GhostOnlySpawn { get; private set; }


    public string invoke;

    [field: SerializeField]
    public bool TickEveryFixedUpdate { get; private set; }

    [field:SerializeField]
    public GameObject cameraShake { get; private set; }
}

[CreateAssetMenu(fileName = "New Player Ability", menuName = "New Player Ability", order = 1)]
public class PlayerAbility : ScriptableObject
{
    public int frameCost;

    [field:SerializeField]
    public KeyCode Keybind { get; private set; }

    [field:SerializeField]
    public Sprite AbilityIcon { get; private set; }

    #region Cooldown
    [field:SerializeField]
    public int TurnCooldown { get; private set; }

    [HideInInspector] public int CurrentCooldown = 0;

    public bool CanUseAbility
    {
        get
        {
            return CurrentCooldown == 0;
        }
    }
    #endregion

    public FrameEvent[] events;
}
