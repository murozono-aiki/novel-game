using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField] GameManagerScript gameManager;

    [SerializeField] Camera mainCamera;
    [SerializeField] EventSystem eventSystem;

    public void SetSceneInactive()
    {
        mainCamera.enabled = false;
        eventSystem.enabled = false;
    }

    public void SetSceneActive()
    {
        mainCamera.enabled = true;
        eventSystem.enabled = true;
    }
}
