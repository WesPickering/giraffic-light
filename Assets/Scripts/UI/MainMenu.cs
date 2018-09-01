using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Instance
    public static MainMenu Instance;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            return;
        }

        Instance = this;
    }
    #endregion

    #region Audio
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip click1;
    [SerializeField]
    AudioClip click2;

    public void PlayClick1()
    {
        audioSource.clip = click1;
        audioSource.Play();
    }

    public void PlayClick2()
    {
        audioSource.clip = click2;
        audioSource.Play();
    }
    #endregion
    
    public void Play()
    {
        PlayClick2();

        Invoke("ToGame", 0.1f);
    }

    void ToGame()
    {
        SceneManager.LoadScene(1);
    }
}
