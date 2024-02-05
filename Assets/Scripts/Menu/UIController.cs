using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;

    private GameObject _activeScreen;
    
    public static UIController UIInstance { get; private set; }

    private void Awake()
    {
        if (UIInstance != null && UIInstance != this)
        {
            Destroy(this);
        }
        else
        {
            UIInstance = this;
        }
    }

    public void ReturnToMenu()
    {
        _activeScreen.SetActive(false);
        _activeScreen = null;
        mainMenu.SetActive(true);
    }

    public void OpenScreen(GameObject newScreen)
    {
        mainMenu.SetActive(false);
        _activeScreen = newScreen;
        _activeScreen.SetActive(true);
    }
}
