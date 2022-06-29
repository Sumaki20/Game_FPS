using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void loadScene (string scenename) {
        SceneManager.LoadScene (scenename);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
