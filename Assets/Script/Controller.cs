using System;
using UnityEngine;
using Model.Map;

public class Controller : MonoBehaviour {

	void Start () 
    {
        SceneEvents.StartingNewGame += StartNewGame;
        SceneEvents.CreatingGame += CreateGame;

        SystemEvents.Instance.InitializeSystems(); // Cause all dynamic data to be loaded
        UIEvents.Instance.ShowMainMenu(); // Trigger the main menu

	}
	
    private void StartNewGame(object sender, EventArgs e)
    {
        UIEvents.Instance.HideMainMenu();
        UIEvents.Instance.ShowNewGameMenu();
    }

    private void CreateGame(object sender, CreateGameEventArgs e)
    {
        MapGenerator mg = new MapGenerator();
        mg.Generate(Game.Instance.Map, 500, 500);

        SceneEvents.Instance.CreateMap(Game.Instance.Map);
        UIEvents.Instance.HideNewGameMenu();
    }
}
