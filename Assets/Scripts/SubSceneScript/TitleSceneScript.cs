using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneScript : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene(1);  // MainSceneをロード
    }
}
