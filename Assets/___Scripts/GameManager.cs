using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(uiController.gameObject);
            Destroy(player.gameObject);
            // Destroy any other scene-persisting objects
            // Audio
            // ...
            return;
        }
        instance = this;
        SceneManager.sceneLoaded += OnLoadScene;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLoadScene;
    }

    private void Start()
    {
        health = playerController.health;
        money = 0;
        uiController.setUIHealth(health);
        uiController.setUIMoney(money);
    }

    [Header("References")]
    public GameObject player;
    public PlayerController playerController;
    public CharacterController characterController;
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

    public void ChangeScene(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnLoadScene(Scene s, LoadSceneMode mode)
    {
        characterController.enabled = false;
        characterController.transform.position = GameObject.Find("Spawn").transform.position;
        characterController.enabled = true;
    }
}
