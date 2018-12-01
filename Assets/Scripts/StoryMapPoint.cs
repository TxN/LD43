using System.Collections;
using System.Collections.Generic;
using EventSys;
using UnityEngine;

public class StoryMapPoint : MonoBehaviour {
    public GameObject Plane = null;
    public EventType Type = EventType.Combat;
    public bool Free = true;

    public SpriteRenderer Renderer = null;

    public Sprite WarIcon = null;
    public Sprite HumIcon = null;

    StoryEvent _curEvent = null;
    float _liveTimer = 0f;
    bool _isPlaneFlyIn = false;

	private void Start()
	{
        EventManager.Subscribe<Event_PlayerActionMade>(this, onPlayerActionMade);
        EventManager.Subscribe<Event_Plane_FlyIn>(this, onPlaneFlyIn);
	}

	private void Update()
	{
        if (!GameState.Instance.IsPause && !_isPlaneFlyIn) {
            _liveTimer += Time.deltaTime;
        }
	}

	private void OnDestroy()
	{
        EventManager.Unsubscribe<Event_PlayerActionMade>(onPlayerActionMade);
	}

	public void SetupEvent(StoryEvent storyEvent) {
        if ( !Renderer ) {
            Renderer = GetComponentInChildren<SpriteRenderer>();
        }
        _curEvent = storyEvent;
        // get event info from manager, setup, etc
        Free = false;
        _liveTimer = 0f;
    }

    public void FinishEvent() {

        _liveTimer = 0f;
        _isPlaneFlyIn = false;
        Free = true;
    }

    void onPlayerActionMade(Event_PlayerActionMade e) {
        if (e.Point != this) {
            return;
        }

        Plane.GetComponent<PlayerLogic>().StartFly(this);
    }

    void onPlaneFlyIn(Event_Plane_FlyIn e)
    {
        _isPlaneFlyIn = true;
    }

}
