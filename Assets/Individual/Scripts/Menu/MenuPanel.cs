using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class MenuPanel : MonoBehaviour {
    public Selectable firstSelected;
    public Button startEvent;
    public Button backEvent;

    void OnEnable() {
        if (firstSelected) {
            StartCoroutine(SelectTask());
        }
    }

	/*
    void Update() {
        if (startEvent) {
            if (InputManager.GetButtonDown(InputDevice.AnyController, InputButton.Start)) {
                startEvent.onClick.Invoke();
            }
        }

        if (backEvent) {
            if (InputManager.GetButtonDown(InputDevice.AnyController, InputButton.Back)) {
                backEvent.onClick.Invoke();
            }
        }
    }
	*/

    IEnumerator SelectTask() {
        yield return null;
        EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
    }
}
