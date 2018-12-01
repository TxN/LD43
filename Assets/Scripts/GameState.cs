using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoSingleton<GameState> {

    HashSet<Object> _pauseHolders = new HashSet<Object>();

    float _timer = 0f;

    int RedPower = 0;
    int BluePower = 0;
    int RedDeaths = 0;
    int BlueDeaths = 0;
    int Aggression = 0;
    int ActionCount = 0;

    void Update() {
        if ( !IsPause ) {
            _timer += Time.deltaTime;
        }

    }

    public void AddPause(Object holder) {
        _pauseHolders.Add(holder);
    }
    public void RemovePause(Object holder) {
        _pauseHolders.Remove(holder);
    }

    public bool IsPause {
        get {
            return _pauseHolders.Count > 0;
        }
    }



    
}

public enum ConflictType {

}

