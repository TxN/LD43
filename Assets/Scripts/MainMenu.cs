using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public Button PlayButton = null;
    public Button QuitButton = null;
    public FadeScreen Fader = null;

    void Start() {
        PlayButton.onClick.AddListener(Play);
        QuitButton.onClick.AddListener(Quit);
    }

    void Play() {
        Fader.gameObject.SetActive(true);
        Fader.FadeBlack(0.5f);
        Invoke("Load", 0.6f);
    }

    void Load() {
        SceneManager.LoadScene("PreGame");
    }

    void Quit() {
        Application.Quit();
    }
}
