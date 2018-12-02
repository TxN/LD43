using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DeathsCounter : MonoBehaviour {
    public Text RedCounter  = null;
    public Text BlueCounter = null;

    Sequence _seq = null;
    float _lastAnimTime = 0f;

    int _lastRed = 0;
    int _lastBlue = 0;

    int _threshold = 50;

    void Update() {

        var gs = GameState.Instance;
        if ( gs.CurrentTime - _lastAnimTime > 0.5f ) {
            if ( gs.RedDeaths - _lastRed > _threshold ) {
                Punch(RedCounter.transform);
                _lastAnimTime = gs.CurrentTime;
            }
            if ( gs.BlueDeaths - _lastBlue > _threshold ) {
                Punch(BlueCounter.transform);
                _lastAnimTime = gs.CurrentTime;
            }
        }
        RedCounter.text = string.Format("USI Deaths:     {0}", gs.RedDeaths);
        BlueCounter.text = string.Format("BERD Deaths: {0}", gs.BlueDeaths);

        _lastBlue = gs.BlueDeaths;
        _lastRed = gs.RedDeaths;
    }

    void Punch(Transform obj) {
        obj.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    }
}
