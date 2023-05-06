/*****************************************************************************
// File Name :         AbilityButton.cs
// Author :            #AUTHOR#
// Contact :           #CONTACT#
// Creation Date :     #DATE#
// Company :           #COMPANY#
//
// Brief Description : Description of what the script does
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    #region Fields
    [SerializeField] private TextMeshProUGUI abilityText;

    [SerializeField] private TextMeshProUGUI keybindText;

    [SerializeField] private TextMeshProUGUI countdownText;

    [SerializeField] private Image abilityIcon;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;
    #endregion

    #region Functions
    private void Awake()
    {
        SetNormalColor();
    }

    public void SetAbilityNameText(string newText)
    {
        if (abilityText == null) return;

        abilityText.text = newText;
    }

    public void SetAbilityKeybindText(string newKeybind)
    {
        if (keybindText == null) return;


        if (newKeybind.Contains("Alpha"))
        {
            newKeybind = newKeybind.Remove(0, 5);
        }

        keybindText.text = newKeybind;
    }


    public void SetAbilityIcon(Sprite newAbilitySprite)
    {
        if (abilityIcon == null || newAbilitySprite == null) return;

        abilityIcon.sprite = newAbilitySprite;
    }

    public void SetSelectedColor()
    {
        SetColors(1);
    }

    public void SetNormalColor()
    {
        SetColors(0.2f);
    }

    private void SetColors(float alpha)//Color newColor)
    {
        GetComponent<CanvasGroup>().alpha = alpha;

        /*
        if (abilityText)
        {
            abilityText.color = newColor;
        }

        if (abilityIcon)
        {
            abilityIcon.color = newColor;
        }*/
    }

    public void SetCooldown(int newCooldown)
    {
        if (countdownText == null) return;

        countdownText.transform.parent.gameObject.SetActive(newCooldown != 0);
        countdownText.text = newCooldown.ToString();
    }
    #endregion
}
