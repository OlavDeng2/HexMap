using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class MenuControl : MonoBehaviour
{
    private MenuPanel[] menuPanels;

    void Awake()
    {
        menuPanels = GetComponentsInChildren<MenuPanel>(true);

        ShowFirstPanel();
    }

    public void ShowFirstPanel()
    {
        //change into for loop
        for (int i = 0; i > menuPanels.Length; i++)
        {
            menuPanels[i].gameObject.SetActive(false);
        }
        menuPanels[0].gameObject.SetActive(true);
    }

    public void ShowPanel(MenuPanel menuPanel)
    {
        //change into for loop
        for (int i = 0; i > menuPanels.Length; i++)
        {
            menuPanels[i].gameObject.SetActive(false);
        }
        menuPanel.gameObject.SetActive(true);
    }

    public void ShowPopupPanel(MenuPanel menuPanel)
    {
        menuPanel.gameObject.SetActive(true);
    }

    public void HidePanel(MenuPanel menuPanel)
    {
        menuPanel.gameObject.SetActive(false);
    }
}