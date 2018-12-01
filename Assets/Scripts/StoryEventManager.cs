using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using EventSys;

public class StoryEventManager : MonoBehaviour {
    public List<StoryMapPoint> StorySpawnPoints = new List<StoryMapPoint>();
    public List<StoryEvent> Events = new List<StoryEvent>();

    public List<string> FirstEvents = new List<string>();

    int _curEventCount = 0;

    HashSet<string> _usedEvents = new HashSet<string>();

    void Awake() {
        Events = Resources.LoadAll<StoryEvent>("StoryEvents").ToList();
    }

    public void TryCreateNewEvent() {
        StoryEvent selectedEvent = null;

        if ( _curEventCount < FirstEvents.Count ) {
            selectedEvent = GetEventById(FirstEvents[_curEventCount]);
        } else {
            selectedEvent = GetRandomEvent();
        }

        if ( selectedEvent == null ) {
            return;
        }

        var point = GetFreeEventMapPoint(selectedEvent.Type);
        if ( point == null ) {
            return;
        }

        _usedEvents.Add(selectedEvent.Id);
        _curEventCount++;



        EventManager.Fire<Event_NewEventCreated>(new Event_NewEventCreated() { EventId = selectedEvent.Id, Point = point });
    }

    StoryEvent GetRandomEvent() {
        for ( int i = 0; i < 50; i++ ) {
            var index = Random.Range(0, Events.Count);
            if ( !_usedEvents.Contains(Events[i].Id) || Events[i].IsReusable ) {
                return Events[i];
            }
        }

        return null;
    }

    StoryMapPoint GetFreeEventMapPoint(EventType type) {
        foreach ( var point in StorySpawnPoints ) {
            if ( point.Type == type && point.Free ) {
                return point;
            }
        }
        return null;
    }

    public StoryEvent GetEventById(string id) {
        foreach ( var e in Events ) {
            if ( e.Id == id ) {
                return e;
            }
        }
        return null;
    }
}
