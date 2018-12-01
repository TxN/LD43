using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventResultWindow : MonoBehaviour {
	public Text EventResultText = null;

	public void ShowResult(string resultText) {
		EventResultText.text = resultText;
		gameObject.SetActive(true);
		CancelInvoke("Hide");
		Invoke("Hide", 5f);
	}

	void Hide() {
		gameObject.SetActive(false);
	}
}
