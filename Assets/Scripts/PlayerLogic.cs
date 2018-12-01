using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSys;

public class PlayerLogic : MonoBehaviour {
    public GameObject Plane = null;
    public float PlaneOnTargetDelta = 5f;
    public float MaxSpawnRadius = 1500;
    public float MinSpawnRadius = 1000;

    public enum PlaneState {
        Ready,
        FlyIn,
        FlyOut
    }

    StoryMapPoint _currentPoint = null;

    PlaneState _state = PlaneState.Ready;

	private void Update()
	{
        float distance = Vector3.Distance(transform.position, _currentPoint.transform.position);
        if (_state == PlaneState.FlyIn && distance <= PlaneOnTargetDelta) {
            _state = PlaneState.FlyOut;
            EventManager.Fire<Event_Plane_OnTarget>(new Event_Plane_OnTarget());
        } else if (_state == PlaneState.FlyOut && distance > PlaneOnTargetDelta) {
            EventManager.Fire<Event_Plane_FlyOut>(new Event_Plane_FlyOut());
        }
	}
}
