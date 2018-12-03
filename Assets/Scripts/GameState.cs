using EventSys;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoSingleton<GameState> {

	[Header("Windows")]
	public NewspaperWindow   NewspaperWindow   = null;
	public ActionWindow      ActionWindow      = null;
	public EventResultWindow EventResultWindow = null;

	[Header("Scene Objects")]
	public StoryEventManager StoryEventManager = null;
	public PlayerLogic Player = null;
    public FadeScreen  Fader  = null;

	[Header("Settings")]
	public float MinEventSpawnDelay = 5;
	public float MaxEventSpawnDelay = 10;
	public float TotalGameTime = 300;
	public int   GameSegmentsCount = 8;

	HashSet<Object> _pauseHolders = new HashSet<Object>();

	float _timer = 0f;
	float _lastEventSpawnTime = 0f;
	float _nexEventSpawnTime  = 3f;
	int   _shownSegmentIndex  = -1;

    public int RedDeaths {
        get {
            return FinalStatsHolder.Instance.RedDeaths;
        }
        set {
            FinalStatsHolder.Instance.RedDeaths = value;
        }
    }

    public int BlueDeaths {
        get {
            return FinalStatsHolder.Instance.BlueDeaths;
        }
        set {
            FinalStatsHolder.Instance.BlueDeaths = value;
        }
    }

	private void Start() {
		EventManager.Subscribe<Event_StoryPointDone>(this,OnStoryPointDone);
	}

	private void OnDestroy() {
		EventManager.Unsubscribe<Event_StoryPointDone>(OnStoryPointDone);
	}

	void Update() {
        if ( Input.GetKeyDown(KeyCode.P) ) { //Quick skip
            _timer = TotalGameTime * 0.95f;
        }
		if ( Input.GetKeyDown(KeyCode.O) ) { //speedup
			Time.timeScale = 2f;
		}
		if ( !IsPause ) {
			_timer += Time.deltaTime;
			UpdateEventSpawn();
			UpdateSegmentShow();
            UpdateDeathCounter();
            CheckEndgame();
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
        if ( _timer > nextSegmentTime && ActionWindow.CanShow() ) {
            _shownSegmentIndex++;
            ShowNewspaper(_shownSegmentIndex);
        }
	}

    void UpdateDeathCounter() {
        if ( Random.Range(0, 100) < 3 ) {
            RedDeaths++;
        }
        if ( Random.Range(0, 100) < 3 ) {
            BlueDeaths++;
        }
    }

    void CheckEndgame() {
        if ( CurrentCompletionPercent > 1.01f ) {
            _pauseHolders.Add(this);
        } else {
            return;
        }
        ActionWindow.CloseWindow();
        Invoke("LoadEndingScene", 2f);
        Fader.gameObject.SetActive(true);
        Fader.FadeBlack(1.5f);
    }

    void LoadEndingScene() {
        SceneManager.LoadScene("EndingScene");
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

        var fs = FinalStatsHolder.Instance;

        fs.RedPower += resultStats.ForceRed;
        fs.BluePower += resultStats.ForceBlue;
        fs.RedDeaths += resultStats.LostRed;
        fs.BlueDeaths += resultStats.LostBlue;
        fs.Aggression += resultStats.Aggression;
		if ( resultType != StoryEvent.ResultOptions.ResultType.Nothing ) {
            fs.ActionCount++;
		}
	}

	void OnStoryPointDone(Event_StoryPointDone e) {
		ModifyStats(StoryEventManager.GetEventById(e.EventId), e.ActionType);
	}

}

