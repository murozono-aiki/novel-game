using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitConfirmSceneScript : MonoBehaviour
{
    GameManagerScript gameManager;
    Scene scene;
    
    void Start()
    {
        scene = SceneManager.GetSceneByBuildIndex(3);  // QuitConfirmSceneを取得
        SceneManager.SetActiveScene(scene);
        
        Scene mainScene = SceneManager.GetSceneByBuildIndex(1);  // MainSceneを取得
        GameObject[] mainSceneGameObjects = mainScene.GetRootGameObjects();
        foreach (GameObject gameObject in mainSceneGameObjects)
        {
            GameManagerScript targetComponent = gameObject.GetComponent<GameManagerScript>();
            if (targetComponent != null)
            {
                gameManager = targetComponent;
                break;
            }
        }
    }

    public void CancelQuitting()
    {
        SceneManager.sceneUnloaded += OnSceneUnload;
        SceneManager.UnloadSceneAsync(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void OnSceneUnload(Scene unloadedScene)
    {
        if (unloadedScene == scene)
        {
            gameManager.mainSceneManager.SetSceneActive();
            SceneManager.sceneUnloaded -= OnSceneUnload;
        }
    }
}
