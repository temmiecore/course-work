using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Stealing my own code fr
    public static GameManager instance;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            // Destroy any other scene-traversing objects
            return;
        }
        instance = this;
    }

    private void Start()
    {
        health = playerController.health;
        uiController.setUIHealth(health);
    }

    [Header("References")]
    public GameObject player;
    public PlayerController playerController;
    public UIController uiController;
    // Audio
    // ...

    [Header("Saving data")]
    public int money;
    public float health;

    public void editMoney(int m) { money += m; uiController.setUIMoney(money); }
    public void editHealth(float h) 
    { 
        health += h; uiController.setUIHealth(health);

        if (health <= 0)
        {
            // Game over
        }
    }
}
