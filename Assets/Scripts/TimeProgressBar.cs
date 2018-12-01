using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeProgressBar : MonoBehaviour {
	public Image ProgressBar = null;

	private void Update() {
		var gs = GameState.Instance;
		var fill = gs.CurrentCompletionPercent;
		ProgressBar.fillAmount = fill;
	}
}
