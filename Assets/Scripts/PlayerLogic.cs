using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSys;

public class PlayerLogic : MonoBehaviour {
    public GameObject Plane = null;
    public float PlaneOnTargetDelta = 5f;
    public float MaxSpawnRadius = 1500;
    public float MinSpawnRadius = 1000;
    public float PlaneSpeed = 100f;
    public float SpawnRadius = 50f;

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
        if (_state == PlaneState.FlyIn) {
            if (distance <= PlaneOnTargetDelta)
            {
                _state = PlaneState.FlyOut;
                EventManager.Fire<Event_Plane_OnTarget>(new Event_Plane_OnTarget());
            } else {
                FlyForward();
            }
        } else if (_state == PlaneState.FlyOut) {
            FlyForward();
        }
	}

    void FlyForward()
    {
        transform.Translate(
            transform.TransformDirection(
                Vector3.forward * PlaneSpeed * Time.deltaTime
            )
        );
    }

    void OnBecameInvisible()
    {
        if (Plane == null || _state == PlaneState.FlyIn)
        {
            return;
        }
       
        EventManager.Fire<Event_Plane_Hidden>(new Event_Plane_Hidden());
        Plane.SetActive(false);
        _state = PlaneState.Ready;
    }

    public void StartFly(StoryMapPoint targetPoint) {
        _currentPoint = targetPoint;
        Vector3 newPosition = Random.onUnitSphere * SpawnRadius;
        transform.position = newPosition;
        transform.LookAt(_currentPoint.transform);

        _state = PlaneState.FlyIn;
        Plane.SetActive(true);
    }
}
