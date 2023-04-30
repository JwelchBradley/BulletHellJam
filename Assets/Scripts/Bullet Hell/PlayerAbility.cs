using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FrameEvent
{
    public int frame;
    public GameObject spawn;
    public string invoke;
}

[CreateAssetMenu(fileName = "New Player Ability", menuName = "New Player Ability", order = 1)]
public class PlayerAbility : ScriptableObject
{
    public int frameCost;

    public FrameEvent[] events;
}
