public class Game
{
	private static Game _instance = new Game();

	private Map _map;

	public Game ()
	{
		_map = new Map();
	}

	public static Game Instance
	{
        get
        {
            return _instance;
        }
	}

	public Map Map
	{
		get
		{
			return _map;
		}
	}
}

