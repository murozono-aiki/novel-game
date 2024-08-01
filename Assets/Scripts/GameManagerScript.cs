using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public ScenarioManager scenarioManager;
    public MainTextController mainTextController;
    public ImageManager imageManager;
    public ButtonManager buttonManager;

    [System.NonSerialized] public int lineNumber = 0;
    
    [System.NonSerialized] public bool isWaitingButtonClick = false;
}
