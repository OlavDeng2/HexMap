using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MenuUtil : MonoBehaviour
{
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        Debug.Break();
#else
			Application.Quit();
#endif
    }

}
