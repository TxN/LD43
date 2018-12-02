using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreGameScene : MonoBehaviour {
    public FadeScreen Fader = null;

    bool _locked = true;

    void Start() {
        Fader.FadeWhite(0.5f);
        Invoke("Unlock", 1f);
    }

    void Unlock() {
        _locked = false;
    }

    void Update() {
        if ( _locked ) {
            return;
        }
        if ( Input.anyKey ) {
            _locked = true;
            Invoke("LoadLevel", 1.5f);
            Fader.FadeBlack(1.2f);
        }
    }

    void LoadLevel() {
        SceneManager.LoadScene("level");
    }
}
