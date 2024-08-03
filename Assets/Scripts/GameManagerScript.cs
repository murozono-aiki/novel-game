using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public ScenarioManager scenarioManager;
    public MainTextController mainTextController;
    public ImageManager imageManager;
    public ButtonManager buttonManager;
    public AudioManager audioManager;
    public MainSceneManager mainSceneManager;

    [System.NonSerialized] public int lineNumber = 0;
    
    // 分岐のボタンが表示されて押されるのを待っている間はtrueとなり、左クリックによって次の行へ進むのを抑止する
    [System.NonSerialized] public bool isWaitingButtonClick = false;
}
