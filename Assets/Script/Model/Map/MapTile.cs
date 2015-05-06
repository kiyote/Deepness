
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
	private Wall _walls;
	//private Terrain _terrain;

	
	public MapTile(Map map, int column, int row)
	{
		_map = map;
		_column = column;
		_row = row;
		_isWall = false;
		_walls = Wall.None;
	}

	/*
	public Terrain Terrain 
	{
		get 
		{
			return _terrain;
		}
	}
	*/
	
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
	
	public Wall Walls
	{
		get
		{
			return _walls;
		}
		set
		{
			_walls = value;
		}
	}

}
