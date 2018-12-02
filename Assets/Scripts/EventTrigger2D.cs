using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger2D : MonoBehaviour {
	public UnityEvent OnTrigger = null;

	private void OnTriggerEnter2D(Collider2D collision) {
		if ( OnTrigger != null ) {
			OnTrigger.Invoke();
		}
	}
}
