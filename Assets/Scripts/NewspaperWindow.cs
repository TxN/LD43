using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class NewspaperWindow : MonoBehaviour {
    public Transform Back = null;
	public Text Title       = null;
	public Text Description = null;

    public List<Newspaper> Newspapers = new List<Newspaper>();

    Sequence _seq = null;

    public void Show(int index) {
        if ( index > Newspapers.Count - 1 ) {
            return;
        }
        GameState.Instance.AddPause(this);
        Title.text = Newspapers[index].Title;
        Description.text = Newspapers[index].Description;
        gameObject.SetActive(true);
        Back.localScale = Vector3.zero;
        _seq = TweenHelper.ReplaceSequence(_seq);
        _seq.Append(Back.DOScale(1, 0.35f));
    }

    public void Hide() {
        _seq = TweenHelper.ReplaceSequence(_seq);
        _seq.Append(Back.DOScale(0, 0.35f));
        _seq.AppendCallback(() => { gameObject.SetActive(false); GameState.Instance.RemovePause(this); });
    }

    void OnDestroy() {
        _seq = TweenHelper.ResetSequence(_seq);
        if ( GameState.Instance != null ) {
            GameState.Instance.RemovePause(this);
        }
    }
}

[System.Serializable]
public class Newspaper {
    public string Title;
    public string Description;
}
