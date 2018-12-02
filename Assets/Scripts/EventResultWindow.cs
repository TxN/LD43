using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventSys;

public class EventResultWindow : MonoBehaviour {
	public Text EventResultText = null;

    public void Awake() {
        EventManager.Subscribe<Event_StoryPointDone>(this, OnStoryPointDone);
        gameObject.SetActive(false);
    }

	public void ShowResult(string resultText) {
		EventResultText.text = resultText;
		gameObject.SetActive(true);
		CancelInvoke("Hide");
		Invoke("Hide", 5f);
	}

	void Hide() {
		gameObject.SetActive(false);
	}

    void OnStoryPointDone(Event_StoryPointDone e) {
        if ( e.ActionType != StoryEvent.ResultOptions.ResultType.Nothing ) {
            var resultText = "";
            foreach (var res in GameState.Instance.StoryEventManager.GetEventById(e.EventId).StoryResults ) {
                if ( res.Type == e.ActionType ) {
                    resultText = res.ResultText;
                    break;
                }
	        }
            if ( !string.IsNullOrEmpty(resultText) ) {
                ShowResult(resultText);
            }
           
        }
    }

    void OnDestroy() {
        EventManager.Unsubscribe<Event_StoryPointDone>(OnStoryPointDone);
    }

}
