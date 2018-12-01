using System.Collections.Generic;
using EventSys;
using UnityEngine;

public class PlayerLogic : MonoBehaviour {
	public GameObject Plane = null;
	public float PlaneOnTargetDelta = 0.25f;
	public float MaxSpawnRadius = 1500;
	public float MinSpawnRadius = 1000;
	public float PlaneSpeed = 10f;
	public float SpawnRadius = 50f;

	public List<RectTransform> SpawnPoints = new List<RectTransform>();

	public enum PlaneState {
		Ready,
		FlyIn,
		FlyOut
	}

	StoryMapPoint _currentPoint = null;

	PlaneState _state = PlaneState.Ready;
	Vector3 _dirVector;
	public PlaneState CurrentState {
		get {
			return _state;
		}
	}

	private void Update() {
		if ( _state != PlaneState.Ready && _currentPoint != null ) {

			float distance = Vector2.Distance(transform.position, _currentPoint.transform.position);
			if ( _state == PlaneState.FlyIn ) {
				if ( distance <= PlaneOnTargetDelta ) {
					_state = PlaneState.FlyOut;
					EventManager.Fire(new Event_Plane_OnTarget() { Point = _currentPoint });
				}
				else {
					FlyForward();
				}
			}
			else if ( _state == PlaneState.FlyOut ) {
				FlyForward();
			}
		} else {
			gameObject.SetActive(false);
		}
	}

	void FlyForward() {
		
		transform.position += (_dirVector * PlaneSpeed * Time.deltaTime);
	}

	public void OnInvisible() {
		if ( Plane == null || _state != PlaneState.FlyOut ) {
			return;
		}

		EventManager.Fire(new Event_Plane_Hidden());
		Plane.SetActive(false);
		_state = PlaneState.Ready;
	}

	public void StartFly(StoryMapPoint targetPoint) {
		_currentPoint = targetPoint;
		Vector2 newPosition = SpawnPoints[Random.Range(0, SpawnPoints.Count)].anchoredPosition;
		
		GetComponent<RectTransform>().anchoredPosition = newPosition;
		_dirVector = (_currentPoint.transform.position - transform.position).normalized;
		var targetPos = targetPoint.transform.position;
		var thisPos = transform.position;
		targetPos.x = targetPos.x - thisPos.x;
		targetPos.y = targetPos.y - thisPos.y;
		var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

		_state = PlaneState.FlyIn;
		Plane.SetActive(true);
	}
}
