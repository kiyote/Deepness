using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Panel_NewGame : MonoBehaviour {

    private Animator _animator;

	void Start () {
	}

    void Awake()
    {
        _animator = GetComponent<Animator>();
        MessageBus.Get().Subscribe<NewGameMenuEvent>(NewGameMenuHandler);
    }
	
    private void NewGameMenuHandler(object sender, NewGameMenuEvent e)
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

    private void Hide(object sender, EventArgs e)
    {
    }

    public void CreateGame()
    {
        MessageBus.Get().Publish<CreateGameEvent>(this, new CreateGameEvent(500, 500));
    }
}
