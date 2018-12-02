using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndgameScene : MonoBehaviour {
    public FadeScreen Fader = null;
    public Image LogoImage = null;
    public Text EndingText = null;

    bool _isLocked = true;

    void Start() {
        SetupEnding();
        Fader.gameObject.SetActive(true);
        Fader.FadeWhite(1.5f);
        Invoke("Unlock", 2f);
    }

    void Update() {
        if ( _isLocked ) {
            return;
        }
        if ( Input.anyKey ) {
            Quit();
        }
    }

    void Unlock() {
        _isLocked = false;
    }

    void SetupEnding() {

    }

    void Quit() {
        FinalStatsHolder.Instance.Destroy();
        SceneManager.LoadScene("StartScene");
    }
}
