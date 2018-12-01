using EventSys;
using UnityEngine;

public class PlayerLogic : MonoBehaviour {
	public GameObject Plane = null;
	public float PlaneOnTargetDelta = 0.25f;
	public float MaxSpawnRadius = 1500;
	public float MinSpawnRadius = 1000;
	public float PlaneSpeed = 10f;
	public float SpawnRadius = 50f;

	public enum PlaneState {
		Ready,
		FlyIn,
		FlyOut
	}

	StoryMapPoint _currentPoint = null;

	PlaneState _state = PlaneState.Ready;

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
		transform.Translate(
			transform.TransformDirection(
				Vector3.forward * PlaneSpeed * Time.deltaTime
			)
		);
	}

	void OnBecameInvisible() {
		if ( Plane == null || _state == PlaneState.FlyIn ) {
			return;
		}

		EventManager.Fire(new Event_Plane_Hidden());
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
