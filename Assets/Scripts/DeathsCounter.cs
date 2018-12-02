using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathsCounter : MonoBehaviour {
    public Text RedCounter  = null;
    public Text BlueCounter = null;

    void Update() {
        var gs = GameState.Instance;
        RedCounter.text = string.Format("USI Deaths:     {0}", gs.RedDeaths);
        BlueCounter.text = string.Format("BERD Deaths: {0}", gs.BlueDeaths);
    }
}
