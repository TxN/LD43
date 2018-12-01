using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoSingleton<GameState> {

    public StoryEventManager StoryEventManager = null;

    public PlayerLogic Player = null;

    HashSet<Object> _pauseHolders = new HashSet<Object>();

    public float MinEventSpawnDelay = 5;
    public float MaxEventSpawnDelay = 10;
    public float TotalGameTime      = 300;
    public int   GameSegmentsCount  = 8;

    float _timer = 0f;

    public int RedPower    = 0;
    public int BluePower   = 0;
    public int RedDeaths   = 0;
    public int BlueDeaths  = 0;
    public int Aggression  = 0;
    public int ActionCount = 0;

    float _lastEventSpawnTime = 0f;
    float _nexEventSpawnTime = 3f;

    void Update() {
        if ( !IsPause ) {
            _timer += Time.deltaTime;
            UpdateEventSpawn();
        }

    }


    void UpdateEventSpawn() {
        if ( _timer > _nexEventSpawnTime ) {
           var createdEvent = StoryEventManager.TryCreateNewEvent();
           if ( createdEvent != null ) {
               _lastEventSpawnTime = _timer;
               _nexEventSpawnTime = _timer + Random.Range(MinEventSpawnDelay, MaxEventSpawnDelay);
           }
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

    public bool CanLaunchPlane() {
        return false;
    }

    public void TryLaunchPlane(StoryMapPoint targetPoint) {
        if ( Player.CurrentState == PlayerLogic.PlaneState.Ready ) {
            Player.StartFly(targetPoint);
        }
    }


    public void ModifyStats(StoryEvent storyEvent, StoryEvent.ResultOptions.ResultType resultType ) {
        StoryEvent.ResultOptions resultStats = null;
        for ( int i = 0; i < storyEvent.StoryResults.Count; i++ ) {
            if ( storyEvent.StoryResults[i].Type == resultType ) {
                resultStats = storyEvent.StoryResults[i];
                break;
            }
        }
        if ( resultStats == null ) {
            return;
        }

        RedPower += resultStats.ForceRed;
        BluePower += resultStats.ForceBlue;
        RedDeaths += resultStats.LostRed;
        BlueDeaths += resultStats.LostBlue;
        Aggression += resultStats.Aggression;
        if ( resultType != StoryEvent.ResultOptions.ResultType.Nothing ) {
            ActionCount++;
        }
    }
    
}

