using EventSys;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class StoryMapPoint : MonoBehaviour {
   
	public EventType Type = EventType.Combat;
    public GameObject SelectedObj = null;
	public bool Free = true;

	public Image Image = null;

	public Sprite WarIcon = null;
	public Sprite HumIcon = null;

	StoryEvent _curEvent = null;
	float _liveTimer = 0f;
	bool _isPlaneFlyIn = false;
	StoryEvent.ResultOptions.ResultType _actionType = StoryEvent.ResultOptions.ResultType.Nothing;

    Sequence _seq = null;

	private void Start() {
        if ( !Image ) {
            Image = GetComponent<Image>();
        }
        if ( Free ) {
            Image.enabled = false;
        }
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
        if ( _curEvent == null ) {
            return;
        }
		EventManager.Fire(new Event_StoryPointDone() { EventId = _curEvent.Id, Point = this, ActionType = _actionType });
	//	Free = true;
		_curEvent = null;
		_actionType = StoryEvent.ResultOptions.ResultType.Nothing;
		_liveTimer = 0f;
		
        _seq = TweenHelper.ReplaceSequence(_seq);
        _seq.Append(transform.DOScale(0, 0.35f));
        _seq.AppendCallback(() =>
        {
            Image.enabled = false;
            SelectedObj.SetActive(false);
            Free = true;
            _isPlaneFlyIn = false;
        });
        
	}

	public void SetupEvent(StoryEvent storyEvent) {

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
        transform.localScale = Vector3.zero;
        _seq = TweenHelper.ReplaceSequence(_seq);
        _seq.Append(transform.DOScale(1, 0.35f));
	}

	public void OpenActionWindow() {
		if ( _curEvent == null || _isPlaneFlyIn ) {
			return;
		}
		GameState.Instance.TryOpenActionWindow(this, _curEvent);
	}

	void OnPlayerActionMade(Event_PlayerActionMade e) {
		if ( e.Point != this ) {
			return;
		}
        SelectedObj.SetActive(true);
		_actionType = e.ActionType;
		_isPlaneFlyIn = true;
	}

	void OnPlaneOnTarget(Event_Plane_OnTarget e) {
        if ( !_isPlaneFlyIn ) {
            return;
        }
        if ( e.Point != this ) {
            return;
        }
		ExpireEvent();
	}

}
