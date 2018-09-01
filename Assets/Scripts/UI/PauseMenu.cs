using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject pauseMenuUI;

    public void Pause()
    {
        pauseUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;

        GameManager.Instance.PlayClick2();
    }

    public void Unpause()
    {
        pauseUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;

        GameManager.Instance.PlayClick2();
    }

    public void ExitGame()
    {
        pauseUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;

        GameManager.Instance.PlayClick2();
        Invoke("ToMenu", 0.03f);
    }

    void ToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
