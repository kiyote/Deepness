using System;
using UnityEngine;
using Model.Map;

public class Controller : MonoBehaviour {

    void Awake()
    {
        MessageBus.Get().Subscribe<StartNewGameEvent>(StartNewGameHandler);
        MessageBus.Get().Subscribe<CreateGameEvent>(CreateGameHandler);
        MessageBus.Get().Subscribe<TerminateEvent>(TerminateHandler);
    }

	void Start() 
    {
        MessageBus.Get().Publish<InitializeSystemEvent>(this, new InitializeSystemEvent()); // Cause all dynamic data to be loaded
        MessageBus.Get().Publish<MainMenuEvent>(this, new MainMenuEvent(true)); // Show the main menu
	}
	
    private void StartNewGameHandler(object sender, StartNewGameEvent e)
    {
        MessageBus.Get().Publish<MainMenuEvent>(this, new MainMenuEvent(false)); // hide the main menu
        MessageBus.Get().Publish<NewGameMenuEvent>(this, new NewGameMenuEvent(true)); // show the new game menu
    }

    private void CreateGameHandler(object sender, CreateGameEvent e)
    {
        MapGenerator mg = new MapGenerator();
        mg.Generate(Game.Instance.Map, 500, 500);

        MessageBus.Get().Publish<CreateMapEvent>(this, new CreateMapEvent(Game.Instance.Map));
        MessageBus.Get().Publish<NewGameMenuEvent>(this, new NewGameMenuEvent(false));
    }

    private void TerminateHandler(object sender, TerminateEvent e)
    {
        Application.Quit();
    }
}
