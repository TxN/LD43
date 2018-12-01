using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryMapPoint : MonoBehaviour {
    public EventType Type = EventType.Combat;
    public bool Free = true;

    public SpriteRenderer Renderer = null;

    public Sprite WarIcon = null;
    public Sprite HumIcon = null;

    StoryEvent _curEvent = null;

    public void SetupEvent(StoryEvent storyEvent) {
        if ( !Renderer ) {
            Renderer = GetComponentInChildren<SpriteRenderer>();
        }
        _curEvent = storyEvent;
        // get event info from manager, setup, etc
        Free = false;
    }

    public void FinishEvent() {

    }


}
