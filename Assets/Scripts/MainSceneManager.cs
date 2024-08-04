using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField] GameManagerScript gameManager;

    [SerializeField] Camera mainCamera;
    [SerializeField] EventSystem eventSystem;

    bool is_active = true;

    public bool Is_active {
        get => is_active;
    }

    public void SetSceneInactive()
    {
        mainCamera.enabled = false;
        eventSystem.enabled = false;
        is_active = false;
    }

    public void SetSceneActive()
    {
        mainCamera.enabled = true;
        eventSystem.enabled = true;
        is_active = true;
    }
}
