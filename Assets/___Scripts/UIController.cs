using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("References")]
    public TMP_Text healthText;
    public TMP_Text moneyText;
    public TMP_Text interactionPopup;

    public void setUIHealth(float amount) 
    {
        healthText.text = amount.ToString();
    }

    public void setUIMoney(int amount) 
    {
        moneyText.text = amount.ToString();
    }

    public void setUIInteractionPopup(string text) 
    {
        interactionPopup.text = text;
    }
}
