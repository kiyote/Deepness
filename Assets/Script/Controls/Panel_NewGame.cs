using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Panel_NewGame : MonoBehaviour {

    private Animator _animator;

	void Start () {
        _animator = GetComponent<Animator>();
        UIEvents.ShowingNewGameMenu += Show;
        UIEvents.HidingNewGameMenu += Hide;
	}
	
    private void Show(object sender, EventArgs e)
    {
        _animator.SetTrigger("FadeIn");
    }

    private void Hide(object sender, EventArgs e)
    {
        _animator.SetTrigger("FadeOut");
    }

    public void CreateGame()
    {
        SceneEvents.Instance.CreateGame(500, 500);
    }
}
