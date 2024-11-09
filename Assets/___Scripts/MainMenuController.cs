using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void onStartGameButtonClick() 
    {
        SceneManager.LoadScene("Forest");
    }

    public void onOptionsButtonClick() 
    { 
        // Open options menu
        // Mouse speed, volume, resolution? turn off some visual effects idk
    }

    public void onExitGameButtonClick()
    {
        Application.Quit();
    }
}
