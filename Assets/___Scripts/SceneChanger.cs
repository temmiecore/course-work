using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public string sceneName;

    public void ChangeScene() 
    {
        if (sceneName != null)
            GameManager.instance.ChangeScene(sceneName);
        else
            Debug.LogWarning("No scene name given to SceneChanger!");
    }
}
