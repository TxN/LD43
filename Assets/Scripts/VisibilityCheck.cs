using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisibilityCheck : MonoBehaviour {

	public UnityEvent OnInvisible = null;

	private void OnBecameInvisible() {
		if ( OnInvisible != null ) {
			OnInvisible.Invoke();
		}
	}
}
