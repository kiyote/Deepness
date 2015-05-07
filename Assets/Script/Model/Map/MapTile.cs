
using System.Collections.Generic;

public class Fringe
{
	public Edge Edge;
	public Corner Corner;
}
	
public class MapTile
{
	private Map _map;
	private int _column;
	private int _row;
	private bool _isWall;
	private TileCompass _fringe;
	private Terrain _terrain;

	
	public MapTile(Map map, int column, int row)
	{
		_map = map;
		_column = column;
		_row = row;
		_isWall = false;
		_fringe = TileCompass.None;
	}

	public Terrain Terrain 
	{
		get 
		{
			return _terrain;
		}
        set
        {
            _terrain = value;
        }
	}
	
	public int Column
	{
		get
		{
			return _column;
		}
	}
	
	public int Row
	{
		get
		{
			return _row;
		}
	}
	
	public bool IsWall
	{
		get
		{
			return _isWall;
		}
		set
		{
			if (value != _isWall)
			{
				_isWall = value;
				_map.SignalMapChanged(this);
			}
		}
	}
	
	public TileCompass Fringe
	{
		get
		{
			return _fringe;
		}
		set
		{
			_fringe = value;
		}
	}

}
