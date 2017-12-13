using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class MenuControl : MonoBehaviour {
    private MenuPanel[] menuPanels;

    void Awake() {
        menuPanels = GetComponentsInChildren<MenuPanel>(true);

		ShowFirstPanel();
    }

	public void ShowFirstPanel() {
		menuPanels.ForEach(x => x.gameObject.SetActive(false));
		menuPanels[0].gameObject.SetActive(true);
	}

    public void ShowPanel(MenuPanel menuPanel) {
		menuPanels.ForEach(x => x.gameObject.SetActive(menuPanel.Equals(x)));
    }

    public void ShowPopupPanel(MenuPanel menuPanel){
        menuPanel.gameObject.SetActive(true);
    }

    public void HidePanel(MenuPanel menuPanel) {
        menuPanel.gameObject.SetActive(false);
    }
}