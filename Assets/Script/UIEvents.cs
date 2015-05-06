using System;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents: MonoBehaviour
{
    public static event EventHandler ShowingMainMenu;
    public static event EventHandler HidingMainMenu;
    public static event EventHandler ShowingNewGameMenu;
    public static event EventHandler HidingNewGameMenu;

    private static UIEvents _instance;

    public static UIEvents Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIEvents>();
            }

            return _instance;
        }
    }

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        if (ShowingMainMenu != null)
        {
            ShowingMainMenu(null, EventArgs.Empty);
        }
    }

    public void HideMainMenu()
    {
        if (HidingMainMenu != null)
        {
            HidingMainMenu(null, EventArgs.Empty);
        }
    }

    public void ShowNewGameMenu()
    {
        if (ShowingNewGameMenu != null)
        {
            ShowingNewGameMenu(null, EventArgs.Empty);
        }
    }

    public void HideNewGameMenu()
    {
        if (HidingNewGameMenu != null)
        {
            HidingNewGameMenu(null, EventArgs.Empty);
        }
    }
}


