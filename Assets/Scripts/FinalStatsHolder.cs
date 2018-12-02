using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalStatsHolder : MonoSingleton<FinalStatsHolder> {
    public int RedPower = 0;
    public int BluePower = 0;
    public int RedDeaths = 0;
    public int BlueDeaths = 0;
    public int Aggression = 0;
    public int ActionCount = 0;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    public void Destroy() {
        Destroy(this.gameObject);
    }
}
