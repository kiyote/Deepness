public class Terrain
{
	private readonly int _id;
    private readonly string _name;
    private readonly bool _blocking;

	public Terrain( int id, string name, bool blocking )
	{
		_id = id;
        _name = name;
        _blocking = blocking;
	}

	public int Id
	{
		get 
		{
			return _id;
		}
	}

    public string Name
    {
        get
        {
            return _name;
        }
    }

    public bool Blocking
    {
        get
        {
            return _blocking;
        }
    }
}

