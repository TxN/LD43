using UnityEngine;

public class ImmortalObject : MonoBehaviour {

	void Start() {
		DontDestroyOnLoad(this.gameObject);
	}
}
