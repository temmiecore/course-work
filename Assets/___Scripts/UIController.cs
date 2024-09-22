using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("References")]
    public TMP_Text healthText;
    public TMP_Text moneyText;

    public void setUIHealth(float amount) 
    {
        healthText.text = "Health: " + amount;
    }

    public void setUIMoney(int amount) 
    {
        moneyText.text = "Money: " + amount;
    }
}
