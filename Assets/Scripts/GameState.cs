using EventSys;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoSingleton<GameState> {

	[Header("Windows")]
	public NewspaperWindow   NewspaperWindow   = null;
	public ActionWindow      ActionWindow      = null;
	public EventResultWindow EventResultWindow = null;

	[Header("Scene Objects")]
	public StoryEventManager StoryEventManager = null;
	public PlayerLogic Player = null;

	[Header("Settings")]
	public float MinEventSpawnDelay = 5;
	public float MaxEventSpawnDelay = 10;
	public float TotalGameTime = 300;
	public int   GameSegmentsCount = 8;

	HashSet<Object> _pauseHolders = new HashSet<Object>();

	//Stats
	public int RedPower = 0;
	public int BluePower = 0;
	public int RedDeaths = 0;
	public int BlueDeaths = 0;
	public int Aggression = 0;
	public int ActionCount = 0;

	float _timer = 0f;
	float _lastEventSpawnTime = 0f;
	float _nexEventSpawnTime  = 3f;
	int   _shownSegmentIndex  = -1;


	private void Start() {
		EventManager.Subscribe<Event_StoryPointDone>(this,OnStoryPointDone);
	}

	private void OnDestroy() {
		EventManager.Unsubscribe<Event_StoryPointDone>(OnStoryPointDone);
	}

	void Update() {
		if ( !IsPause ) {
			_timer += Time.deltaTime;
			UpdateEventSpawn();
			UpdateSegmentShow();
            UpdateDeathCounter();
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

	void UpdateSegmentShow() {
        var segmentTime = TotalGameTime / GameSegmentsCount;
        var nextSegmentTime = segmentTime * (_shownSegmentIndex + 1);
        if ( _timer > nextSegmentTime ) {
            _shownSegmentIndex++;
            ShowNewspaper(_shownSegmentIndex);
        }
	}

    void UpdateDeathCounter() {
        if ( Random.Range(0, 100) < 5 ) {
            RedDeaths++;
        }
        if ( Random.Range(0, 100) < 5 ) {
            BlueDeaths++;
        }
    }

    void ShowNewspaper(int newspaperIndex) {
        NewspaperWindow.Show(newspaperIndex);
    }



	public void AddPause(Object holder) {
		_pauseHolders.Add(holder);
	}
	public void RemovePause(Object holder) {
		_pauseHolders.Remove(holder);
	}


	public float CurrentTime {
		get {
			return _timer;
		}
	}

	public float CurrentCompletionPercent {
		get {
			return _timer / TotalGameTime;
		}
	}

	public bool IsPause {
		get {
			return _pauseHolders.Count > 0;
		}
	}

	public bool CanLaunchPlane() {
		return Player.CurrentState == PlayerLogic.PlaneState.Ready;
	}

	public void TryLaunchPlane(StoryMapPoint targetPoint) {
		if ( Player.CurrentState == PlayerLogic.PlaneState.Ready ) {
			Player.StartFly(targetPoint);
		}
	}

	public void TryOpenActionWindow(StoryMapPoint point, StoryEvent storyEvent) {
		if ( ActionWindow.CanShow() ) {
			ActionWindow.ShowWindow(point, storyEvent);
		}
	}


	public void ModifyStats(StoryEvent storyEvent, StoryEvent.ResultOptions.ResultType resultType) {
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

		RedPower   += resultStats.ForceRed;
		BluePower  += resultStats.ForceBlue;
		RedDeaths  += resultStats.LostRed;
		BlueDeaths += resultStats.LostBlue;
		Aggression += resultStats.Aggression;
		if ( resultType != StoryEvent.ResultOptions.ResultType.Nothing ) {
			ActionCount++;
		}
	}

	void OnStoryPointDone(Event_StoryPointDone e) {
		ModifyStats(StoryEventManager.GetEventById(e.EventId), e.ActionType);
	}

}

