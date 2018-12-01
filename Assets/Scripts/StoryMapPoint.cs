using EventSys;
using UnityEngine;
using UnityEngine.UI;

public class StoryMapPoint : MonoBehaviour {
	public GameObject Plane = null;
	public EventType Type = EventType.Combat;
	public bool Free = true;

	public Image Image = null;

	public Sprite WarIcon = null;
	public Sprite HumIcon = null;

	StoryEvent _curEvent = null;
	float _liveTimer = 0f;
	bool _isPlaneFlyIn = false;
	StoryEvent.ResultOptions.ResultType _actionType = StoryEvent.ResultOptions.ResultType.Nothing;

	private void Start() {
		EventManager.Subscribe<Event_PlayerActionMade>(this, OnPlayerActionMade);
		EventManager.Subscribe<Event_Plane_OnTarget>(this, OnPlaneOnTarget);
	}

	private void Update() {
		if ( _curEvent == null ) {
			return;
		}
		if ( !GameState.Instance.IsPause && !_isPlaneFlyIn ) {
			_liveTimer += Time.deltaTime;
		}
		if ( !_isPlaneFlyIn && _liveTimer > _curEvent.TimeToExpire ) {
			ExpireEvent();
		}
	}

	private void OnDestroy() {
		EventManager.Unsubscribe<Event_PlayerActionMade>(OnPlayerActionMade);
		EventManager.Unsubscribe<Event_Plane_OnTarget>(OnPlaneOnTarget);
	}

	void ExpireEvent() {
		EventManager.Fire(new Event_StoryPointDone() { EventId = _curEvent.Id, Point = this, ActionType = _actionType });
		Free = true;
		_curEvent = null;
		_actionType = StoryEvent.ResultOptions.ResultType.Nothing;
		_liveTimer = 0f;
		Image.enabled = false;
	}

	public void SetupEvent(StoryEvent storyEvent) {
		if ( !Image ) {
			Image = GetComponentInChildren<Image>();
		}
		if ( storyEvent.Type == EventType.Combat ) {
			Image.sprite = WarIcon;
		}
		if ( storyEvent.Type == EventType.Humanitarian ) {
			Image.sprite = HumIcon;
		}
		Image.SetNativeSize();
		Image.enabled = true;

		_curEvent = storyEvent;
		Free = false;
		_liveTimer = 0f;
		_actionType = StoryEvent.ResultOptions.ResultType.Nothing;
	}

	public void OpenActionWindow() {
		if ( _curEvent == null ) {
			return;
		}

	}

	void OnPlayerActionMade(Event_PlayerActionMade e) {
		if ( e.Point != this ) {
			return;
		}
		_actionType = e.ActionType;
		_isPlaneFlyIn = true;
	}

	void OnPlaneOnTarget(Event_Plane_OnTarget e) {
		ExpireEvent();
	}

}
