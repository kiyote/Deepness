using System;
using System.Collections.Generic;
using UnityEngine;

public class Panel_MainMenu : MonoBehaviour {

    private Animator _animator;

	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();

        UIEvents.ShowingMainMenu += Show;
        UIEvents.HidingMainMenu += Hide;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void Show(object sender, EventArgs e)
    {
        _animator.SetTrigger("FadeIn");
    }

    private void Hide(object sender, EventArgs e)
    {
        _animator.SetTrigger("FadeOut");
    }

    public void StartNewGame()
    {
        SceneEvents.Instance.StartNewGame();
    }
}
