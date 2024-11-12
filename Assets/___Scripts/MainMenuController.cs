using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Animator blackScreen;

    public void onStartGameButtonClick() 
    {
        StartCoroutine(startGame());
    }

    IEnumerator startGame() 
    {
        blackScreen.SetTrigger("Play");
        yield return new WaitForSeconds(2);
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
