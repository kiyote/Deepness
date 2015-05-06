using System;
using UnityEngine;

public class Controller : MonoBehaviour {

	void Start () 
    {
        SceneEvents.StartingNewGame += StartNewGame;
        SceneEvents.CreatingGame += CreateGame;
	}
	
    private void StartNewGame(object sender, EventArgs e)
    {
        UIEvents.Instance.HideMainMenu();
        UIEvents.Instance.ShowNewGameMenu();
    }

    private void CreateGame(object sender, CreateGameEventArgs e)
    {
        Game.Instance.Map.Create(e.MapWidth, e.MapHeight);
        SceneEvents.Instance.CreateMap(Game.Instance.Map);
        UIEvents.Instance.HideNewGameMenu();
    }
}
