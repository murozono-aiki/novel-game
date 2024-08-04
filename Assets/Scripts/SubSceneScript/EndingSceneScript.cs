using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneScript : MonoBehaviour
{
    void Start()
    {
        Scene scene = SceneManager.GetSceneByBuildIndex(2);  // EndingSceneを取得
        SceneManager.SetActiveScene(scene);
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(1);  // MainSceneをロード
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
