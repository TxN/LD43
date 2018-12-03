using UnityEngine;
using UnityEngine.UI;
using EventSys;

using DG.Tweening;

public class ActionWindow : MonoBehaviour {
	public GameObject Back = null;
	public Text Title = null;
	public Text Description = null;
	public Button RedWarButton = null;
	public Button BlueWarButton = null;
	public Button HumButton = null;

	public GameObject WarVariantsHolder = null;
	public GameObject HumVariantsHolder = null;

	StoryMapPoint _curPoint = null;
	StoryEvent _curEvent = null;

	Sequence _seq = null;

	bool _actionEnabled = false;

	private void Start() {
		EventManager.Subscribe<Event_StoryPointDone>(this, OnStoryPointDone);
	}

	private void OnDestroy() {
		EventManager.Unsubscribe<Event_StoryPointDone>(OnStoryPointDone);
		_seq = TweenHelper.ResetSequence(_seq);
	}

	public bool CanShow() {
		return !gameObject.activeSelf;
	}

	public void ShowWindow(StoryMapPoint point, StoryEvent storyEvent) {
		if ( point == null || storyEvent == null ) {
			return;
		}
		gameObject.SetActive(true);
		_actionEnabled = false;
		Back.transform.localScale = Vector3.zero;
		_seq = TweenHelper.ReplaceSequence(_seq);
		_seq.Append(Back.transform.DOScale(1f, 0.35f));
		_seq.AppendCallback(() => { _actionEnabled = true; });
		WarVariantsHolder.SetActive(storyEvent.Type == EventType.Combat);
		HumVariantsHolder.SetActive(storyEvent.Type == EventType.Humanitarian);
		RedWarButton.gameObject.SetActive(storyEvent.RedTeam == true);
		BlueWarButton.gameObject.SetActive(storyEvent.BlueTeam == true);
		_curEvent = storyEvent;
		_curPoint = point;
        Title.text = _curEvent.Title;
        Description.text = _curEvent.Description;
	}

	public void Update() {
		var gs = GameState.Instance;
		if ( gs ) {
			var flag = gs.CanLaunchPlane();
			RedWarButton.interactable = flag;
			BlueWarButton.interactable = flag;
			HumButton.interactable = flag;
		}
	}

	public void CloseWindow() {
		if ( !_actionEnabled ) {
			return;
		}
		_seq = TweenHelper.ReplaceSequence(_seq);
		_seq.Append(Back.transform.DOScale(0, 0.35f));
		_seq.AppendCallback(() => { _actionEnabled = true; gameObject.SetActive(false); });
		
		// TODO: Anim
	}

	public void SelectRedWarVariant() {
		if ( !_actionEnabled || _curEvent == null || _curPoint == null ) {
			return;
		}

		EventManager.Fire(new Event_PlayerActionMade() { ActionType = StoryEvent.ResultOptions.ResultType.Red, EventId = _curEvent.Id, Point = _curPoint });
		var gs = GameState.Instance;
		gs.TryLaunchPlane(_curPoint);
		CloseWindow();
	}

	public void SelectBlueVarVariant() {
		if ( !_actionEnabled || _curEvent == null || _curPoint == null ) {
			return;
		}
		EventManager.Fire(new Event_PlayerActionMade() { ActionType = StoryEvent.ResultOptions.ResultType.Blue, EventId = _curEvent.Id, Point = _curPoint });
		var gs = GameState.Instance;
		gs.TryLaunchPlane(_curPoint);
		CloseWindow();

	}

	public void SelectHumVariant() {
		if ( !_actionEnabled || _curEvent == null || _curPoint == null ) {
			return;
		}
		if ( _curEvent.RedTeam ) {
			EventManager.Fire(new Event_PlayerActionMade() { ActionType = StoryEvent.ResultOptions.ResultType.Red, EventId = _curEvent.Id, Point = _curPoint });
		} else {
			EventManager.Fire(new Event_PlayerActionMade() { ActionType = StoryEvent.ResultOptions.ResultType.Blue, EventId = _curEvent.Id, Point = _curPoint });
		}
		var gs = GameState.Instance;
		gs.TryLaunchPlane(_curPoint);
		CloseWindow();
	}

	void OnStoryPointDone(Event_StoryPointDone e) {
		if ( e.Point == _curPoint && _actionEnabled && gameObject.activeSelf ) {
			CloseWindow();
		}
	}
}
