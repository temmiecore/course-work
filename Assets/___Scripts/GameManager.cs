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
            Destroy(audioManager.gameObject);
            // Destroy any other scene-persisting objects
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
    public AudioManager audioManager;
    // ...

    [Header("Saving data")]
    public string currentScene;
    public int money;
    public float health;

    public void editMoney(int m) { money += m; uiController.setUIMoney(money); }
    public void editHealth(float h) 
    { 
        health += h; uiController.setUIHealth(health);

        if (health <= 0)
        {
            ChangeScene(currentScene);

            health = 100;
            uiController.setUIHealth(health);
            money = 0;
            uiController.setUIMoney(money);
        }
    }

    public void ChangeScene(string sceneName) 
    {
        currentScene = sceneName;
        SceneManager.LoadScene(sceneName);
    }

    public void OnLoadScene(Scene s, LoadSceneMode mode)
    {
        characterController.enabled = false;
        characterController.transform.position = GameObject.Find("Spawn").transform.position;
        characterController.enabled = true;

        if (s.name == "Forest") 
        {
            audioManager.Play("rain");
            audioManager.Play("music_forest");
        }
        else
        {
            audioManager.Stop("rain");
            audioManager.Stop("music_forest");
            audioManager.Play("music_dungeon");
        }
    }
}
