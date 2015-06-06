using System;
using System.Collections.Generic;
using UnityEngine;

public class Panel_MainMenu : MonoBehaviour {

    private Animator _animator;

	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();

        MessageBus.Get.Subscribe<MainMenuEvent>(MainMenuHandler);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void MainMenuHandler(object sender, MainMenuEvent e)
    {
        if (e.Show)
        {
            _animator.SetTrigger("FadeIn");
        }
        else
        {
            _animator.SetTrigger("FadeOut");
        }
    }

    public void StartNewGame()
    {
        MessageBus.Get.Publish<StartNewGameEvent>(this, new StartNewGameEvent());
    }

    public void Quit()
    {
        MessageBus.Get.Publish<TerminateEvent>(this, new TerminateEvent());
    }
}
